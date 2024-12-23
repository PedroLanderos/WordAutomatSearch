using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranscriptionApi.Application.Interfaces;
using TranscriptionApi.Domain.Entities;
using System.Net.Http;
using System.Diagnostics;
using Vosk;
using NAudio.Wave;

namespace TranscriptionApi.Infrastructure.Services
{
    public class TranscriptionService : ITranscription
    {
        public async Task<TranscriptionEntity> TranscribeAsync(string videoUrl)
        {
			try
			{
                string videoPath = await DownLoadVideoAsync(videoUrl);
                string audioFilePath = Path.Combine(Path.GetDirectoryName(videoPath), Guid.NewGuid().ToString() + ".wav");
                ExtractAudio(videoPath, audioFilePath);

                string transcript = TranscribeAudio(audioFilePath);

                return new TranscriptionEntity
                {
                    VideoUrl = videoUrl,
                    Transcript = transcript
                };
            }
			catch (Exception)
			{
				throw new Exception();
			}
        }

		private async Task<string> DownLoadVideoAsync(string videoUrl)
		{
            string tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "TempFiles");
            string videoFileName = Guid.NewGuid().ToString() + ".mp4";
            string videoPath = Path.Combine(tempFolder, videoFileName);

			try
			{
                Directory.CreateDirectory(tempFolder);
                using (var httpClient = new HttpClient())
				{
					var response = await httpClient.GetAsync(videoUrl);
					response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
					{
                        using (var fileStream = new FileStream(videoPath, FileMode.Create, FileAccess.Write))
						{
                            await stream.CopyToAsync(fileStream);
                        }
                    }

                }
                return videoPath;
            }
			catch (Exception)
			{
                throw new Exception("Error al descargar el video");
            }
		}

        private void ExtractAudio(string videoFilePath, string audioFilePath)
        {
			try
			{
                string ffmpegArgs = $"-i \"{videoFilePath}\" -vn -acodec pcm_s16le -ar 16000 -ac 1 \"{audioFilePath}\"";

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = ffmpegArgs,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();
            }
			catch (Exception)
			{
				throw new Exception("error al extraer el audio");
			}
        }

        private string TranscribeAudio(string audioFilePath)
        {
            try
            {
                string modelPath = Path.Combine(Directory.GetCurrentDirectory(), "TranscriptionApi.Infrastructure", "VoskModels", "vosk-model-es-0.42");

                if (!Directory.Exists(modelPath))
                {
                    throw new Exception($"El modelo de Vosk no se encuentra en la ruta: {modelPath}");
                }

                var model = new Model(modelPath);

                using (var waveReader = new WaveFileReader(audioFilePath))
                {
                    var recognizer = new VoskRecognizer(model, waveReader.WaveFormat.SampleRate);
                    var buffer = new byte[4096];
                    int bytesRead;
                    var transcript = new StringBuilder();

                    while ((bytesRead = waveReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (recognizer.AcceptWaveform(buffer, bytesRead))
                        {
                            transcript.AppendLine(recognizer.Result());
                        }
                        else
                        {
                            transcript.AppendLine(recognizer.PartialResult());
                        }
                    }

                    transcript.AppendLine(recognizer.FinalResult());
                    return transcript.ToString();
                }

            }
            catch (Exception)
            {

                throw new Exception("error mientras se descargaba el modelo");
            }
        }
    }
}
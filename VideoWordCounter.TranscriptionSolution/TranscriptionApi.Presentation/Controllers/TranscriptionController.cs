using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranscriptionApi.Application.DTOs;
using TranscriptionApi.Application.Interfaces;

namespace TranscriptionApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptionController(ITranscription transcriptionInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<TranscriptionDTO>> GetText(string videoUrl)
        {
            var transcribe = await transcriptionInterface.TranscribeAsync(videoUrl);
            if (transcribe is null) return NotFound("el proceso no pudo hacerse");

            return Ok(transcribe);
        }
    }
}

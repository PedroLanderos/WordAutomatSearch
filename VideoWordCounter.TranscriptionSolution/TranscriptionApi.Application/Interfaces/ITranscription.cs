using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranscriptionApi.Domain.Entities;

namespace TranscriptionApi.Application.Interfaces
{
    public interface ITranscription
    {
        //trasncript video
        Task<TranscriptionEntity> TranscribeAsync(string videoUrl);
    }
}

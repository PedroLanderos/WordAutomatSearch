using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptionApi.Domain.Entities
{
    public class TranscriptionEntity
    {
        public string VideoUrl { get; set; }
        public string Transcript { get; set; }
    }
}

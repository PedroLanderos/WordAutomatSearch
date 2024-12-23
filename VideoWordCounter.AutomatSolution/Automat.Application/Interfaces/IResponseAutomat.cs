using Automat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat.Application.Interfaces
{
    public interface IResponseAutomat
    {
        ResponseEntity GetWordFrecuency(string word, string text);
    }
}

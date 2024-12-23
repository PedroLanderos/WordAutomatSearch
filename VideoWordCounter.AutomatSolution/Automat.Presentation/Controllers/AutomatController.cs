using Automat.Application.Interfaces;
using Automat.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Automat.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomatController(IResponseAutomat automatInterface) : ControllerBase
    {
        [HttpGet]
        public ActionResult<ResponseEntity> GetWordFrec(string word, string text)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("La palabra y el texto no pueden estar vacíos.");
            }
            var response = automatInterface.GetWordFrecuency(word, text);
            return Ok(response);
        }
    }
}

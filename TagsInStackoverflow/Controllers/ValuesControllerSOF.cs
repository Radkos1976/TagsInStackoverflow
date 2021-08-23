using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TagsInStackoverflow.Model;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TagsInStackoverflow.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesControllerSOF : ControllerBase
    {
        private readonly ILogger<ValuesControllerSOF> _logger;

        public ValuesControllerSOF(ILogger<ValuesControllerSOF> logger)
        {
            _logger = logger;
        }
        // GET: api/<ValuesControllerSOF>
        [HttpGet]
        public Task<List<Item>> Get()
        {
            GetFromSOF TagsSOF = new();            
            return  TagsSOF.Lst_Of_Tags();
        }    

        // GET api/<ValuesControllerSOF>/5
        [HttpGet("{Number_Tags}")]
        public Task<List<Item>> Get(int Number_Tags)
        {
            GetFromSOF TagsSOF = new();
            TagsSOF.Number_Tags = Number_Tags;            
            return TagsSOF.Lst_Of_Tags();
        }

    }
}

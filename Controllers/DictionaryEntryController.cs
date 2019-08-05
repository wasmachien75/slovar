using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace slovar.Controllers
{
    [Route("api")]
    [ApiController]
    public class DictionaryEntryController : ControllerBase
    {
        DictionaryEntryContext _context;
        public DictionaryEntryController()
        {
            _context = new DictionaryEntryContext();
        }
        [HttpGet("id/{id}")]
        public ActionResult<DictionaryEntry> Get(int id)
        {
            return _context.DictionaryEntries.Find(id);
        }

        [Route("search")]
        [HttpGet]
        public ActionResult<DictionaryEntrySearchResult> Search()
        {
            Microsoft.Extensions.Primitives.StringValues searchValue;
            Request.Query.TryGetValue("startsWith", out searchValue);
            if (searchValue.Count == 0)
            {
                return BadRequest();
            }
            return new DictionaryEntrySearchResult()
            {
                Results = _context.DictionaryEntries
                    .Where(de => de.Lemma.StartsWith(searchValue.First().ToLower()))
                    .Take(10)
                    .ToArray()
            };

        }

    }
}

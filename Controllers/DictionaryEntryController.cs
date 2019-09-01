using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;

namespace Slovar.Controllers
{
    [Route("api")]
    [ApiController]
    public class DictionaryEntryController : ControllerBase
    {
        DictionaryEntryContext _context;
        IMemoryCache _cache;
        public DictionaryEntryController(IMemoryCache cache)
        {
            _context = new DictionaryEntryContext();
            _cache = cache;

        }
        [HttpGet("id/{id}")]
        public ActionResult<DictionaryEntry> Get(int id)
        {
            return _context.DictionaryEntries.Find(id);
        }

        [HttpGet("lemma/{lemma}")]
        public ActionResult<DictionaryEntry> Get(string lemma)
        {
            return _context.DictionaryEntries.FirstOrDefault(de => de.LemmaForSearch == lemma);
        }

        [HttpGet("random")]
        public ActionResult<DictionaryEntry> Random()
        {
            return _context.DictionaryEntries.FromSql("SELECT * FROM DictionaryEntries ORDER BY RANDOM() LIMIT 1").First();
        }

        [Route("search")]
        [HttpGet]
        public ActionResult<DictionaryEntrySearchResult> Search()
        {
            Request.Query.TryGetValue("startsWith", out StringValues searchValue);
            if (searchValue.Count == 0)
            {
                return BadRequest();
            }
            var singleSearchValue = searchValue.First();
            DictionaryEntrySearchResult cacheResult;
            if (!_cache.TryGetValue(singleSearchValue, out cacheResult))
            {
                String key = TransformLemmaForSearch(searchValue.First().ToLower());
                var result = new DictionaryEntrySearchResult()
                {
                    Results = _context.DictionaryEntries
                    .Where(de => de.LemmaForSearch.StartsWith(key))
                    .OrderBy(de => de.LemmaForSearch)
                    .Take(10)
                    .ToArray()
                };
                _cache.Set(singleSearchValue, result, new TimeSpan(5, 0, 0, 0, 0));
                return result;
            }
            return cacheResult;

        }

        private string TransformLemmaForSearch(string lemma)
        {
            return new LemmaForSearchTransformer(lemma).Construct();
        }

    }
}

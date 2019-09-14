using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;
using Slovar.Models;

namespace Slovar.Controllers
{
    [Route("api")]
    [ApiController]
    public class DictionaryEntryController : ControllerBase
    {
        DictionaryContext _context;
        IMemoryCache _cache;
        public DictionaryEntryController(IMemoryCache cache, DictionaryContext context)
        {
            _cache = cache;
            _context = context;

        }
        [HttpGet("id/{id}")]
        public ActionResult<DictionaryEntry> Get(int id)
        {
            return _context.DictionaryEntries.Find(id);
        }

        [HttpGet("lemma/{lemma}")]
        public ActionResult<DictionaryEntryForClient> Get(string lemma)
        {
            var entry = _context.DictionaryEntries
                .Include(de => de.Usages)
                .FirstOrDefault(de => de.LemmaForSearch == lemma);

            if (entry == null)
            {
                NotFound();
            }
            return new DictionaryEntryForClient(entry);
        }

        [HttpGet("random")]
        public ActionResult<DictionaryEntry> Random()
        {
            return _context.DictionaryEntries.FromSql("SELECT * FROM DictionaryEntries ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
        }

        [HttpGet("search")]
        public ActionResult<DictionaryEntrySearchResult> Search([FromQuery] string startsWith)
        {
            if (!_cache.TryGetValue(startsWith, out DictionaryEntrySearchResult cacheResult))
            {
                String key = TransformLemmaForSearch(startsWith.ToLower());
                var result = new DictionaryEntrySearchResult()
                {
                    Results = _context.DictionaryEntries
                    .Include(e => e.Usages)
                    .Where(de => de.LemmaForSearch.StartsWith(key))
                    .OrderBy(de => de.LemmaForSearch)
                    .Take(10)
                    .Select(e => new DictionaryEntryForClient(e))
                    .ToList()
                };
                _cache.Set(startsWith, result, new TimeSpan(5, 0, 0, 0, 0));
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

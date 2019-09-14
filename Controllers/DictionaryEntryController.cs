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
            var cacheKey = "entry_" + lemma;
            if (!_cache.TryGetValue(cacheKey, out DictionaryEntryForClient cacheResult))
            {
                var entry = _context.DictionaryEntries
                    .Include(de => de.Usages)
                    .FirstOrDefault(de => de.LemmaForSearch == lemma);

                if (entry == null)
                {
                    NotFound();
                }
                var entryForClient = new DictionaryEntryForClient(entry);
                _cache.Set("entry_" + lemma, entryForClient, TimeSpan.FromDays(1));
                return entryForClient;
            }
            return cacheResult;
        }

        [HttpGet("random")]
        public ActionResult<DictionaryEntry> Random()
        {
            return _context.DictionaryEntries.FromSql("SELECT * FROM DictionaryEntries ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
        }

        [HttpGet("search")]
        public ActionResult<DictionaryEntrySearchResult> Search([FromQuery] string startsWith)
        {
            var cacheKey = "search_" + startsWith;
            if (!_cache.TryGetValue(cacheKey, out DictionaryEntrySearchResult cacheResult))
            {
                String key = TransformLemmaForSearch(startsWith.ToLower());
                var result = new DictionaryEntrySearchResult()
                {
                    Results = _context.DictionaryEntries
                    .Where(de => de.LemmaForSearch.StartsWith(key))
                    .OrderBy(de => de.LemmaForSearch)
                    .Take(10)
                    .Select(e => e.Lemma)
                    .ToList()
                };
                _cache.Set(cacheKey, result, TimeSpan.FromDays(1));
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

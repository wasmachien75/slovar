using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("lemma/{lemma}")]
        public ActionResult<DictionaryEntryForClient> Get(string lemma)
        {
            var normalizedLemma = TransformLemmaForSearch(lemma);
            var cacheKey = "entry_" + normalizedLemma;
            if (!_cache.TryGetValue(cacheKey, out DictionaryEntryForClient cacheResult))
            {
                var entry = _context.DictionaryEntries
                    .Include(de => de.Usages)
                    .FirstOrDefault(de => de.LemmaForSearch == normalizedLemma);

                if (entry == null)
                {
                    return NotFound();
                }
                var entryForClient = new DictionaryEntryForClient(entry);
                _cache.Set(cacheKey, entryForClient, TimeSpan.FromDays(1));
                return entryForClient;
            }
            return cacheResult;
        }

        [HttpGet("random")]
        public ActionResult<DictionaryEntryForClient> Random()
        {
            var entry = _context.DictionaryEntries.FromSql("SELECT * FROM DictionaryEntries ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
            return new DictionaryEntryForClient(entry);
        }

        [HttpGet("search")]
        public ActionResult<DictionaryEntrySearchResult> Search([FromQuery] string startsWith)
        {
            var cacheKey = "search_" + startsWith;
            var lang = IdentifyLanguage(startsWith);
            if (!_cache.TryGetValue(cacheKey, out DictionaryEntrySearchResult cacheResult))
            {
                String key = TransformLemmaForSearch(startsWith.ToLower());
                var result = new DictionaryEntrySearchResult()
                {
                    Results = _context.DictionaryEntries
                    .Where(de => lang == Language.Russian ? de.LemmaForSearch.StartsWith(key) : de.Translation.Contains(key))
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

        private Language IdentifyLanguage(string text)
        {
            foreach (char c in text)
            {
                if (c >= 65 && c <= 122)
                {
                    return Language.English;
                }
                if (c >= 1024 && c <= 1279)
                {
                    return Language.Russian;
                }
            }
            return Language.Russian; //default
        }

        private string TransformLemmaForSearch(string lemma)
        {
            return new LemmaNormalizer(lemma).Normalize();
        }

    }
}

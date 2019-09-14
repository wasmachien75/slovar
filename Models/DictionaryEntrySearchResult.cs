using System.Collections.Generic;

namespace Slovar.Models
{
    public class DictionaryEntrySearchResult
    {
        public IEnumerable<DictionaryEntryForClient> Results { get; set; }
    }
}
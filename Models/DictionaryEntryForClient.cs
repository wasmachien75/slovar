using Slovar;
using System.Collections.Generic;
using System.Linq;

namespace Slovar.Models
{
    public class DictionaryEntryForClient
    {
        public string Lemma { get; set; }
        public string Definition { get; set; }
        public string Translation { get; set; }
        public IEnumerable<Usage> Usages { get; set; }
        public int? StressIndex { get; set; }

        public DictionaryEntryForClient(DictionaryEntry entry)
        {
            this.Definition = entry.Definition;
            this.Lemma = entry.Lemma;
            this.Usages = entry.Usages?.Take(5);
            this.Translation = entry.Translation;
            this.StressIndex = entry.StressIndex;
        }
    }
}

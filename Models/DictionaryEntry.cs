using Microsoft.AspNetCore.Mvc;

namespace slovar
{
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    public class DictionaryEntry
    {
        public int Id { get; set; }
        public string Lemma { get; set; }
        public string Definition { get; set; }
        public int? StressIndex { get; set; }
    }
}
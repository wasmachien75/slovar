using Microsoft.AspNetCore.Mvc;

namespace Slovar
{
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    public class DictionaryEntry
    {
        private string _lemma;
        public int Id { get; set; }
        public string Lemma
        {
            get
            {
                return _lemma;
            }
            set
            {
                _lemma = value;
                LemmaForSearch = new LemmaForSearchTransformer(_lemma).Construct();
            }
        }
        public string Definition { get; set; }
        public int? StressIndex { get; set; }
        public string Translation { get; set; }
        public string LemmaForSearch { get; private set; }
    }
}
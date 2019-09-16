namespace Slovar
{
    public class LemmaNormalizer
    {
        string _lemma;
        public LemmaNormalizer(string lemma)
        {
            _lemma = lemma;
        }
        public string Normalize()
        {
            return _lemma.Replace('ё', 'е');
        }
    }
}
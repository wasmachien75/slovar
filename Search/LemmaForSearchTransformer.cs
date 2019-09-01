namespace Slovar
{
    public class LemmaForSearchTransformer
    {
        string _lemma;
        public LemmaForSearchTransformer(string lemma)
        {
            _lemma = lemma;
        }
        public string Construct()
        {
            return _lemma.Replace('ё', 'е');
        }
    }
}
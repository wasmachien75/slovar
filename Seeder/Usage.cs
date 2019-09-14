using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Slovar
{
    public class UsageSeeder
    {
        private string _filePath = @"D:\Code\slovar\Slovar\Migrations\corpora.xml";
        private DictionaryContext _ctx;
        public UsageSeeder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DictionaryContext>();
            optionsBuilder.UseSqlite("Data source=dict.db");
            _ctx = new DictionaryContext(optionsBuilder.Options);
        }
        private bool IsCyrillic(char c)
        {
            return c >= 1040 && c <= 1103;
        }

        private DictionaryEntry FindCorrespondingEntry(string lemma)
        {
            return _ctx.DictionaryEntries.FirstOrDefault(e => e.Lemma == lemma);
        }
        public void Seed()
        {
            var document = XDocument.Load(new FileStream(_filePath, FileMode.Open));
            var sentences = document.Root
                .Descendants("sentence");
            sentences = sentences.Where(s => IsCyrillic(s.Element("source").Value.First()));

            var tokens = sentences.Descendants("token").Descendants("l");
            int cnt = 0;
            foreach (var token in tokens)
            {
                var normalizedToken = token.Attribute("t").Value;
                if (IsCyrillic(normalizedToken.First()))
                {
                    var entry = FindCorrespondingEntry(normalizedToken);
                    if (entry != null)
                    {
                        Usage u = new Usage()
                        {
                            Entry = entry,
                            Sentence = token.Ancestors("sentence").FirstOrDefault().Element("source").Value,
                            Position = token.Ancestors("token").FirstOrDefault().ElementsBeforeSelf().Count() + 1
                        };
                        _ctx.Usages.Add(u);
                        cnt++;
                        if (cnt % 500 == 0)
                        {
                            _ctx.SaveChanges();
                            Console.WriteLine($"Saved changes (#{cnt}");
                        }
                    }

                }
            }
            _ctx.SaveChanges();
        }
    }
}
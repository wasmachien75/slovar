using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Slovar.Models;
using System.IO;
using System.Text;

namespace Slovar.Seeders
{

    public class EntryComparer : IComparer<DictionaryEntry>
    {
        public int Compare(DictionaryEntry first, DictionaryEntry second)
        {
            return first.Id;
        }
    }
    public class TranslationSeeder
    {
        public void Query()
        {
            var map = GetTranslationMap();
            var optionsBuilder = new DbContextOptionsBuilder<DictionaryContext>();
            optionsBuilder.UseSqlite("Data source=dict.db");
            var _context = new DictionaryContext(optionsBuilder.Options);
            var lastId = _context.DictionaryEntries.OrderBy(e => e.Id).Last().Id;

            var last = 0;
            while (last < lastId)
            {
                var set = _context.DictionaryEntries
                           .Where(e => e.Id > last)
                           .OrderBy(e => e.Id)
                           .Take(1000);
                foreach (var entry in set)
                {
                    _context.Update(entry);
                    string translation;
                    if (map.TryGetValue(entry.Lemma, out translation))
                    {
                        entry.Translation = translation;
                    };

                }
                last = set.Last().Id;
                _context.SaveChanges();
            }


        }

        public Dictionary<string, string> GetTranslationMap()
        {

            Dictionary<string, string> translationMap = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(new FileStream(@"C:\Temp\Ru-En-Smirnitsky.dsl - Copy", FileMode.Open));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var translationBuilder = new StringBuilder();
                while (char.IsWhiteSpace((char)reader.Peek()))
                {
                    translationBuilder.AppendLine(reader.ReadLine());

                }
                translationMap.Add(line.Trim(), translationBuilder.ToString().Trim());
            }
            return translationMap;

        }

    }
}
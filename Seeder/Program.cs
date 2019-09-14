using System;
using Microsoft.EntityFrameworkCore;
using Slovar;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Seeder
{
    public class Seeder
    {
        public static void Translate()
        {
            var s = new TranslationSeeder();
            s.Query();
        }

        public static void SetStressIndex()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DictionaryContext>();
            optionsBuilder.UseSqlite("Data source=dict.db");
            var _context = new DictionaryContext(optionsBuilder.Options);
            using (var inputStream = new FileStream(@"C:\Users\Willem\Downloads\ru-ru_ozhegov_shvedova_cc_v2_0.dsl", FileMode.Open))
            {
                int cnt = 0;
                StreamReader reader = new StreamReader(inputStream);
                while (!reader.EndOfStream)
                {
                    while (char.IsWhiteSpace((char)reader.Peek()))
                    {
                        reader.ReadLine();
                    }
                    string lemma = reader.ReadLine();
                    DictionaryEntry entry = _context.DictionaryEntries.Where(de => de.Lemma == lemma).FirstOrDefault();
                    if (entry != null)
                    {
                        string definition = reader.ReadLine();
                        if (definition.Contains("_"))
                        {
                            Match match = Regex.Match(definition, "[А-Я]+(?=_)");
                            if (match.Success)
                            {
                                _context.Update(entry);
                                entry.StressIndex = match.Value.Length;
                                cnt++;
                                if (cnt % 500 == 0)
                                {
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                _context.SaveChanges();

            }
        }
        static void AddInitial()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DictionaryContext>();
            optionsBuilder.UseSqlite("Data source=dict.db");
            var _context = new DictionaryContext(optionsBuilder.Options);
            using (var inputStream = new FileStream(@"Seeder\dictionary.dsl", FileMode.Open))
            {
                Parser p = new Parser(inputStream);
                var entries = p.ParseEntries();
                int cnt = 0;
                foreach (var entry in entries)
                {
                    _context.Add(entry);
                    Console.WriteLine($"Add {entry.Lemma}");
                    if (cnt % 1000 == 0)
                    {
                        _context.SaveChanges();
                    }
                    cnt++;
                }
                _context.SaveChanges();
            };
        }

    }
    class Parser
    {
        //lines that start with no tab are new entries.
        //following lines that start with a tab form the definition of that entry.
        Stream _inputStream;
        StreamReader reader;

        public Parser(Stream inputStream)
        {
            _inputStream = inputStream;
            reader = new StreamReader(_inputStream);
        }

        public IEnumerable<DictionaryEntry> ParseEntries()
        {
            while (reader.Peek() != -1)
            {
                yield return ParseSingleEntry();
            }
        }

        private DictionaryEntry ParseSingleEntry()
        {
            var entry = new DictionaryEntry();
            string line = reader.ReadLine();
            var definitionBuilder = new StringBuilder();
            while (char.IsWhiteSpace((char)reader.Peek()))
            {
                definitionBuilder.Append(reader.ReadLine());

            }
            entry.Lemma = line.Trim();
            entry.Definition = definitionBuilder.ToString().Trim();
            return entry;
        }
    }
}

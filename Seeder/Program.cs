using System;
using Microsoft.EntityFrameworkCore;
using slovar;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Seeder
{
    class Program
    {
        static void _Main(string[] args)
        {
            var _context = new DictionaryEntryContext();
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
        bool reachedEnd = false;
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

using NUnit.Framework;
using Slovar.Controllers;
using Slovar.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Slovar.Tests
{
    public class DictionaryEntryControllerTest
    {
        SqliteConnection _connection;
        DictionaryContext _context;
        MemoryCache _cache;
        DictionaryEntryController controller;

        [SetUp]
        public void Setup()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<DictionaryContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new DictionaryContext(options);
            _context.Database.EnsureCreated();
            _cache = new MemoryCache(new MemoryCacheOptions());
            controller = new DictionaryEntryController(_cache, _context);

        }

        [Test]
        public void RandomTest()
        {
            _context.Add(new DictionaryEntry()
            {
                Lemma = "BabyPoeder",
                Definition = "Poeder voor baby's"
            });
            _context.SaveChanges();
            var entry = controller.Random().Value;
            Assert.AreEqual(entry.Lemma, "BabyPoeder");
        }

        [Test]
        public void GetEntryWithTransformableLemma()
        {
            _context.Add(new DictionaryEntry()
            {
                Lemma = "ёлка",
                Definition = "jajaja"
            });
            _context.SaveChanges();
            var result = controller.Get("ёлка");
            Assert.NotNull(result.Value);

        }

        [Test]
        public void CacheTest()
        {
            var entry = new DictionaryEntry()
            {
                Lemma = "abc",
                Definition = "def"
            };
            _context.Add(entry);
            _context.SaveChanges();
            controller.Get(entry.Lemma);
            Assert.AreEqual(_cache.Count, 1);
            controller.Get(entry.Lemma); //get it from the cache
        }

        [Test]
        public void SearchTest()
        {
            _context.Add(new DictionaryEntry()
            {
                Lemma = "abc",
                Definition = "def",
                Usages = new List<Usage>(){new Usage(){
                    Sentence = "abc gaat naar de winkel.",
                    Position = 1
                    }
                }
            });
            _context.SaveChanges();
            var results = controller.Search("abc").Value;
            Assert.AreEqual(results.Results.Count(), 1);
        }

        [Test]
        public void SearchFromCacheTest()
        {
            _context.Add(new DictionaryEntry()
            {
                Lemma = "abc"
            });
            _context.SaveChanges();
            var result = controller.Search("abc").Value;
            Assert.AreEqual(_cache.Count, 1);
            result = controller.Search("abc").Value; //get it from the cache

        }

        [Test]
        public void LemmaNotFoundTest()
        {
            Assert.True(controller.Get("lepel").Result is NotFoundResult);
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }

    }
}
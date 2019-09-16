using System;
using Microsoft.EntityFrameworkCore;
using Slovar.Models;

namespace Slovar
{
    public class DictionaryContext : DbContext
    {
        public DbSet<DictionaryEntry> DictionaryEntries { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DictionaryContext(DbContextOptions<DictionaryContext> context) : base(context) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DictionaryEntry>().HasKey(de => de.Id);
            builder.Entity<DictionaryEntry>().Property(de => de.Id).ValueGeneratedOnAdd();
            builder.Entity<Usage>().HasKey(de => de.Id);
            builder.Entity<Usage>().Property(de => de.Id).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
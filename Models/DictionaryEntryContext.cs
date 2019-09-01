using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Slovar
{
    public class DictionaryEntryContext : DbContext
    {
        public DbSet<DictionaryEntry> DictionaryEntries { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=dict.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DictionaryEntry>().HasKey(de => de.Id);
            builder.Entity<DictionaryEntry>().Property(de => de.Id).ValueGeneratedOnAdd();
        }
    }
}
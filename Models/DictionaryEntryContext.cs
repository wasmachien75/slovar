using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace slovar
{
    public class DictionaryEntryContext : DbContext
    {
        public DbSet<DictionaryEntry> DictionaryEntries { get; set; }
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=dict.db");
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DictionaryEntry>().HasKey(de => de.Id);
            builder.Entity<DictionaryEntry>().Property(de => de.Id).ValueGeneratedOnAdd();
        }
    }
}
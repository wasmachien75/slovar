﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Slovar;

namespace Slovar.Migrations
{
    [DbContext(typeof(DictionaryContext))]
    partial class DictionaryEntryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Slovar.DictionaryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Definition");

                    b.Property<string>("Lemma");

                    b.Property<string>("LemmaForSearch");

                    b.Property<int?>("StressIndex");

                    b.Property<string>("Translation");

                    b.HasKey("Id");

                    b.ToTable("DictionaryEntries");
                });

            modelBuilder.Entity("Slovar.Usage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EntryId");

                    b.Property<int>("Position");

                    b.Property<string>("Sentence");

                    b.HasKey("Id");

                    b.HasIndex("EntryId");

                    b.ToTable("Usages");
                });

            modelBuilder.Entity("Slovar.Usage", b =>
                {
                    b.HasOne("Slovar.DictionaryEntry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using slovar;

namespace slovar.Migrations
{
    [DbContext(typeof(DictionaryEntryContext))]
    [Migration("20190821180952_add-searchstring")]
    partial class addsearchstring
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("slovar.DictionaryEntry", b =>
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
#pragma warning restore 612, 618
        }
    }
}

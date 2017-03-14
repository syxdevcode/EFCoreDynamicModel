using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure;

namespace Infrastructure.Migrations.DynamicModelDb
{
    [DbContext(typeof(DynamicModelDbContext))]
    [Migration("20170314063651_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity("BareDiamond", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color")
                        .HasMaxLength(20);

                    b.Property<string>("Size")
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("BareDiamond");

                    b.HasAnnotation("SqlServer:TableName", "BareDiamond");
                });
        }
    }
}

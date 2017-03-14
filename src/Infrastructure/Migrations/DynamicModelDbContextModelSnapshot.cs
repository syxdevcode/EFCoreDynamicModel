using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DynamicModelDbContext))]
    partial class DynamicModelDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

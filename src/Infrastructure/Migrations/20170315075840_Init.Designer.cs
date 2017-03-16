using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DynamicModel.Infrastructure;

namespace DynamicModel.Infrastructure.Migrations
{
    [DbContext(typeof(ModelDbContext))]
    [Migration("20170315075840_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DynamicModel.Domain.RuntimeModelMeta", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<string>("ModelName");

                    b.Property<string>("Properties");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ModelId");

                    b.ToTable("Metas");
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure;

namespace Infrastructure.Migrations.ModelDb
{
    [DbContext(typeof(ModelDbContext))]
    partial class ModelDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DynamicModel.Domain.RuntimeModelMeta", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<string>("ModelName");

                    b.HasKey("ModelId");

                    b.ToTable("Metas");
                });

            modelBuilder.Entity("DynamicModel.Domain.RuntimeModelMeta+ModelPropertyMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsRequired");

                    b.Property<int>("Length");

                    b.Property<string>("Name");

                    b.Property<string>("PropertyName");

                    b.Property<int?>("RuntimeModelMetaModelId");

                    b.Property<string>("ValueType");

                    b.HasKey("Id");

                    b.HasIndex("RuntimeModelMetaModelId");

                    b.ToTable("ModelPropertyMeta");
                });

            modelBuilder.Entity("DynamicModel.Domain.RuntimeModelMeta+ModelPropertyMeta", b =>
                {
                    b.HasOne("DynamicModel.Domain.RuntimeModelMeta")
                        .WithMany("Properties")
                        .HasForeignKey("RuntimeModelMetaModelId");
                });
        }
    }
}

using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Infrastructure
{
    public class DynamicModelDbContext : DbContext
    {
        private readonly IRuntimeModelProvider _modelProvider;
        private readonly ICoreConventionSetBuilder _builder;
        private readonly IMemoryCache _cache;
        private static string DynamicCacheKey = "DynamicModel"; 

        public DynamicModelDbContext(DbContextOptions<DynamicModelDbContext> options, ICoreConventionSetBuilder builder, IRuntimeModelProvider modelProvider, IMemoryCache cache) : base(options)
        {
            _modelProvider = modelProvider;
            _builder = builder;
            _cache = cache; 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //直接从缓存读取model，如果不存在再build
            IMutableModel model = _cache.GetOrCreate(DynamicCacheKey, entry =>
            {
                var modelBuilder = new ModelBuilder(_builder.CreateConventionSet());
                Type[] runtimeModels = _modelProvider.GetTypes();
                foreach (var item in runtimeModels)
                {
                    modelBuilder.Model.AddEntityType(item).SqlServer().TableName = item.Name;
                }
                _cache.Set(DynamicCacheKey, modelBuilder.Model);

                return modelBuilder.Model;
            });

            optionsBuilder.UseModel(model);
            base.OnConfiguring(optionsBuilder);
        }

    }
}
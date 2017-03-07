using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLib;

namespace Infrastructure
{
    public class DynamicModelDbContext: DbContext
    {
        private readonly IRuntimeModelProvider _modelProvider;

        public DynamicModelDbContext(DbContextOptions<DynamicModelDbContext> options, IRuntimeModelProvider modelProvider) : base(options)
        {
            _modelProvider = modelProvider;
        }

        public DbSet<RuntimeModelMeta> Metas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //_modelProvider就是我们上面定义的IRuntimeModelProvider,通过依赖注入方式获取到实例
            Type[] runtimeModels = _modelProvider.GetTypes("product");
            foreach (var item in runtimeModels)
            {
                builder.Model.AddEntityType(item);
            }

            base.OnModelCreating(builder);
        }
    }
}

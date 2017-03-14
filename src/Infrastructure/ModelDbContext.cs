using DynamicModel.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicModel.Infrastructure
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
        { }

        public DbSet<RuntimeModelMeta> Metas { get; set; }
    }
}

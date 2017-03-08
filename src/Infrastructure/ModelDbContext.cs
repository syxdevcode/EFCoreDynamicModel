using Microsoft.EntityFrameworkCore;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext(DbContextOptions<DynamicModelDbContext> options) : base(options)
        { }

        //public DbSet<RuntimeModelMeta> Metas { get; set; }
    }
}

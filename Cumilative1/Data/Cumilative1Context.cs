using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cumilative1.Models;

namespace Cumilative1.Data
{
    public class Cumilative1Context : DbContext
    {
        public Cumilative1Context (DbContextOptions<Cumilative1Context> options)
            : base(options)
        {
        }

        public DbSet<Cumilative1.Models.Teacher> Teacher { get; set; } = default!;
    }
}

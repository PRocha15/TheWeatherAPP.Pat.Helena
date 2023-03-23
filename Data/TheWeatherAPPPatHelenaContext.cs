using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWeatherAPP.Pat.Helena.Models;

namespace TheWeatherAPP.Pat.Helena.Data
{
    public class TheWeatherAPPPatHelenaContext : DbContext
    {
        public TheWeatherAPPPatHelenaContext (DbContextOptions<TheWeatherAPPPatHelenaContext> options)
            : base(options)
        {
        }

        public DbSet<TheWeatherAPP.Pat.Helena.Models.Users> Users { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri.Data
{
    public class AgriConfigurationContext : DbContext
    {
        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options) : base (options)
        {
        }

        public DbSet<Browser> Browsers { get; set; }
        public DbSet<Animal> Animals { get; set; }

        
    }
}

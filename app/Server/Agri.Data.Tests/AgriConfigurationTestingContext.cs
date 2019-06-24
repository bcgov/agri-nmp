using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Agri.Data.Tests
{
    public class AgriConfigurationTestingContext : AgriConfigurationContext
    {
        public AgriConfigurationTestingContext(DbContextOptions<AgriConfigurationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>()
                .Property(e => e.DefaultNitrogenCredit)
                .HasConversion(v => new ArrayWrapper(v), v => v.Values);

            base.OnModelCreating(modelBuilder);
        }

        // Note this makes the mapper happy, but won't be called in 2.1.
        private struct ArrayWrapper
        {
            public ArrayWrapper(int[] values) => Values = values;

            public int[] Values { get; }
        }
    }
}
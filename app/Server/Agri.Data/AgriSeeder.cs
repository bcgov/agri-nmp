using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Data
{
    public class AgriSeeder
    {
        private AgriConfigurationContext _context;

        public AgriSeeder(AgriConfigurationContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            //If the database is not present or if migrations are required
            //create the database and/or run the migrations
            _context.Database.EnsureCreated();

            var saveChanges = false;
            if (!_context.AmmoniaRetentions.Any())
            {

                saveChanges = true;
            }

            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }
    }
}

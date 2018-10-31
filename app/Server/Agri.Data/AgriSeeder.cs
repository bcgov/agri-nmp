using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.LegacyData.Models.Impl;

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
            var staticData = new StaticData();
            var staticExtraLists = new StaticDataExtraLists();

            if (!_context.AmmoniaRetentions.Any())
            {
                var ammoniaRetentions = staticExtraLists.GetAmmoniaRetentions();
                _context.AmmoniaRetentions.AddRange(ammoniaRetentions);
            }

            if (!_context.Animals.Any())
            {
                var animals = staticData.GetAnimals();
                var animalSubtypes = staticData.GetAnimalSubTypes();
                foreach (var animal in animals)
                {
                    var subtypes = animalSubtypes.Where(s => s.AnimalId == animal.Id).ToList();
                    if (subtypes.Any())
                    {
                        animal.AnimalSubTypes.AddRange(subtypes);
                    }
                }
                _context.Animals.AddRange(animals);

                var types = staticData.GetAnimalSubTypes();
            }
            
            _context.SaveChanges();
        }
    }
}

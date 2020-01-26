using Agri.Data;
using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.CalculateService
{
    public interface IFeedAreaCalculator
    {
        decimal GetK20AgronomicBalance(Field field);

        decimal GetNitrogenAgronomicBalance(Field field);

        decimal GetP205AgronomicBalance(Field field);
    }

    public class FeedAreaCalculator : IFeedAreaCalculator
    {
        private readonly AgriConfigurationRepository _repo;
        private readonly AgriConfigurationContext _context;

        public FeedAreaCalculator(AgriConfigurationRepository repo, AgriConfigurationContext context)
        {
            _repo = repo;
            _context = context;
        }

        public decimal GetNitrogenAgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }

        public decimal GetP205AgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }

        public decimal GetK20AgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }
    }
}
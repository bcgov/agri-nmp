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
        decimal K20AgronomicBalance(Field field);

        decimal NAgronomicBalance(Field field);

        decimal P205AgronomicBalance(Field field);
    }

    public class FeedAreaCalculator : IFeedAreaCalculator
    {
        private readonly AgriConfigurationRepository _repo;

        public FeedAreaCalculator(AgriConfigurationRepository repo)
        {
            _repo = repo;
        }

        public decimal NAgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }

        public decimal P205AgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }

        public decimal K20AgronomicBalance(Field field)
        {
            var result = 0M;

            return result;
        }
    }
}
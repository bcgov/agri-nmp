using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.CalculateService
{
    public interface IFeedAreaCalculator
    {
        decimal K20AgronomicBalance();

        decimal NAgronomicBalance();

        decimal P205AgronomicBalance();
    }

    public class FeedAreaCalculator : IFeedAreaCalculator
    {
        public decimal NAgronomicBalance()
        {
            var result = 0M;

            return result;
        }

        public decimal P205AgronomicBalance()
        {
            var result = 0M;

            return result;
        }

        public decimal K20AgronomicBalance()
        {
            var result = 0M;

            return result;
        }
    }
}
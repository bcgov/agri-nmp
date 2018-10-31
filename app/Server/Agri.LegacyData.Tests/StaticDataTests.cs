using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.LegacyData.Models.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agri.LegacyData.Tests
{
    [TestClass]
    public class StaticDataTests
    {
        [TestMethod]
        public void GetAnimalSubTypesSuccessfully()
        {
            var staticData = new StaticData();
            var result = staticData.GetAnimalSubTypes();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        
    }
}

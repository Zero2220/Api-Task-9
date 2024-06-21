using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Testing
{
    [TestClass]
    public class UnitTestCalc
    {

        [TestMethod]
        public void Toplama()
        {
            int x = 10;
            int y = 20;

            int m = x + y;

            Assert.AreEqual(30, m);

        }

        [TestMethod]
        public void Cixma()
        {
            int x = 10;
            int y = 20;

            int m = x - y;

            Assert.AreEqual(30, m);

        }
    }
}

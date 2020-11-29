using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimeManagement.Tests
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Multiply(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Multiply(2, 2));
        }

        int Multiply(int x, int y)
        {
            return x * y;
        }
    }
}

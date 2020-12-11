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
        public void Test1()
        {
            Assert.Equal(4, Multiply(2, 2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(6, Multiply(2, 3));
        }

        int Multiply(int x, int y)
        {
            return x * y;
        }
    }
}

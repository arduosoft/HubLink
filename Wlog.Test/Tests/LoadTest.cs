using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Wlog.Test.Tests
{

    [Trait("Category", "LoadTest")]
    class LoadTest
    {
        [Fact]
        public void AlwaysFailButSkipped()
        {
            Assert.True(1 != 1);
        }
    }
}

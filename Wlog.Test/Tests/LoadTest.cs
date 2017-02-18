using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Wlog.Test.Tests
{
    /// <summary>
    /// this test contains test used in LoadTest sessions
    /// this test are skypped during CI and runned only manually
    /// usually one at time, starting from a clean installation
    /// to evalutate system performance.
    /// </summary>
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

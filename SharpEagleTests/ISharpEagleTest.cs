using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEagleTests
{
    public interface ISharpEagleTest
    {
        bool Test(out string testResults);
    }
}

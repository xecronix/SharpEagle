using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEagleTests
{
    class BaseTest
    {
        public List<ISharpEagleTest> Tests = new List<ISharpEagleTest>();
        public void AddTest(ISharpEagleTest test)
        {
            Tests.Add(test);
        }

        public bool Test(out string testResult)
        {
            int fail = 0;
            int pass = 0;
            bool retval = true;
            testResult = "";

            Console.Out.Write(System.Environment.NewLine + GetType().Name + System.Environment.NewLine);
            foreach (ISharpEagleTest test in Tests)
            {
                try
                {
                    string results = "";
                    if (test.Test(out results) == true)
                    {
                        Console.Out.Write(".");
                        pass++;
                    }
                    else
                    {
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine(results);
                        fail++;
                    }
                }
                catch (Exception ex)
                {
                    fail++;
                    string err = string.Format("ERROR: {0}{1}", test.GetType().Name, System.Environment.NewLine);
                    testResult += err + ex.ToString() + System.Environment.NewLine;
                }

            }

            Console.Out.Write(System.Environment.NewLine);
            if (fail > 0)
            {
                Console.Out.WriteLine("FAIL");
            }
            else
            {
                Console.Out.WriteLine("PASS");
            }

            Console.Out.WriteLine(string.Format("\tFail: {0}", fail));
            Console.Out.WriteLine(string.Format("\tPass: {0}", pass));

            retval = fail == 0;
            return retval;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEagle;


namespace SharpEagleTests
{
    public class SharpEagleTester
    {
        // 
        // Simple test runner
        //
        List<ISharpEagleTest> Tests = new List<ISharpEagleTest>();
        public bool Add(ISharpEagleTest test) { Tests.Add(test); return true; }
        public bool Test()
        {
            bool retval = true;
            foreach (ISharpEagleTest test in Tests)
            {

                string testOutput = "";
                if (test.Test(out testOutput) == false)
                {
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("FAIL: ***");
                    Console.Out.WriteLine(testOutput);
                    retval = false;
                    break;  // no more testing
                }
                else
                {
                    Console.Out.Write(testOutput + "");
                }
                
            }
            return retval;
        }
        static void Main(string[] args)
        {
            SharpEagleTester tester = new SharpEagleTester();
            tester.Add(new TestSubstitutionTag());
            tester.Add(new TestActionTag());

            tester.Test();
            Console.In.ReadLine();
        }
    }
}


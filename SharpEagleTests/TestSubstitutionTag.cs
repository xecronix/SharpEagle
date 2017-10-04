using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEagle;

namespace SharpEagleTests
{
    class TestSubstitutionTag : ISharpEagleTest
    {
        SharpEagle.Template Eagle = new SharpEagle.Template();
        List<ISharpEagleTest> Tests = new List<ISharpEagleTest>();
        public bool Test(out string testResult)
        {
            int fail = 0;
            int pass = 0;
            bool retval = true;
            Tests.Add(new SimpleSubstitutionTest());
            Tests.Add(new OpenTagTest());
            Tests.Add(new IgnoreOpenSymbolTest());
            Tests.Add(new IgnoreWhiteSpaceTest());
            Tests.Add(new MissingSubstitutionTagTest());
            testResult = "";
            int testNumber = 0;

            foreach (ISharpEagleTest test in Tests)
            {
                testNumber++;
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
                        testResult += results + System.Environment.NewLine;
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

            Console.Out.Write("\nTestSubstitutionTag: ");
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

    public class SimpleSubstitutionTest : ISharpEagleTest
    {
        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {=substitution:} test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("substitution", "cool");
            string result = new SharpEagle.Template().Parse(template, tags);
            string expected = "This is a cool test!";
            if (result != expected)
            {
                results = string.Format("SimpleSubstitutionTest: Result [{0}] is not [{1}]", result, expected);
                pass = false;
            }
            else
            {
                results = "PASS";
            }
            return pass;
        }
    }

    public class OpenTagTest : ISharpEagleTest
    {
        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {=substitution: test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("substitution", "cool");
            try
            {
                string result = new SharpEagle.Template().Parse(template, tags);
                pass = false;
                results = "OpenTagTest: Template with an open tag did not throw an exception but it should have.";
            }
            catch (Exception ex)
            {
                pass = true;
                results = "PASS";
            }
            return pass;
        }
    }

    public class IgnoreOpenSymbolTest : ISharpEagleTest
    {
        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {{=substitution:}} test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("substitution", "cool");
            string result = new SharpEagle.Template().Parse(template, tags);
            string expected = "This is a {cool} test!";
            if (result != expected)
            {
                results = string.Format("IgnoreOpenSymbol: Result [{0}] is not [{1}]", result, expected);
                pass = false;
            }
            else
            {
                results = "PASS";
            }
            return pass;
        }
    }

    public class IgnoreWhiteSpaceTest : ISharpEagleTest
    {
        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {{=  substitution  :}} test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("substitution", "cool");
            string result = new SharpEagle.Template().Parse(template, tags);
            string expected = "This is a {cool} test!";
            if (result != expected)
            {
                results = string.Format("IgnoreWhiteSpaceTest: Result [{0}] is not [{1}]", result, expected);
                pass = false;
            }
            else
            {
                results = "PASS";
            }
            return pass;
        }
    }

    public class MissingSubstitutionTagTest : ISharpEagleTest
    {
        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {{=  zsubstitution  :}} test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("substitution", "cool");
            string result = new SharpEagle.Template().Parse(template, tags);
            string expected = "This is a {{=  zsubstitution  :}} test!";
            if (result != expected)
            {
                results = string.Format("IgnoreWhiteSpaceTest: Result [{0}] is not [{1}]", result, expected);
                pass = false;
            }
            else
            {
                results = "PASS";
            }
            return pass;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEagle;

namespace SharpEagleTests
{
    class TestSubstitutionTag : BaseTest, ISharpEagleTest
    {
        public TestSubstitutionTag()
        {
            AddTest(new SimpleSubstitutionTest());
            AddTest(new OpenTagTest());
            AddTest(new IgnoreOpenSymbolTest());
            AddTest(new IgnoreWhiteSpaceTest());
            AddTest(new MissingSubstitutionTagTest());
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
            string template = "This is a {=substitution: test!"; //this is a bad template
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

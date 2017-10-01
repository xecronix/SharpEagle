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
        public bool Test(out string testResult)
        {
            bool retval = true;
            testResult = "PASS";
            string template = "This is a {=substitution:} test!";
            SharpEagle.Template sharpEagle = new SharpEagle.Template();
            sharpEagle.AddTag(new Tag("substitution", "cool"));
            string result = sharpEagle.Parse(template, null);
            string expected = "This is a cool test!";
            if (result != expected)
            {
                testResult = string.Format("Result [{0}] is not [{1}]", result, expected);
                retval = false;
            }

            return retval;
        }
    }

    public class Tag : ITemplateTag
    {
        private string k;
        private string v;
        public Tag(string key, string value)
        {
            k = key;
            v = value;
        }

        public string TagText()
        {
            return k;
        }

        public string Value()
        {
            return v;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEagle;

namespace SharpEagleTests
{
    class TestActionTag : BaseTest, ISharpEagleTest
    {
        public TestActionTag()
        {
            AddTest(new SimpleActionTest());
            AddTest(new NestedSubTemplateTest());
        }
    }

    public class SimpleActionTest : ISharpEagleTest, ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> context)
        {
            SharpEagle.Template eagle = new SharpEagle.Template();
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags["descr"] = "cool";
            string retval = eagle.Parse(template, tags);
            return retval;
        }

        public bool Test(out string results)
        {
            bool pass = true;
            string template = "This is a {@action{=descr:}:} test!";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("action", this);
            string result = eagle.Parse(template, tags);
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

    public class NestedSubTemplateTest : ISharpEagleTest, ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("demographics", new DemographicsAction());
            Dictionary<string, string> newTags = new Dictionary<string, string>();
            newTags.Add("title", "This is a test for template nesting.");
            string retval = eagle.Parse(template, newTags).Trim();
            return retval;
        }

        public bool Test(out string results)
        {
            string template =
 @"{@nested {=title:}{@demographics
Name      {=Name:}
Country   {=Country:}
Christian {=Christian:}:}:}";

            string expected =
@"This is a test for template nesting.
Name      Xecronix
Country   Unknown
Christian Yes";
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("nested", this);
            Dictionary<string, string> tags = new Dictionary<string, string>();
            results = eagle.Parse(template, tags);
            bool retval = expected.Equals(results); 
            return retval; 
        }
    }

    public class DemographicsAction : ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            SharpEagle.Template eagle = new SharpEagle.Template();
            Dictionary<string, string> newTags = new Dictionary<string, string>();
            newTags.Add("Name", "Xecronix");
            newTags.Add("Country", "Unknown");
            newTags.Add("Christian", "Yes");
            eagle.AddAction("demographics", new DemographicsAction());
            string retval = eagle.Parse(template, newTags);
            return retval;
        }
    }

}

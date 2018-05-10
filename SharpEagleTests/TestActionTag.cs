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
            AddTest(new NestedSubTemplateLoopingTest());
            AddTest(new MultipleSequentialActionTest());
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
            string retval = eagle.Parse(template, newTags);
            return retval;
        }
    }

    public class NestedSubTemplateLoopingTest : ISharpEagleTest, ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("demographics", new LoopingDemographicsAction());
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
Name      Xecronix 1
Country   Unknown 1
Christian Yes 1
Name      Xecronix 2
Country   Unknown 2
Christian Yes 2
Name      Xecronix 3
Country   Unknown 3
Christian Yes 3";
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("nested", this);
            Dictionary<string, string> tags = new Dictionary<string, string>();
            results = eagle.Parse(template, tags);
            bool retval = expected.Equals(results);
            return retval;
        }
    }

    public class LoopingDemographicsAction : ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            string retval = "";
            SharpEagle.Template eagle = new SharpEagle.Template();
            
            for (int i = 1; i < 4; i++)
            {
                Dictionary<string, string> newTags = new Dictionary<string, string>();
                newTags.Add("Name", "Xecronix " + i);
                newTags.Add("Country", "Unknown " + i);
                newTags.Add("Christian", "Yes " + i);
                retval += eagle.Parse(template, newTags);
            }
            return retval;
        }
    }

    public class MultipleSequentialActionTest : ISharpEagleTest, ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            SharpEagle.Template eagle = new SharpEagle.Template();
            Dictionary<string, string> newTags = new Dictionary<string, string>();
            newTags.Add("title", "Multiple Sequential Action Test.");
            string retval = eagle.Parse(template, newTags).Trim();
            return retval;
        }

        public bool Test(out string results)
        {
            string template =
@"{@nested {=title:}:}
{@multiaction1{=sub:}:}";

            string expected =
@"Multiple Sequential Action Test.
MultiAction1 TEXT";
            SharpEagle.Template eagle = new SharpEagle.Template();
            eagle.AddAction("nested", this);
            eagle.AddAction("multiaction1", new MultiAction1());
            Dictionary<string, string> tags = new Dictionary<string, string>();
            results = eagle.Parse(template, tags);
            bool retval = expected.Equals(results);
            return retval;
        }
    }

    public class MultiAction1 : ITemplateAction
    {
        public string Run(string template, Dictionary<string, string> tags)
        {
            string retval = "";
            SharpEagle.Template eagle = new SharpEagle.Template();

            Dictionary<string, string> newTags = new Dictionary<string, string>();
            newTags.Add("sub", "MultiAction1 TEXT");
            retval += eagle.Parse(template, newTags);

            return retval;
        }
    }

}

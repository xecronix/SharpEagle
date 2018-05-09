using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateAction
    {
        string Run(string template, Dictionary<string, string> context);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateAction
    {
        string TagText();
        string Action(string template, Object[] contextData);
    }
}

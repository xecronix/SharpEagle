using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateAction
    {
        string Action(string template, List<ITemplateTag> context);
    }
}

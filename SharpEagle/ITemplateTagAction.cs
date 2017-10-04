using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateTagAction
    {
        string Action(List<ITemplateTag> context, List<string>args = null);
    }
}

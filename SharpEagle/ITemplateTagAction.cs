using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateTagAction
    {
        string TagText();
        string Action(Object[] contextData);
    }
}

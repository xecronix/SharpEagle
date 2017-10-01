using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEagle
{
    public interface ITemplateTag 
    {
        string TagText();
        string Value();
    }
}

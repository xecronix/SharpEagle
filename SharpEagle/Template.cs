using System;

namespace SharpEagle
{
    public class Template

    {
        private enum State { SKIP, ECHO, TAG_OPENING, TAG_OPEN, READ_TAG_DATA, TAG_CLOSING, TAG_CLOSED, ERROR}
        private enum Tag { NONE, ACTION, SUBSTITUTION }
        public string Parse(string template, Object [] contextData) { return ""; }
        private string DoTag() { return ""; }
        private string DoAction() {return ""; }
        public string AddTag(ITemplateTag action) { return ""; }
        public string AddAction(ITemplateAction action) { return ""; }
        public string AddTagAction(ITemplateTagAction action) { return ""; }
    }
}

using System;
using System.Collections.Generic;

namespace SharpEagle
{
    public class Template

    {
        private enum State { SKIP, ECHO, TAG_OPENING, TAG_OPEN, READ_TAG_DATA, TAG_CLOSING, TAG_CLOSED, ERROR}
        private enum TagType { NONE, ACTION, SUBSTITUTION }
        private string DoTag() { return ""; }
        private string DoAction() { return ""; }

        public string AddAction(string key, ITemplateAction action) { return ""; }
        public string AddTagAction(string key, ITemplateTagAction action) { return ""; }
        public string Parse(string template, Dictionary<string, string> context)
        {
            State state = State.ECHO;
            TagType tagType = TagType.NONE;
            string err = "";
            string retval = "";
            string tag="";
            string subTemplate = "";
            string temp="";
            string unhandledTag = "";
            int nestingLevel = 0;            

            foreach (char c in template)
            {
                switch (state)
                {
                    case State.ERROR:
                        throw new Exception(err);
                    case State.ECHO:
                        {
                            if (c == '{')
                            {
                                temp = "{";
                                unhandledTag = "";
                                unhandledTag += c;
                                state = State.TAG_OPENING;
                            }
                            else
                            {
                                retval += c;
                            }
                            break;
                        }                        
                    case State.TAG_OPENING:
                        if (c == '=')
                        {
                            if (tagType == TagType.NONE)
                            {
                                tagType = TagType.SUBSTITUTION;
                            }
                            unhandledTag += c;
                            nestingLevel++;
                            state = State.SKIP;
                        }
                        else if (c == '@')
                        {
                            unhandledTag += c;
                            tagType = TagType.ACTION;
                            nestingLevel++;
                        }
                        else if (c == '{')
                        {
                            if (tagType != TagType.ACTION)
                            {
                                unhandledTag = "";
                                unhandledTag += c;
                                retval += temp;
                            }
                            else
                            {
                                unhandledTag = "";
                                unhandledTag += c;
                                subTemplate += temp;
                            }
                        }
                        else
                        {
                            retval += temp;
                            retval += c;
                            if (tagType != TagType.ACTION)
                            {
                                state = State.ECHO;
                            }
                        }
                        break;
                    case State.TAG_OPEN:
                        break;
                    case State.SKIP:
                        unhandledTag += c;
                        if ("\r\n\t ".IndexOf(c) == -1)
                        {
                            tag += c;
                            state = State.READ_TAG_DATA;
                        }
                        else
                        {
                            //continue skipping whitespace.
                        }
                        break;
                    case State.READ_TAG_DATA:
                        {
                            unhandledTag += c;
                            if (tagType == TagType.SUBSTITUTION)
                            {
                                if (c == ':')
                                {
                                    state = State.TAG_CLOSING;
                                    temp = ":";
                                }
                                else
                                {
                                    tag += c;
                                }
                            }
                            else if (tagType == TagType.ACTION)
                            {

                            }
                        }
                        break;
                    case State.TAG_CLOSING:
                        {
                            if (c == '}')
                            {
                                nestingLevel--;
                                if (tagType == TagType.SUBSTITUTION)
                                {
                                    if (context.ContainsKey(tag.Trim()))
                                    {
                                        retval += context[tag.Trim()];
                                    }
                                    else
                                    {
                                        retval += unhandledTag + c;
                                    }
                                }
                                
                                state = State.ECHO;
                            }
                            else
                            {
                                tag += temp;
                                temp = "";
                                state = State.READ_TAG_DATA;
                            }
                        }
                        break;
                    case State.TAG_CLOSED:
                        break;
                    default:
                        break;
                }
            }
            if (state != State.ECHO)
            {
                throw new Exception(string.Format("Parser Finished in a Bad STATE {0}", state));
            }
            return retval;
        }
    }
}

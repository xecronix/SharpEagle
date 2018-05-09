using System;
using System.Collections.Generic;

namespace SharpEagle
{
    public class Template

    {
        private enum State { SKIP, ECHO, TAG_OPENING, TAG_OPEN, READ_TAG_DATA, READ_SUB_TEMPLATE,TAG_CLOSING, TAG_CLOSED, ERROR}
        private enum TagType { NONE, ACTION, SUBSTITUTION }
        private Dictionary<string, ITemplateAction> TemplateActions = new Dictionary<string, ITemplateAction>();
        private string DoAction() { return ""; }

        public void AddAction(string key, ITemplateAction action) { TemplateActions.Add(key, action); }
        public void AddTagAction(string key, ITemplateTagAction action) { }
        public string Parse(string template, Dictionary<string, string> context)
        {
            if (context == null)
            {
                context = new Dictionary<string, string>();
            }

            if (template == null)
            {
                return "";
            }

            State state = State.ECHO;
            TagType tagType = TagType.NONE;
            string retval = "";
            string tag="";
            string subTemplate = "";
            string temp="";
            string unhandledTag = "";
            int nestingLevel = 0;            

            foreach (char c in template)
            {
                if (tagType == TagType.NONE)
                {
                    switch (state)
                    {
                        case State.ERROR:
                            {
                                break;
                            }

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
                            {
                                if (c == '=')
                                {
                                    if (tagType == TagType.NONE)
                                    {
                                        tagType = TagType.SUBSTITUTION;
                                    }
                                    unhandledTag += c;
                                    state = State.SKIP;
                                }
                                else if (c == '@')
                                {
                                    unhandledTag += c;
                                    tagType = TagType.ACTION;
                                    state = State.SKIP;
                                    nestingLevel++;
                                }
                                else if (c == '{')
                                {
                                    unhandledTag = "";
                                    unhandledTag += c;
                                    retval += temp;
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
                            }
                    }
                }
                else if (tagType == TagType.SUBSTITUTION)
                {
                    switch (state)
                    {
                        case State.ERROR:
                            {
                                break;
                            }

                        case State.SKIP:
                            {
                                unhandledTag += c;
                                if ("\r\n\t ".IndexOf(c) == -1)
                                {
                                    state = State.READ_TAG_DATA;
                                    tag += c;
                                }
                                break;
                            }

                        case State.READ_TAG_DATA:
                            {
                                unhandledTag += c;
                                if (c == ':')
                                {
                                    state = State.TAG_CLOSING;
                                    temp = ":";
                                }
                                else
                                {
                                    tag += c;
                                }
                                break;
                            }

                        case State.TAG_CLOSING:
                            {
                                if (c == '}')
                                {
                                    if (context.ContainsKey(tag.Trim()))
                                    {
                                        retval += context[tag.Trim()];
                                    }
                                    else
                                    {
                                        retval += unhandledTag + c;
                                    }
                                    state = State.ECHO;
                                    tag = "";
                                    unhandledTag = "";
                                    tagType = TagType.NONE;
                                }
                                else
                                {
                                    unhandledTag += temp + c;
                                    tag += temp + c;
                                    state = State.READ_TAG_DATA;

                                }
                                break;
                            }
                    }
                }
                else if (tagType == TagType.ACTION)
                {
                    switch (state)
                    {
                        case State.ERROR:
                            {
                                break;
                            }

                        case State.SKIP:
                            {
                                unhandledTag += c;
                                if ("\r\n\t ".IndexOf(c) == -1)
                                {
                                    state = State.READ_TAG_DATA;
                                    tag += c;
                                }
                                break;
                            }

                        case State.TAG_OPENING:
                            {
                                unhandledTag += c;
                                subTemplate += temp + c;
                                if (c == '@' || c == '=')
                                {
                                    nestingLevel++;
                                }
                                state = State.READ_SUB_TEMPLATE;
                                break;
                            }

                        case State.READ_SUB_TEMPLATE:
                            {
                                unhandledTag += c;
                                if (c == ':')
                                {
                                    temp = ":";
                                    state = State.TAG_CLOSING;
                                }
                                else if (c == '{')
                                {
                                    temp = "{";
                                    state = State.TAG_OPENING;
                                }
                                else
                                {
                                    subTemplate += c;
                                }
                                break;

                            }

                        case State.READ_TAG_DATA:
                            {
                                unhandledTag += c;
                                if ("\r\n\t{ ".IndexOf(c) != -1)
                                {
                                    if (c == '{')
                                    {
                                        temp = "{";
                                        state = State.TAG_OPENING;
                                    }
                                    else
                                    {
                                        subTemplate += c;
                                        state = State.READ_SUB_TEMPLATE;
                                    }
                                }
                                else
                                {
                                    tag += c;
                                }
                                break;
                            }

                        case State.TAG_CLOSING:
                            {
                                unhandledTag += c;
                                if (c == '}')
                                {
                                    nestingLevel--;
                                    if (nestingLevel == 0)
                                    {
                                        // TODO :: we need to do the callback
                                        if (TemplateActions.ContainsKey(tag) == true)
                                        {
                                            retval += TemplateActions[tag].Run(subTemplate, context);
                                            //retval += unhandledTag + c;
                                        }
                                        else
                                        {
                                            retval += unhandledTag + c;
                                        }
                                        tag = "";
                                        subTemplate = "";
                                        temp = "";
                                        unhandledTag = "";
                                        state = State.ECHO;
                                        tagType = TagType.NONE;
                                    }
                                    else
                                    {
                                        subTemplate += temp + c;
                                        state = State.READ_SUB_TEMPLATE;
                                    }
                                }
                                else
                                {
                                    state = State.READ_SUB_TEMPLATE;
                                    subTemplate += temp + c;
                                }
                                break;
                            }
                    }
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

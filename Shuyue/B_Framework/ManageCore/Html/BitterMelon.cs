using Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Html
{
    public class BitterMelon : HtmlModel
    {
        private string _html;
        private string _host;
        private string _absoluteUri;
        public BitterMelon(string html)
        {
            _html = html;
            HandleHtml();
        }

        public BitterMelon(Uri uri, string method = "get")
        {
            _host = uri.Host;
            _absoluteUri = uri.Scheme + "://" + uri.Host + uri.AbsolutePath;
            HttpHelper httpHelper = new HttpHelper();
            HttpItem item = new HttpItem
            {
                URL = uri.ToString(),
                Method = method,
            };
            HttpResult result = httpHelper.GetHtml(item);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _html = result.Html;
                _html = _html.Trim();
                HandleHtml();
            }
        }

        //处理html
        private void HandleHtml()
        {
            //整理html
            ArrangeHtml(_html);
            //获取html中常用属性的值
            GetHtmlValue();
            //获取链接
            GetHtmlLinks();
        }

        private void GetHtmlValue()
        {
            //获取各部分HTML代码
            base.AllHtml = MelonReg.GetMatchValue(CommonReg.AllHtml, _html);
            base.AllHead = MelonReg.GetMatchValue(CommonReg.AllHead, _html);
            base.AllBody = MelonReg.GetMatchValue(CommonReg.AllBody, _html);
            //获取title
            base.Head.Title = MelonReg.GetMatchValue(CommonReg.Title, base.AllHead);
            //获取meta标签属性
            Match metaMatch = MelonReg.GetMatch(CommonReg.Meta, base.AllHead);
            while (metaMatch.Success)
            {
                if (!base.Head.Meta.ContainsKey(metaMatch.Groups["name"].Value))
                {
                    base.Head.Meta.Add(metaMatch.Groups["name"].Value, metaMatch.Groups["content"].Value);
                }
                metaMatch = metaMatch.NextMatch();
            }
            //获取scripts
            Match scriptsMatch = MelonReg.GetMatch(CommonReg.Scripts, _html);
            while (scriptsMatch.Success)
            {
                base.AllScripts += scriptsMatch.Value + "|";
                scriptsMatch = scriptsMatch.NextMatch();
            }
        }

        /// <summary>
        /// 获取文本链接
        /// </summary>
        private void GetHtmlLinks()
        {
            Match linkMatch = MelonReg.GetMatch(CommonReg.Link, base.AllBody);
            while (linkMatch.Success)
            {
                string linkHref = linkMatch.Groups["href"].Value;
                string linkName = linkMatch.Groups["name"].Value;
                if (linkHref.Contains("www") || linkHref.StartsWith("http"))
                {
                    if (linkHref.Contains(_host))
                    {
                        base.LinkCollection.SitelinkCount++;
                    }
                    else
                    {
                        base.LinkCollection.OffSitelinkCount++;
                    }
                }
                else
                {
                    linkHref = (linkHref.StartsWith("/") ? "http://" + _host : _absoluteUri.EndsWith("/") ? _absoluteUri : _absoluteUri + "/") + linkHref;
                    base.LinkCollection.SitelinkCount++;
                }
                base.LinkCollection.Links.Add(new Link { FullLink = linkMatch.Value, Href = linkHref, Name = linkName });
                linkMatch = linkMatch.NextMatch();
            }
        }

        //整理html文档
        private void ArrangeHtml(string content)
        {
            try
            {
                HashSet<string> notChangeLineTag = new HashSet<string> { "!doctype", "meta", "link", "line", "!--[if", "![endif]--", "!--" };
                Stack<string> tagStack = new Stack<string>();//标签栈
                StringBuilder line = new StringBuilder();//存放行字符串的容器
                string tag = "";//标签
                string currentStr = "";//当前line中的字符串
                //循环html
                for (int i = 0; i < content.Length; i++)
                {
                    //判断为标签开始
                    if (content[i] == '<')
                    {
                        //获取当前标签名称
                        int j = i + 1;
                        while (j < content.Length && content[j] != '>' && content[j++] != ' ') ;
                        tag = content.Substring(i + 1, j - i - 1);
                        tag = tag.Replace("/", "").Trim();
                        //非正常开始标记
                        if (tag.Contains("<"))
                        {
                            line.Append(content[i]);
                            continue;
                        }
                        //标签开始之前有文本，或其他
                        if (line.Length > 0)
                        {
                            currentStr = line.ToString();
                            if (currentStr.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "") != "")
                            {
                                HtmlLines hl = new HtmlLines
                                {
                                    LineStr = currentStr,
                                    LineLevel = tagStack.Count
                                };
                                base.LineQueue.Enqueue(hl);
                            }
                        }
                        //重置行文本
                        line = new StringBuilder();
                        line.Append(content[i]);
                    }
                    //判断为标签结束
                    else if (content[i] == '>')
                    {
                        line.Append(content[i]);
                        currentStr = line.ToString();
                        //自闭合标签，直接加入队列
                        if (content[i - 1] == '/')
                        {
                            HtmlLines hl = new HtmlLines
                            {
                                LineStr = currentStr,
                                LineLevel = tagStack.Count,
                                TagName = tag
                            };
                            base.LineQueue.Enqueue(hl);
                        }
                        else
                        {
                            HtmlLines hl = new HtmlLines();
                            //闭合标签
                            if (currentStr.Trim().StartsWith("</"))
                            {
                                //此前最后一个标签文本
                                string ltag = "";
                                if (tagStack.Count > 0)
                                    ltag = tagStack.Pop();
                                //这里存在问题，如果html不规范，比如<span></a><span>，则会使整个html乱掉
                                //这里多pop出一个，是为了兼容<span><a></span>这种情况
                                //相比较来说第二种情况应该会比第一种情况出现的更频繁
                                if (ltag != tag && tagStack.Count > 0)
                                    ltag = tagStack.Pop();
                                hl.LineStr = currentStr;
                                hl.LineLevel = tagStack.Count;
                                hl.TagName = "/" + tag;
                                base.LineQueue.Enqueue(hl);
                            }
                            //标签
                            else
                            {
                                hl.LineStr = currentStr;
                                hl.LineLevel = tagStack.Count;
                                hl.TagName = tag;
                                base.LineQueue.Enqueue(hl);

                                if (!notChangeLineTag.Contains(tag.ToLower()) && !tag.StartsWith(" ") && !tag.StartsWith("!--") && !tag.Contains("--"))
                                    tagStack.Push(tag);
                            }
                        }
                        //重置行文本
                        line = new StringBuilder();
                    }
                    else
                    {
                        line.Append(content[i]);
                    }
                }
                //可能存在的乱七八糟的东西
                if (line.Length > 0)
                {
                    HtmlLines hl = new HtmlLines
                    {
                        LineStr = currentStr,
                        LineLevel = tagStack.Count
                    };
                    base.LineQueue.Enqueue(hl);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 获取合并行的html队列
        /// </summary>
        /// <returns></returns>
        public Queue<HtmlLines> GetMergedLineQueue()
        {
            try
            {
                bool isMerge = false;
                string mergeTagName = "";
                HashSet<string> mergeTagSet = new HashSet<string> { "script", "a", "title", "lable", "b", "h1", "h2", "h3", "h4", "i" };
                Queue<HtmlLines> newQueue = new Queue<HtmlLines>();
                string newLineStr = "";
                foreach (HtmlLines line in base.LineQueue)
                {
                    if (isMerge)
                    {
                        newLineStr += line.LineStr;
                        if (line.TagName != null && line.TagName == "/" + mergeTagName)
                        {
                            HtmlLines newHtmlLines = new HtmlLines
                            {
                                LineStr = newLineStr,
                                LineLevel = line.LineLevel,
                                TagName = mergeTagName,
                            };
                            newQueue.Enqueue(newHtmlLines);
                            isMerge = false;
                            mergeTagName = "";
                        }
                    }
                    else
                    {
                        if (line.TagName != null && mergeTagSet.Contains(line.TagName.ToLower()))
                        {
                            isMerge = true;
                            mergeTagName = line.TagName;
                            newLineStr = "";
                            newLineStr += line.LineStr;
                        }
                        else
                        {
                            HtmlLines newHtmlLines = line;
                            newQueue.Enqueue(newHtmlLines);
                        }
                    }
                }
                return newQueue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

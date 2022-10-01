using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TaskManager.Tasks.Utils
{
    public class HTMLFilter
    {
        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string Filter(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            Regex regex1 =
                  new Regex(@"<script[sS]+</script *>",
                  RegexOptions.IgnoreCase);
            Regex regex2 =
                  new Regex(@" href *= *[sS]*script *:",
                  RegexOptions.IgnoreCase);
            Regex regex3 =
                  new Regex(@" no[sS]*=",
                  RegexOptions.IgnoreCase);
            Regex regex4 =
                  new Regex(@"<iframe[sS]+</iframe *>",
                  RegexOptions.IgnoreCase);
            Regex regex5 =
                  new Regex(@"<frameset[sS]+</frameset *>",
                  RegexOptions.IgnoreCase);
            Regex regex6 =
                  new Regex(@"<img[^>]+>",
                  RegexOptions.IgnoreCase);
            Regex regex7 =
                  new Regex(@"</p>",
                  RegexOptions.IgnoreCase);
            Regex regex8 =
                  new Regex(@"<p>",
                  RegexOptions.IgnoreCase);
            Regex regex9 =
                  new Regex(@"<[^>]*>",
                  RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记   
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性   
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件   
            html = regex4.Replace(html, ""); //过滤iframe   
            html = regex5.Replace(html, ""); //过滤frameset   
            html = regex6.Replace(html, ""); //过滤frameset   
            html = regex7.Replace(html, ""); //过滤frameset   
            html = regex8.Replace(html, ""); //过滤frameset   
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");  
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = Regex.Replace(html, "[\f\n\r\t\v]", "");  //过滤回车换行制表符  
            return FilterXSS(html);
        }


        /// <summary>
        /// 过滤html(不过滤引号)
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterString(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            Regex regex1 =
                  new Regex(@"<script[sS]+</script *>",
                  RegexOptions.IgnoreCase);
            Regex regex2 =
                  new Regex(@" href *= *[sS]*script *:",
                  RegexOptions.IgnoreCase);
            Regex regex3 =
                  new Regex(@" no[sS]*=",
                  RegexOptions.IgnoreCase);
            Regex regex4 =
                  new Regex(@"<iframe[sS]+</iframe *>",
                  RegexOptions.IgnoreCase);
            Regex regex5 =
                  new Regex(@"<frameset[sS]+</frameset *>",
                  RegexOptions.IgnoreCase);
            Regex regex6 =
                  new Regex(@"<img[^>]+>",
                  RegexOptions.IgnoreCase);
            Regex regex7 =
                  new Regex(@"</p>",
                  RegexOptions.IgnoreCase);
            Regex regex8 =
                  new Regex(@"<p>",
                  RegexOptions.IgnoreCase);
            Regex regex9 =
                  new Regex(@"<[^>]*>",
                  RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记   
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性   
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件   
            html = regex4.Replace(html, ""); //过滤iframe   
            html = regex5.Replace(html, ""); //过滤frameset   
            html = regex6.Replace(html, ""); //过滤frameset   
            html = regex7.Replace(html, ""); //过滤frameset   
            html = regex8.Replace(html, ""); //过滤frameset   
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");  
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = Regex.Replace(html, "[\f\n\r\t\v]", "");  //过滤回车换行制表符  
            return FilterXSSString(html);
        }


        /// <summary>
        /// 过滤HTML标签
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTML(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            try
            {
                string result;
                result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("'", " ");
                result = result.Replace("\t", string.Empty);
                result = Regex.Replace(result, @"( )+", " ");
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*span([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&nbsp;", " ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&bull;", " * ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&frasl;", "/", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&copy;", "(c)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&reg;", "(r)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
                string breaks = "\r\r\r";
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
                return Filter(result);
            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }
        }

        /// <summary>
        /// 过滤HTML标签(不过滤引号)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTMLString(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            try
            {
                string result;
                result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("'", " ");
                result = result.Replace("\t", string.Empty);
                result = Regex.Replace(result, @"( )+", " ");
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*span([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&nbsp;", " ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&bull;", " * ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&frasl;", "/", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&copy;", "(c)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&reg;", "(r)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
                string breaks = "\r\r\r";
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
                return FilterString(result);
            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }
        }


        /// <summary>   
        /// 过滤HTML标签
        /// </summary>   
        /// <param name="input">传入字符串</param>   
        /// <returns>过滤后的字符串</returns>   
        public static string FilterXSS(string html)
        {

            Regex regex1 = new Regex(@">", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@"<", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"＞", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"＜", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex("\"", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"”", RegexOptions.IgnoreCase);

            html = regex1.Replace(html, "");
            html = regex2.Replace(html, "");
            html = regex3.Replace(html, "");
            html = regex4.Replace(html, "");
            html = regex5.Replace(html, "");
            html = regex6.Replace(html, "");

            return html;
        }


        /// <summary>   
        /// 过滤HTML标签(不过滤引号)
        /// </summary>   
        /// <param name="input">传入字符串</param>   
        /// <returns>过滤后的字符串</returns>   
        public static string FilterXSSString(string html)
        {

            Regex regex1 = new Regex(@">", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@"<", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"＞", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"＜", RegexOptions.IgnoreCase);


            html = regex1.Replace(html, "");
            html = regex2.Replace(html, "");
            html = regex3.Replace(html, "");
            html = regex4.Replace(html, "");

            return html;
        }

    }
}

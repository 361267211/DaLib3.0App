using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Standard
{
    /// <summary>
    /// 消息(只有Context可以接收JSON字符串)
    /// </summary>
    public class MessageHand
    {
        /// <summary>
        /// 创建此消息的类
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// 调用此消息的方法
        /// </summary>
        public readonly MethodBase MethodBase;

        /// <summary>
        /// 创建时间
        /// </summary>
        public readonly DateTime CreateTime;

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception ex;

        /// <summary>
        /// 初始化消息
        /// </summary>
        public MessageHand()
        {
            StackTrace trace = new StackTrace();
            Type = trace.GetFrame(1).GetMethod().DeclaringType;
            MethodBase = trace.GetFrame(1).GetMethod();
            WriteType = WriteType.TXT;
            CreateTime = DateTime.Now;

        }

        /// <summary>
        /// 消息代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 消息正文，支持JSON
        /// </summary>
        public string Context;

        /// <summary>
        /// 写入方式
        /// </summary>
        public WriteType WriteType;

        /// <summary>
        /// 调度记录日志需要的部分参数JSON
        /// </summary>
        public string LogInfoEntityJson
        {
            get
            {
                var exMsg = "";
                if (ex != null) exMsg = ex.Message;
                return "{\"WriteType\":" + (int)WriteType + ",\"Code\":" + Code + ",\"Context\":\"" + StringToJson(Context)
                    + "\",\"NameSpace\":\"" + Type.Namespace + "\",\"ClassName\":\"" + Type.Name + "\",\"MethodName\":\"" +
                    MethodBase.Name + "\",\"ExceptionMessage\":\"" + exMsg +
                    "\",\"CreateTime\":\"" + CreateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
            }
        }

        /// <summary>
        /// 转义String中JSON需要的特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private String StringToJson(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }


    }

    /// <summary>
    /// 消息记录方式
    /// </summary>
    public enum WriteType
    {
        /// <summary>
        /// 数据库和TXT
        /// </summary>
        All = 0,

        /// <summary>
        /// 数据库
        /// </summary>
        DB = 1,

        /// <summary>
        /// TXT
        /// </summary>
        TXT = 2
    }

    /// <summary>
    /// 消息代码
    /// </summary>
    public class CODE
    {
        public const int SUCCED = 200;
        public const int FAIED = 500;

    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Scheduler.Service.Utils
{
    public class JsonHelper
    {

        /// <summary>
        /// 对象转换为json字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ObjToJson(Object o)
        {

            return JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// json解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            string serialStr = JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);
            return serialStr;

        }



    }
}

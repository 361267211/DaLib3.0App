/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.JsonSerialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartLibrary.Navigation.Application.PageParam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Filter
{
    public class ColumnChangeAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public ColumnChangeAttribute()
        {

        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
          //  var columnInfo = BodyModelAsync<NavigationColumnParam>(context.HttpContext.Request);

            base.OnResultExecuting(context);
        }

        /// <summary>
        /// BodyModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Request"></param>
        /// <returns></returns>
        public T BodyModelAsync<T>(HttpRequest Request) where T : new()
        {
            T t = new T();

            try
            {
                Request.EnableBuffering();
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string body = reader.ReadToEnd();
                    Request.Body.Position = 0;//以后可以重复读取
                    t = JSON.Deserialize<T>(body);
                }
            }
            catch (Exception ex)
            {
                t = default;
            }
            return t;
        }
    }
}

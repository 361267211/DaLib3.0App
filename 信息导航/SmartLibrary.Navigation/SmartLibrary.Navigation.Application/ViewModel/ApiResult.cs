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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [XmlRoot("root")]
    [Serializable]
    public class ApiResult<T>
    {
        public ApiResult() { }

        public ApiResult(T t)
        {
            this.result = t;
        }

        public ApiResult(T t, int count = 0, int status = 100, string info = "OK")
        {
            this.result = t;
            this.count = count;
            this.status = status;
            this.info = info;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int status = 100;
        /// <summary>
        /// 信息
        /// </summary>
        public string info = "OK";
        /// <summary>
        /// 结果
        /// </summary>
        public T result { get; set; }
        /// <summary>
        /// 匹配结果总数
        /// </summary>
        public int count = 0;
    }
}

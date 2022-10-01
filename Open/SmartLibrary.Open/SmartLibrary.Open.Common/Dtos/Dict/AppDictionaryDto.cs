/*********************************************************
* 名    称：AppDictionaryDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210910
* 描    述：应用字典数据传输模型
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Common.Dtos
{
    /// <summary>
    /// 应用模块字典数据模型
    /// </summary>
    public class AppDictionaryDto
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public Guid ID { get; set; }
        ///// <summary>
        ///// 字典类型
        ///// </summary>
        //[MaxLength(20, ErrorMessage = "字典类型最多输入20个字符")]
        //public string DictType { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        [MaxLength(20, ErrorMessage = "字典名称最多输入20个字符")]
        public string Key { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        [MaxLength(50, ErrorMessage = "字典值最多输入50个字符")]
        public string Value { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [MaxLength(100, ErrorMessage = "描述信息最多输入100个字符")]
        public string Desc { get; set; }

    }
}

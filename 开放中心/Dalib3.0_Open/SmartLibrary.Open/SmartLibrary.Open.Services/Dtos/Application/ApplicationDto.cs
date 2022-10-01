/*********************************************************
* 名    称：ApplicationDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用传输模型
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用传输模型
    /// </summary>
    public class ApplicationDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        [MinLength(2, ErrorMessage = "应用名称请输入2-100个字符")]
        [MaxLength(100, ErrorMessage = "应用名称请输入2-100个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        [Required(ErrorMessage ="请选择应用类型")]
        public int AppType { get; set; }
        /// <summary>
        /// 服务类型集合
        /// </summary>
        [Required(ErrorMessage = "至少选择一种服务类型")]
        public List<string> ServiceTypes { get; set; }
        /// <summary>
        /// 服务包集合
        /// </summary>
        public List<string> ServicePacks { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        [Required(ErrorMessage = "请选择供应商")]
        public Guid DevID { get; set; }
        /// <summary>
        /// 适用终端
        /// </summary>
        public List<int> Terminal { get; set; }
        /// <summary>
        /// 应用场景
        /// </summary>
        public List<int> Scene { get; set; }
        /// <summary>
        /// 应用简介
        /// </summary>
        [MinLength(0, ErrorMessage = "应用简介请输入0-50个字符")]
        [MaxLength(50, ErrorMessage = "应用简介请输入0-50个字符")]
        public string Intro { get; set; }
        /// <summary>
        /// 应用介绍
        /// </summary>
        [MinLength(0, ErrorMessage = "应用简介请输入0-1000个字符")]
        [MaxLength(50, ErrorMessage = "应用简介请输入0-1000个字符")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "请选择或上传图标")]
        public string Icon { get; set; }

        /// <summary>
        /// 是否免费试用
        /// </summary>
        [Required(ErrorMessage = "请选择是否免费试用")]
        public bool FreeTry { get; set; }
        /// <summary>
        /// 建议售价
        /// </summary>
        [DataValidation(ValidationTypes.Money, ErrorMessage = "请输入金额")]
        public decimal? AdvisePrice { get; set; }
        /// <summary>
        /// 定价类型,0:一次付清，1：月付费，2：年付费
        /// </summary>
        [Range(0,2,ErrorMessage ="未找到对应定价类型")]
        public int PriceType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }



}

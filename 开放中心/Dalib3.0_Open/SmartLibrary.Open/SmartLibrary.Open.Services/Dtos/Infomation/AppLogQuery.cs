/*********************************************************
 * 名    称：AppLogQuery
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/14 21:27:20
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.Open.Common.AssemblyBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Infomation
{
    public class AppLogQuery : TableQueryBase
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 信息类型 1-应用动态 2-活动信息 3-使用教程
        /// </summary>
        public int InfoType { get; set; }
    }
}

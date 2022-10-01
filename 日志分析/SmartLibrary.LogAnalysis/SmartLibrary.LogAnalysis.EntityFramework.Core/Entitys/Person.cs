/*********************************************************
 * 名    称：Person
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：数据库模型Person示例。
 *
 * 更新历史：
 *
 * *******************************************************/


using Furion.DatabaseAccessor;
using SmartLibrary.LogAnalysis.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Core.Entitys
{
    /// <summary>
    /// 测试 EF、多租户、多租户迁移用的模型Person
    /// </summary>
    public class Person : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
    }
}

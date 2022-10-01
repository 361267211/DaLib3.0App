﻿using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：NewsContentExpend
    /// 作    者：张泽军
    /// 创建时间：2021/9/13 10:50:46
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentExpend: Entity<string>
    {
        //[Key, StringLength(64)]
        //public string Id { get; set; }

        [StringLength(64),Required]
        public string ColumnID { get; set; }

        [StringLength(64),Required]
        public string FiledName { get; set; }

        [StringLength(20),Required]
        public string Filed { get; set; }

        public bool DeleteFlag { get; set; }
    }
}

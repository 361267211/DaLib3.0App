using SmartLibrary.Navigation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Dto
{
    /// <summary>
    /// 名    称：ContentDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:10:32
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ContentDto
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        ///<summary>
        ///内容标题
        ///</summary>
        [StringLength(50), Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        [StringLength(200)]
        public string TitleStyle { get; set; }

        /// <summary>
        /// 内容扩展项键值对
        /// </summary>
        public List<KeyValuePair<string, object>> TitleStyleKV
        {
            get
            {
                if (string.IsNullOrEmpty(TitleStyle))
                    return null;
                List<KeyValuePair<string, object>> listKV = new List<KeyValuePair<string, object>>();
                foreach (var ext in TitleStyle.Split(';'))
                {

                    var value = Converter.ToType<bool>(ext.Split('-')[1]);
                    if ("B,I,U".Contains(ext.Split('-')[0]))
                        listKV.Add(new KeyValuePair<string, object>(ext.Split('-')[0], Converter.ToType<bool>(ext.Split('-')[1])));
                    else if ("font".Contains(ext.Split('-')[0]))
                        listKV.Add(new KeyValuePair<string, object>(ext.Split('-')[0], Converter.ToType<int>(ext.Split('-')[1])));
                    else
                        listKV.Add(new KeyValuePair<string, object>(ext.Split('-')[0], ext.Split('-')[1]));

                }
                return listKV;
            }
        }

        ///<summary>
        ///副标题
        ///</summary>
        [StringLength(200)]
        public string SubTitle { get; set; }

        ///<summary>
        ///所属目录
        ///</summary>
        [StringLength(64), Required]
        public string CatalogueID { get; set; }

        /// <summary>
        /// 所属目录名
        /// </summary>
        public string CatalogueName { get; set; }

        ///<summary>
        ///多目录投递
        ///</summary>
        public string RelationCatalogueIDs { get; set; }

        /// <summary>
        /// 多目录投递键值对
        /// </summary>
        public List<KeyValuePair<string, string>> RelationCatalogueIDsKV { get; set; }

        ///<summary>
        ///内容
        ///</summary>
        public string Contents { get; set; }

        ///<summary>
        ///链接
        ///</summary>
        public string LinkUrl { get; set; }

        ///<summary>
        ///发布人
        ///</summary>
        [StringLength(50,MinimumLength =2), Required]
        public string Publisher { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100), Required]
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }

        ///<summary>
        ///发布日期
        ///</summary>
        [Required]
        public DateTime PublishDate { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        [Required]
        public bool Status { get; set; }

        ///<summary>
        ///状态名
        ///</summary>
        [Required]
        public string StatusName { get { return Status ? "正常" : "下架"; } }

        /// <summary>
        /// 点击数
        /// </summary>
        public int HitCount { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Required]
        public int SortIndex { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 页面序号
        /// </summary>
        public int IndexNum { get; set; }
    }

    public class ContentVo: ContentDto
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public string  ColumnId { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string  ColumnName { get; set; }
    }


}

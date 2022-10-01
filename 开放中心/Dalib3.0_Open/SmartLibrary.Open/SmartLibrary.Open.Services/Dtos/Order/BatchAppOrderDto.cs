using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Order
{
    public class BatchAppOrderDto
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        [Required(ErrorMessage = "客户ID不能为空")]
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 应用集合
        /// </summary>
        [Required(ErrorMessage = "请至少选择一个应用")]
        public List<Guid> AppIdList { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 开通类型
        /// </summary>
        public int OpenType { get; set; }
        /// <summary>
        /// 开通途径
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required(ErrorMessage = "请填写开始日期")]
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 截至日期
        /// </summary>
        [Required(ErrorMessage = "请填写截止日期")]
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [MinLength(2, ErrorMessage = "申请人输入2-20个字符")]
        [MaxLength(20, ErrorMessage = "申请人输入2-20个字符")]
        public string ApplyMan { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500, ErrorMessage = "备注输入0-500个字符")]
        public string Remark { get; set; }
    }
}

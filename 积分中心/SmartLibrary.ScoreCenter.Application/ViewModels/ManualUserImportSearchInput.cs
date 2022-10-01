/*********************************************************
* 名    称：ManualUserImportSearchDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩查询
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分奖惩查询
    /// </summary>
    public class ManualUserImportSearchDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
    }
}

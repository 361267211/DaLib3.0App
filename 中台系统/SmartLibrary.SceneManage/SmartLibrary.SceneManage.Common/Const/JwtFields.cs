/*********************************************************
 * 名    称：JwtFields
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/2 10:28:30
 * 描    述：Jwt包含信息
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Common.Const
{
    /// <summary>
    /// Jwt包含信息
    /// </summary>
    public class JwtFields
    {
        /// <summary>
        /// 机构账号
        /// </summary>
        public const string OrgId= nameof(OrgId);
        /// <summary>
        /// 机构密码
        /// </summary>
        public const string OrgSecret = nameof(OrgSecret);
        /// <summary>
        /// 机构编号
        /// </summary>
        public const string OrgCode = nameof(OrgCode);
        /// <summary>
        /// 用户标识
        /// </summary>
        public const string UserKey = nameof(UserKey);
    }
}

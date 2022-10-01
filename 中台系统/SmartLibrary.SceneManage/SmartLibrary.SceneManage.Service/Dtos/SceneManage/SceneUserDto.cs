/*********************************************************
 * 名    称：SceneUserDto
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:49:42
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class SceneUserDto
    {
        /// <summary>
        /// 场景用户标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 场景标识
        /// </summary>
        public string SceneId { get; set; }

        /// <summary>
        /// 用户群体标识
        /// </summary>
        public string UserSetId { get; set; }


        /// <summary>
        /// 用户群体类型。1-学院，2-类型，3-分组
        /// </summary>
        public int UserSetType { get; set; }
    }
}

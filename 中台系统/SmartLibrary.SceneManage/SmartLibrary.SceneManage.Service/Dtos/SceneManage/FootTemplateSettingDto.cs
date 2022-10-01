/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
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
    public class FootTemplateSettingDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 头部模板的id
        /// </summary>
        public string FootTemplateId { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// JS文件的路径
        /// </summary>
        public List<string> JsPath { get; set; }
    }
}

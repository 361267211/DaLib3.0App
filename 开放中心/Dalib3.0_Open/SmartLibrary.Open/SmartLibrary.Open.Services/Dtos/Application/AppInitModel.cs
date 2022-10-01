/*********************************************************
* 名    称：AppInitModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用管理页面初始化模型
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common.Dtos;
using System.Collections.Generic;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用管理页面初始化模型
    /// </summary>
    public class AppInitModel
    {
        public AppInitModel()
        {
            ServiceTypeDict = new List<SysDictModel>();
            ServicePackDict = new List<SysDictModel>();
            StatusDict = new List<EnumDictModel>();
            TerminalDict = new List<EnumDictModel>();
            SceneDict = new List<EnumDictModel>();
        }
        /// <summary>
        /// 服务类型字典
        /// </summary>
        public List<SysDictModel> ServiceTypeDict { get; set; }
        /// <summary>
        /// 服务包字典
        /// </summary>
        public List<SysDictModel> ServicePackDict { get; set; }
        /// <summary>
        /// 服务状态字典
        /// </summary>
        public List<EnumDictModel> StatusDict { get; set; }
        /// <summary>
        /// 终端字典
        /// </summary>
        public List<EnumDictModel> TerminalDict { get; set; }
        /// <summary>
        /// 场景字典
        /// </summary>
        public List<EnumDictModel> SceneDict { get; set; }
    }
}

/*********************************************************
* 名    称：RegionOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：区域地址管理
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 区域地址管理
    /// </summary>
    public class RegionOutput
    {
        public int Id { get; set; }
        public string IdDisp
        {
            get
            {
                return Id.ToString();
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        public int PId { get; set; }
        //public string SName { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityCode { get; set; }
        //public string YzCode { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        public string MerName { get; set; }
        //public float Lng { get; set; }
        //public float Lat { get; set; }
        //public string PinYin { get; set; }
        //下级地址
        public List<RegionOutput> Children { get; set; }
    }
}

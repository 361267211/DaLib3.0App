/*********************************************************
* 名    称：AdaptEncoder.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：文本适配器
* 更新历史：
*
* *******************************************************/
using Mapster;
using System.Collections.Generic;

namespace SmartLibrary.User.Common.Utils
{
    /// <summary>
    /// 适配器，用于对文本脱敏,实际在Mapper处理
    /// </summary>
    public static class AdaptEncoder
    {
        /// <summary>
        /// 对象编码转换，需要自行配置Mapper规则
        /// </summary>
        /// <param name="oriQueryFilter"></param>
        /// <returns></returns>
        public static TObj EncodedFilter<TObj, TEncodeObj>(TObj obj) where TEncodeObj : class where TObj : class
        {
            var tempObject = obj.Adapt<TEncodeObj>();
            return tempObject.Adapt<TObj>();
        }

        /// <summary>
        /// 对象编码转换，需要自行配置Mapper规则
        /// </summary>
        /// <param name="oriQueryFilter"></param>
        /// <returns></returns>
        public static TObj SensitiveEncode<TObj, TEncodeObj>(TObj obj) where TEncodeObj : new() where TObj : new()
        {
            var tempObject = obj.Adapt<TEncodeObj>();
            return tempObject.Adapt<TObj>();
        }

        /// <summary>
        /// 对象编码转换，需要自行配置Mapper规则
        /// </summary>
        /// <param name="oriQueryFilter"></param>
        /// <returns></returns>
        public static PagedList<TObj> SensitiveEncode<TObj, TEncodeObj>(PagedList<TObj> objPagedList) where TEncodeObj : new() where TObj : new()
        {
            var tempObject = objPagedList.Adapt<PagedList<TEncodeObj>>();
            return tempObject.Adapt<PagedList<TObj>>();
        }
    }
}

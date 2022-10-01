/*********************************************************
* 名    称：SysDictViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：字典视图模型
* 更新历史：
*
* *******************************************************/
namespace SmartLibrary.Open.Common.Dtos
{
    /// <summary>
    /// 字典模型
    /// </summary>
    public class SysDictModel
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 枚举类型模型
    /// </summary>
    public class EnumDictModel
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }
}

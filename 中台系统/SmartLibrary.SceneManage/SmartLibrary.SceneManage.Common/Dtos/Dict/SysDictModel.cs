/*********************************************************
* 名    称：SysDictViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：字典视图模型
* 更新历史：
*
* *******************************************************/
namespace SmartLibrary.SceneManage.Common.Dtos
{
    /// <summary>
    /// 字典模型
    /// </summary>
    public class SysDictModel<T>
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
    }
}

/*********************************************************
* 名    称：UserCreateInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者创建
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者创建
    /// </summary>
    public class UserCreateInput
    {
        public UserCreateInput()
        {
            UserData = new UserInput();
            CardData = new CardInput();
        }
        /// <summary>
        /// 读者信息
        /// </summary>
        public UserInput UserData { get; set; }
        /// <summary>
        /// 卡信息
        /// </summary>
        public CardInput CardData { get; set; }
    }
}

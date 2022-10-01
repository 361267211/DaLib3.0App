/*********************************************************
* 名    称：DictItem.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：泛型字典对象
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos
{
    public class DictItem<T1,T2> 
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }
    }
}

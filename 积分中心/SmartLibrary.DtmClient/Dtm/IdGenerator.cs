/*********************************************************
* 名    称：IdGenerator.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：分支id生成器
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// 分支id生成器
    /// </summary>
    public class IdGenerator
    {
        private string parentId;
        private int branchId;

        public IdGenerator(string parentId = "")
        {
            this.parentId = parentId;
            this.branchId = 0;
        }
        /// <summary>
        /// 生成分支
        /// </summary>
        /// <returns></returns>
        public string NewBranchId()
        {
            if (this.branchId >= 99)
            {
                throw new ArgumentException("branch id is larger than 99");
            }
            if (this.parentId.Length > 20)
            {
                throw new ArgumentException("total branch id is longer than 20");
            }
            this.branchId = this.branchId + 1;

            return this.parentId + this.branchId.ToString().PadLeft(2, '0');
        }

    }
}

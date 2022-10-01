/*********************************************************
 * 名    称：NewsDataAdapter
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/7 18:23:36
 * 描    述：新闻数据适配器
 *
 * 更新历史：
 *
 * *******************************************************/
using SqlSugar;
using System.Collections.Generic;
using TaskManager.Model.Entities;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    public interface INavDataAdapter
    {
        List<ContentBack> GetNavContentList(List<string> columnIds,out MessageHand message);

        /// <summary>
        /// 取新闻对应的栏目数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<NavigationColumnBack> GetNavColumnList(out MessageHand message);
        /// <summary>
        /// 组织需要获取的权限项目
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<SysMenuPermission> GetSysPermissionList(List<NavigationColumn> columns, SqlSugarClient client, out MessageHand message);

        List<NavigationCatalogueBack> GetNavCatalogueList(List<string> columnIds, out MessageHand message);
    }
}
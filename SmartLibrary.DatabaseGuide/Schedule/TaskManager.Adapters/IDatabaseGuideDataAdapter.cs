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
using System;
using System.Collections.Generic;
using TaskManager.Model.Entities;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    public interface IDatabaseGuideDataAdapter
    {
        List<DatabaseTerraceBack> GetDatabaseList(DateTime lastSuccessTime, out MessageHand message);


    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Util
{
    public static class ExtendModel
    {
        public static List<TResult> DtToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表
            var prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            var t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach(t.GetProperties(), p => { if (dt.Columns.Contains(p.Name)) prlist.Add(p); });
            //创建返回的集合
            var oblist = new List<TResult>();
            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                var ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p =>
                {

                    if (row[p.Name] != DBNull.Value)
                    {
                        if (p.PropertyType == typeof(Int32) || p.PropertyType == typeof(Int64))
                        {
                            p.SetValue(ob, Convert.ToInt32(row[p.Name]), null);
                        }
                        else
                        {
                            p.SetValue(ob, row[p.Name], null);
                        }

                    }

                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        public static List<TResult> DtStringAllToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表
            var prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            var t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 

            Array.ForEach(t.GetProperties(), p => { if (dt.Columns.Contains(p.Name)) prlist.Add(p); });

            //创建返回的集合
            var oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                var ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p =>
                {

                    //if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name].ToString(), null);
                    if (row[p.Name] != DBNull.Value)
                    {
                        p.SetValue(ob, row[p.Name].ToString(), null);
                    }
                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }
    }
}

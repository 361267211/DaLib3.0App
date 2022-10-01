using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application
{
    /// <summary>
    /// 合并表达式 And Or Not扩展
    /// </summary>
    public static class ExpressionExtend
    {
        /// <summary>合并表达式 expLeft and expRight</summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            //用于将参数名进行替换，二者参数不一样
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            //需要先将参数替换为一致的，可能参数名不一样
            var left = visitor.Replace(expLeft.Body);//左侧的表达式
            var right = visitor.Replace(expRight.Body);//右侧的表达式
            var body = Expression.And(left, right);//合并表达式
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
        /// <summary>合并表达式 expr1 or expr2</summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            //需要先将参数替换为一致的，可能参数名不一样
            var left = visitor.Replace(expr1.Body);
            var right = visitor.Replace(expr2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            var candidateExpr = expr.Parameters[0];
            var body = Expression.Not(expr.Body);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>参数替换者 </summary>
        class NewExpressionVisitor : ExpressionVisitor
        {
            public ParameterExpression _NewParameter { get; private set; }
            public NewExpressionVisitor(ParameterExpression param)
            {
                this._NewParameter = param;//用于把参数替换了
            }
            /// <summary> 替换</summary>
            public Expression Replace(Expression exp)
            {
                return this.Visit(exp);
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                //返回新的参数名
                return this._NewParameter;
            }
        }
    }
}

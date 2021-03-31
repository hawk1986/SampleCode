using System;
using System.Collections;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities.Extensions
{
    public static class QueryableExt
    {
        /// <summary>
        /// 根據 propertyName 欄位，對 IQueryable 物件進行排序，由 asc 決定遞增或遞減。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable 物件</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="asc">排序 true 遞增/ false 遞減</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string asc)
        {
            var type = typeof(T);
            string methodName = asc == "asc" ? "OrderBy" : "OrderByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// 根據 propertyName 欄位，對 IQueryable 物件進行排序，由 asc 決定遞增或遞減，必須先使用 OrderBy 才能使用 ThenBy。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable 物件</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="asc">排序 true 遞增/ false 遞減</param>
        /// <returns></returns>
        public static IQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName, string asc)
        {
            var type = typeof(T);
            string methodName = asc == "asc" ? "ThenBy" : "ThenByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// 依 keyword 對 IQueryable 物件的 propertyName 欄位進行搜尋。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable 物件</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="keyword">關鍵字</param>
        /// <returns></returns>
        public static IQueryable<T> Like<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var constant = Expression.Constant("%" + keyword + "%");
            MethodCallExpression methodExp = Expression.Call(null, typeof(SqlMethods).GetMethod("Like", new Type[] { typeof(string), typeof(string) }), propertyAccess, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);
            return source.Where(lambda);
        }

        /// <summary>
        /// 將 IEnumerable 轉換至 DataTable
        /// </summary>
        /// <param name="list">IEnumerable</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(this IEnumerable list)
        {
            var dt = new DataTable();
            bool schemaIsBuild = false;
            PropertyInfo[] props = null;

            foreach (object item in list)
            {
                if (!schemaIsBuild)
                {
                    props = item.GetType().GetProperties();
                    foreach (var pi in props)
                    {
                        dt.Columns.Add(new DataColumn(pi.Name, pi.PropertyType));
                    }

                    schemaIsBuild = true;
                }

                var row = dt.NewRow();
                foreach (var pi in props)
                {
                    row[pi.Name] = pi.GetValue(item, null);
                }

                dt.Rows.Add(row);
            }

            dt.AcceptChanges();

            return dt;
        }
    }
}

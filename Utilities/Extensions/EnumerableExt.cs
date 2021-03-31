using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Utilities.Interface;
using Utilities.Model;

namespace Utilities.Extensions
{
    public static class EnumerableExt
    {
        /// <summary>
        /// 轉型為 Dictionary
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyGetter"></param>
        /// <param name="valueGetter"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> ToDictionaryEx<TElement, TKey, TValue>(
            this IEnumerable<TElement> source,
            Func<TElement, TKey> keyGetter,
            Func<TElement, TValue> valueGetter)
        {
            IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (var e in source)
            {
                var key = keyGetter(e);
                if (dict.ContainsKey(key))
                    continue;

                dict.Add(key, valueGetter(e));
            }
            return dict;
        }

        /// <summary>
        /// 轉型為 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (source.Any())
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(source.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 轉型為 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="hasSequence"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, bool hasSequence, params ColumnInfo[] columns)
        {
            if (columns == null || columns.Length == 0)
                return null;

            var props = typeof(T).GetProperties();
            var result = new DataTable();
            //加入自定義的欄位
            List<PropertyInfo> map = new List<PropertyInfo>();
            //判斷是否需要序號
            if (hasSequence)
                result.Columns.Add("序號", typeof(int));

            foreach (var column in columns)
            {
                if (!string.IsNullOrEmpty(column.FieldName))
                {
                    var prop = props.FirstOrDefault(x => x.Name == column.FieldName);
                    map.Add(prop);
                    //加入欄位
                    result.Columns.Add(column.ColumnName, prop?.PropertyType ?? typeof(string));
                }
                else
                {
                    map.Add(null);
                    //加入預設值欄位
                    result.Columns.Add(column.ColumnName, column.DefaultValue == null ? typeof(string) : column.DefaultValue.GetType());
                }
            }

            if (source.Any())
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    //判斷是否需要序號
                    if (hasSequence)
                        tempList.Add(i + 1);

                    for (int j = 0; j < map.Count; j++)
                    {
                        if (map[j] == null)
                            tempList.Add(columns[j].DefaultValue);
                        else
                        {
                            object obj = map[j].GetValue(source.ElementAt(i), null);
                            tempList.Add(obj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// SelectRecursive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursive(children, selector))
                    yield return child;
            }
        }

        /// <summary>
        /// SelectRecursiveChildren
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectRecursiveChildren<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector) where T : IChildren<T>
        {
            foreach (var parent in source)
            {
                parent.Children = selector(parent).ToList();
                if (parent.Children.Count > 0)
                    SelectRecursiveChildren(parent.Children, selector);
            }
            return source;
        }
    }
}
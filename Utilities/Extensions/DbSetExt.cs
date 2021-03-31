using System.Collections.Generic;
using System.Data.Entity;

namespace Utilities.Extensions
{
    public static class DbSetExt
    {
        /// <summary>
        /// 批次新增
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dbSet">this</param>
        /// <param name="objects">multiple rows</param>
        public static void AddObjects<T>(this DbSet<T> dbSet, IEnumerable<T> objects) where T : class
        {
            foreach (var item in objects)
            {
                dbSet.Add(item);
            }
        }
    }
}
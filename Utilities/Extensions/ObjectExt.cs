using System;
using System.Reflection;

namespace Utilities.Extensions
{
    public static class ObjectExt
    {
        /// <summary>
        /// 如果物件本身為 null 回傳 String.Empty 非 null 回傳自己.ToString()
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string NullToStringEmpty(this object me)
        {
            return (me == null) ? string.Empty : me.ToString();
        }

        /// <summary>
        /// 將Class Member 前後空白移除
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T TrimClass<T>(T Value) where T : class, new()
        {
            //建立一個回傳用的 CdrcusInfo
            var Result = new T();

            //宣告一個 PropertyInfo 陣列，來接取 Type 所有的共用屬性
            PropertyInfo[] PI_List = Value.GetType().GetProperties();

            foreach (PropertyInfo Item in PI_List)
            {
                //取得屬性的Value
                object value = Item.GetValue(Value, null);
                if (value != null)
                {
                    //取得屬性的Type
                    Type myType = Type.GetType(Item.PropertyType.FullName);

                    //如果是字串就Trim()
                    if (myType == typeof(string))
                    {
                        value = value.ToString().Trim();
                        //將結果存入Result
                        Item.SetValue(Result, value, null);
                    }
                }

            }

            return Result;
        }
    }
}

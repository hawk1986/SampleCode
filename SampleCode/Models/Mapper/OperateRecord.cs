#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generated at 06/23/2020 15:43:06
//       Runtime Version: 4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using SampleCode.ViewModel;

namespace SampleCode.Models
{
    public partial class OperateRecord
    {
        /// <summary>
        /// 將 ViewModel 轉換為 Model 物件
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static OperateRecord Convert(OperateRecordViewModel source)
        {
            return source == null ? null : new OperateRecord
            {
                SequenceNo = source.SequenceNo,
                ID = source.ID,
                RecordID = source.RecordID,
                TableName = source.TableName ?? string.Empty,
                RecordInfo = source.RecordInfo ?? string.Empty,
                Action = source.Action ?? string.Empty,
                Result = source.Result ?? string.Empty,
                OperateIP = source.OperateIP ?? string.Empty,
                OperateUser = source.OperateUser ?? string.Empty,
                OperateTime = source.OperateTime,
            };
        }

        /// <summary>
        /// 將 Model 轉換為 ViewModel 物件
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static OperateRecordViewModel Convert(OperateRecord source)
        {
            return source == null ? null : new OperateRecordViewModel
            {
                SequenceNo = source.SequenceNo,
                ID = source.ID,
                RecordID = source.RecordID,
                TableName = source.TableName ?? string.Empty,
                RecordInfo = source.RecordInfo ?? string.Empty,
                Action = source.Action ?? string.Empty,
                Result = source.Result ?? string.Empty,
                OperateIP = source.OperateIP ?? string.Empty,
                OperateUser = source.OperateUser ?? string.Empty,
                OperateTime = source.OperateTime,
            };
        }

        /// <summary>
        /// 轉換運算子
        /// </summary>
        /// <param name="source"></param>
        public static explicit operator OperateRecord(OperateRecordViewModel source)
        {
            return Convert(source);
        }

        /// <summary>
        /// 轉換運算子
        /// </summary>
        /// <param name="source"></param>
        public static explicit operator OperateRecordViewModel(OperateRecord source)
        {
            return Convert(source);
        }
    }
}
#pragma warning restore 1591
using System;
using System.Runtime.Serialization;

namespace Utilities.ApnsNotifications
{
    [Serializable]
    public class NotificationException : Exception
    {
        protected NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotificationException()
            : base()
        {
        }

        public NotificationException(int code, string message)
            : base(message)
        {
            this.Code = code;
        }

        public int Code
        {
            get;
            set;
        }
    }
}

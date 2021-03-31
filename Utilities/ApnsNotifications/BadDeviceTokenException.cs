using System;
using System.Runtime.Serialization;

namespace Utilities.ApnsNotifications
{
    [Serializable]
    public class BadDeviceTokenException : Exception
    {
        protected BadDeviceTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BadDeviceTokenException(string deviceToken)
            : base(string.Format("Device Token Length ({0}) Is not the required length of {1} characters!", deviceToken.Length, Notification.DEVICE_TOKEN_STRING_SIZE))
        {
            this.DeviceToken = deviceToken;
        }

        public string DeviceToken
        {
            get;
            private set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Utilities.ApnsNotifications
{
    [Serializable]
    public class NotificationBatchException : Exception
    {
        protected NotificationBatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal NotificationBatchException(List<NotificationDeliveryError> errors)
            : base(String.Format("There were delivery problems with {0} notifications in batch.  See the DeliveryErrors property for details.", errors.Count))
        {
            DeliveryErrors = errors.AsReadOnly();
        }

        public IList<NotificationDeliveryError> DeliveryErrors { get; private set; }
    }
}

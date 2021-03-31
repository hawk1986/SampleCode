using System.Collections.Generic;

namespace Utilities
{
    public class PushMessage
    {
        public List<string> registration_ids = new List<string>();
        public MessageDetail data = new MessageDetail();
    }

    public class MessageDetail
    {
        public string message = string.Empty;
    }
}

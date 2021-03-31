using Newtonsoft.Json;

namespace Utilities
{
    public class JsonMessage
    {
        public string Style { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

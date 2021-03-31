using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SampleCode.ViewModel.Struct
{
    [Serializable]
    public class TemplateFields : Dictionary<string, string>
    {
        protected TemplateFields(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TemplateFields() { }

        public TemplateFields(Dictionary<string, string> dictionary) : base(dictionary) { }

        public TemplateFields(List<string> keyList)
        {
            Clear();
            if (null != keyList && keyList.Count > 0)
            {
                foreach (var item in keyList)
                    if (!ContainsKey(item))
                        Add(item, string.Empty);
            }
        }

        public TemplateFields(string dictionaryString)
        {
            Clear();
            if (!string.IsNullOrEmpty(dictionaryString))
            {
                Dictionary<string, string> freeFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictionaryString);
                foreach (var item in freeFields)
                    if (!ContainsKey(item.Key))
                        Add(item.Key, item.Value);
            }
        }

        public override string ToString()
        {
            return Count > 0 ? JsonConvert.SerializeObject(this) : "{}";
        }
    }
}
using System.Collections.Generic;

namespace SampleCode.ViewModel
{
    public class Paging<T> where T : class
    {
        public int total { get; set; }

        public List<T> rows { get; set; }
    }
}

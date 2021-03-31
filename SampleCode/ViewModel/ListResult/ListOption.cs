namespace SampleCode.ViewModel.ListResult
{
    public class ListOption
    {
        public string field { get; set; }

        public string title { get; set; }

        public bool checkbox { get; set; }

        public bool sortable { get; set; }

        public bool visible { get; set; }

        public string formatter { get; set; }

        public int? width { get; set; }

        public ListOption()
        {
            sortable = true;
            visible = true;
            formatter = string.Empty;
        }
    }
}
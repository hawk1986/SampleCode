
using ResourceLibrary;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SampleCode.ViewModel.SearchModel
{
    public class SearchModel
    {
        [Display(Name = "Keyword", ResourceType = typeof(Resource))]
        public string Search { get; set; }

        public string Sort { get; set; }

        public string Order { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

		public Dictionary<string, SelectList> Formatter { get; set; }

        public SearchModel()
        {
            Search = string.Empty;
            Sort = "SequenceNo";
            Order = "desc";
            Limit = 10;
            Offset = 0;
        }
    }
}

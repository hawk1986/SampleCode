using System.Collections.Generic;

namespace SampleCode.ViewModel
{
    public class FunctionSequenceInfo
    {
        public byte GroupSequence { get; set; }

        public byte Sequence { get; set; }

        public int BitCode { get; set; }

        public List<FunctionViewModel> DependencyFunctionList { get; set; }

        public FunctionSequenceInfo()
        {
            GroupSequence = 1;
            Sequence = 1;
            BitCode = 1;
            DependencyFunctionList = new List<FunctionViewModel>();
        }
    }
}
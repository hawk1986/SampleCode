using System;

namespace Utilities
{
    public class Auth
    {
        public Guid FunctionID { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Code { get; set; }

        public string ModuleName { get; set; }

        public string MenuIcon { get; set; }

        public string GroupName { get; set; }

        public string SimpleName { get; set; }

        public string DisplayName { get; set; }

        public bool DisplayTree { get; set; }

        public byte ModuleSequence { get; set; }

        public byte Sequence { get; set; }

        public byte GroupSequence { get; set; }

        public int BitCode { get; set; }

        public int Dependency { get; set; }

        public string ID { get { return string.Concat(ActionName.StartsWith("Index") ? "Index" : ActionName, "_", ControllerName); } }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleCode.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisualMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VisualMenu()
        {
            this.Children = new HashSet<VisualMenu>();
        }
    
        public int SequenceNo { get; set; }
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
        public byte Sequence { get; set; }
        public byte MenuLevel { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string Layout { get; set; }
        public string IconPath { get; set; }
        public string HeaderPath { get; set; }
        public int ViewCount { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Parameter { get; set; }
        public string ExternalUrl { get; set; }
        public bool IsInlinePage { get; set; }
        public bool IsShowFooter { get; set; }
        public bool IsEnable { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public System.DateTime UpdateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisualMenu> Children { get; set; }
        public virtual VisualMenu Parent { get; set; }
    }
}

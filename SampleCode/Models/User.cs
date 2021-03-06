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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Department = new HashSet<Department>();
            this.UserChangePass = new HashSet<UserChangePass>();
            this.UserProfile = new HashSet<UserProfile>();
            this.Role = new HashSet<Role>();
        }
    
        public int SequenceNo { get; set; }
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ContactTel { get; set; }
        public bool IsEnable { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public System.DateTime LoginTime { get; set; }
        public System.DateTime ChangePassTime { get; set; }
        public string PhotoPath { get; set; }
        public bool IsToken { get; set; }
        public string HashToken { get; set; }
        public string TokenData { get; set; }
        public string FreeFields { get; set; }
        public Nullable<System.Guid> DefaultIndex { get; set; }
        public Nullable<System.Guid> DepartmentID { get; set; }
        public string DepartmentIDs { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department { get; set; }
        public virtual Function Function { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserChangePass> UserChangePass { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> UserProfile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Role { get; set; }
    }
}

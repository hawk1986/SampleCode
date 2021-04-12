using ResourceLibrary;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class ExportCSVModel
    {
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Planned Start Date")]
        public string PlannedStartDate { get; set; }
        [Display(Name = "Planned Completion Date")]
        public string PlannedCompletionDate { get; set; }
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }
        [Display(Name = "Project Owner Federation ID")]
        public string ProjectOwnerFederationID { get; set; }
        [Display(Name = "Project Owner Email Address")]
        public string ProjectOwnerEmailAddress { get; set; }
        [Display(Name = "Portfolio Name")]
        public string PortfolioName { get; set; }
        [Display(Name = "Portfolio Owner Federation ID")]
        public string PortfolioOwnerFederationID { get; set; }
        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }
        [Display(Name = "Program Owner Federation ID")]
        public string ProgramOwnerFederationID { get; set; }
        [Display(Name = "Currency")]
        public string Currency { get; set; }
        [Display(Name = "Budget")]
        public string Budget { get; set; }
        [Display(Name = "Planned Benefit")]
        public string PlannedBenefit { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Line of Business")]
        public string LineofBusiness { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Brand")]
        public string Brand { get; set; }
        [Display(Name = "Market")]
        public string Market { get; set; }
        [Display(Name = "Office")]
        public string Office { get; set; }
        [Display(Name = "Chargeable vs. Non Chargeable")]
        public string ChargeablevsNonChargeable { get; set; }
        [Display(Name = "Project Type")]
        public string ProjectType { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Accounting Project ID")]
        public string AccountingProjectID { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntity { get; set; }
        [Display(Name = "Finance Source System")]
        public string FinanceSourceSystem { get; set; }
        [Display(Name = "CRM URN ID")]
        public string CRMURNID { get; set; }
        [Display(Name = "CRM Source System")]
        public string CRMSourceSystem { get; set; }
        [Display(Name = "Other Source System")]
        public string OtherSourceSystem { get; set; }
        [Display(Name = "Additional Project ID")]
        public string AdditionalProjectID { get; set; }
        [Display(Name = "Record Status")]
        public string RecordStatus { get; set; }
    }
}
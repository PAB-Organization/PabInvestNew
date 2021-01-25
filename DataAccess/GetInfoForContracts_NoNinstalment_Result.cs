//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    
    public partial class GetInfoForContracts_NoNinstalment_Result
    {
        public string CaseNumber { get; set; }
        public string RepaymentNoninstalment { get; set; }
        public Nullable<decimal> CreditLimitAmount_AmountValue { get; set; }
        public string CreditLimitAmount_Currency { get; set; }
        public Nullable<decimal> UsedAmount_AmountValue { get; set; }
        public string UsedAmount_Currency { get; set; }
        public string CreditUsedInMonth { get; set; }
        public Nullable<decimal> CurrentOutstandingPrincipalAmount_AmountValue { get; set; }
        public string CurrentOutstandingPrincipalAmount_Currency { get; set; }
        public int MaximumNumberofDelinquencyDays { get; set; }
        public int TotalNumberofDelinquencyDays { get; set; }
        public int TotalNumberofDelinquencyDaysInLastYear { get; set; }
        public Nullable<decimal> OverdueAmount_AmountValue { get; set; }
        public string OverdueAmount_Currency { get; set; }
        public Nullable<decimal> CurrentUndisbursedLoanAmount_AmountValue { get; set; }
        public string CurrentUndisbursedLoanAmount_Currency { get; set; }
        public string LoanType { get; set; }
        public string CreditPurpose { get; set; }
        public string CreditCurrency { get; set; }
        public string PhaseofOperation { get; set; }
        public string InformationAboutOverdueContract { get; set; }
        public string LoanPartiallySecuredByDeposit { get; set; }
        public Nullable<System.DateTime> Dates_Start { get; set; }
        public Nullable<System.DateTime> Dates_ExpectedEnd { get; set; }
        public Nullable<System.DateTime> Dates_RealEnd { get; set; }
        public Nullable<System.DateTime> Dates_Overdue { get; set; }
        public Nullable<System.DateTime> Dates_Repayment { get; set; }
        public Nullable<int> Dates_CanceledDemand { get; set; }
        public Nullable<System.DateTime> Dates_ExpofLimitation { get; set; }
        public string ContractTerm { get; set; }
        public Nullable<decimal> Penalty_PenaltySum_AmountValue { get; set; }
        public string Penalty_PenaltySum_Currency { get; set; }
        public Nullable<decimal> Penalty_PenaltyPaid_AmountValue { get; set; }
        public string Penalty_PenaltyPaid_Currency { get; set; }
        public string Subjects_key__0S__IndividualRelation_IdentifierIndividual { get; set; }
        public string Subjects_key__0S__IndividualRelation_RoleOfClient { get; set; }
        public string Subjects_key__0S__CompanyRelation_IdentifierCompany { get; set; }
        public string Subjects_key__0S__CompanyRelation_RoleOfClient { get; set; }
        public Nullable<decimal> MaxInstallmentAmount { get; set; }
        public string MaxInstallmentCurrency { get; set; }
        public int activeid { get; set; }
    }
}

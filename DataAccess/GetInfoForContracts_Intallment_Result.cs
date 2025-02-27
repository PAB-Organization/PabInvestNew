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
    
    public partial class GetInfoForContracts_Intallment_Result
    {
        public string CaseNumber { get; set; }
        public string RepaymentInstalment { get; set; }
        public string InstalmentType { get; set; }
        public string PeriodicityOfPayments { get; set; }
        public Nullable<decimal> TotalCreditAmount_AmountValue { get; set; }
        public string TotalCreditAmount_Currency { get; set; }
        public Nullable<int> StandardInstallmentCount { get; set; }
        public Nullable<decimal> StandardInstallmentAmount { get; set; }
        public Nullable<int> OverdueInstallmentCount { get; set; }
        public Nullable<decimal> OverdueAmount_AmountValue { get; set; }
        public string OverdueAmount_Currency { get; set; }
        public Nullable<decimal> OutstandingInstallmentCount { get; set; }
        public Nullable<decimal> OutstandingAmount_AmountValue { get; set; }
        public string OutstandingAmount_Currency { get; set; }
        public Nullable<decimal> CurrentOverduePrincipalAmount_AmountValue { get; set; }
        public string CurrentOverduePrincipalAmount_Currency { get; set; }
        public Nullable<decimal> CurrentOverdueInteresetandCommissionAmount_AmountValue { get; set; }
        public string CurrentOverdueInteresetandCommissionAmount_Currency { get; set; }
        public Nullable<decimal> CurrentOutstandingPrincipalAmount_AmountValue { get; set; }
        public string CurrentOutstandingPrincipalAmount_Currency { get; set; }
        public Nullable<decimal> CurrentOutstandingInteresetandCommissionAmount_AmountValue { get; set; }
        public string CurrentOutstandingInteresetandCommissionAmount_Currency { get; set; }
        public int MaximumNumberofDelinquencyDays { get; set; }
        public int TotalNumberofDelinquencyDays { get; set; }
        public int TotalNumberofDelinquencyDaysInLastYear { get; set; }
        public Nullable<int> TotalNumberofOverdueInstallments { get; set; }
        public Nullable<decimal> CurrentUndisbursedLoanAmount_AmountValue { get; set; }
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
        public string Subjects_IndividualRelation_IdentifierIndividual { get; set; }
        public string Subjects_IndividualRelation_RoleOfClient { get; set; }
        public string Subjects_CompanyRelation_IdentifierCompany { get; set; }
        public string Subjects_CompanyRelation_RoleOfClient { get; set; }
        public Nullable<decimal> MaxInstallmentAmount { get; set; }
        public string MaxInstallmentCurrency { get; set; }
        public int activeid { get; set; }
    }
}

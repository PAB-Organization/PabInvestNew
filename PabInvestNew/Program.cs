using DataAccess;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PabInvestNew
{
   
        class Program
        {
            static void Main(string[] args)
            {
                try
                {

                Response("429804253");

                //UploadNonInstalment();

                //  Upload();

                //UploadCreditCard();
                //SendMail("PubInvest: კრედიტინფოს სერვისი წარმატებით დასრულდა!");
                //using (var db = new CreditinfoServiceEntities())
                //{
                //    DateTime date = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                //    date = Convert.ToDateTime(date.ToString("yyyy-MM-dd"));
                //    var query = from st in db.CreditInfoBatchIds
                //                 .Where(s => s.IsML == 2 && s.Date == date)
                //                select st;
                //    foreach (var batchid in query)
                //    {
                //        Response(batchid.BatchId.ToString());
                //    }

                //}
            }
            catch (Exception ex)
                {

                }
                //UploadPersonalData();
            }

           
            public static void SendMail(string Body)
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("CreditInfoResult@gmail.com", "pabgaga123");
                MailMessage msgobj = new MailMessage();
                msgobj.To.Add("nozadzegiorgipab@gmail.com ");
                msgobj.CC.Add("shanavalevani@gmail.com");
                msgobj.From = new MailAddress("CreditInfoResult@gmail.com");
                msgobj.Subject = "CreditInfo Result";
                msgobj.Body = Body.ToString();
                client.Send(msgobj);
            }
            private static void Response(string ResponseID)
            {
                try
                {
                    get.creditinfosolutions.Service auth = new get.creditinfosolutions.Service();
                    auth.Credentials = new System.Net.NetworkCredential("pabinvest.up", "1tcbYzVm");
                    //XmlDocument doc = new XmlDocument();
                    var node = auth.BatchResponse(ResponseID);
                    //doc.AppendChild(node);
                    //doc.Save(@"C:\Users\pab\Desktop\Analitics\_Analytics\Creditinfo\ML\Real\creditinfosync\Batch Response\" + Guid.NewGuid().ToString() + ".xml");

                    // File.WriteAllText(@"C:\Users\pab\Desktop\Analitics\_Analytics\Creditinfo\ML\Real\creditinfosync\Batch Response\Response.txt", node.InnerXml.ToString());

                    string readText = node.InnerXml.ToString();
                    XmlDocument doc = new XmlDocument();
                    readText = readText.Replace("xmlns=\"http://cb4.creditinfosolutions.com/BatchUploader/Batch\"", "");

                    readText = "<root>" + readText + "</root>";
                    doc.LoadXml(readText);
                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/Commands/Command");
                    string _ErrorText = "", _Identifier = "";
                    int ErrorStep;
                    foreach (XmlNode node1 in nodes)
                    {
                        ErrorStep = 0;
                        XmlDocument doc1 = new XmlDocument();

                        doc1.LoadXml(node1.InnerXml);
                        XmlNodeList nodes1 = doc1.DocumentElement.SelectNodes("/Exception/Parameters/Parameter");

                        foreach (XmlNode node2 in nodes1)
                        {
                            ErrorStep++;
                            _Identifier = node1.Attributes["identifier"].Value.ToString();
                            _ErrorText = node2.SelectSingleNode("Value") == null ? "" : node2.SelectSingleNode("Value").InnerText;
                            using (var db = new CreditinfoServiceEntities())
                            {
                                if (_ErrorText != "")
                                {
                                    var customers = db.Set<CreditInfoResult>();
                                    customers.Add(new CreditInfoResult { Identifier = _Identifier, Result = _ErrorText, Date = DateTime.Now, IsML = 2, ErrorTypeId = ErrorStep });

                                    db.SaveChanges();
                                }
                            }
                        }



                        // _ErrorText = node1.SelectSingleNode("Exception/Parameters/Parameter/Value") == null ? "" : node1.SelectSingleNode("Exception/ErrorCode").InnerText;



                    }

                }
                catch (Exception ex)
                {
                    SendMail("PubInvest_Response: კრედიტინფოს სერვისის გაშვებისას დაფიქსირდა ხარვეზი,მიზეზი: " + System.Environment.NewLine + ex.Message.ToString());
                }
            }





            private static void UploadCreditCard()
            {
                try
                {
                    using (var db = new CreditinfoServiceEntities())
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var data = db.GetInfoForContracts_CreditCardPI().ToList();
                            if (data.Count <= 0)
                                break;
                            using (var client = new CreditinfoService.ServiceSoapClient())
                            {
                            var common = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_outer.xml");
                            var template = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_CreditCard.xml");

                            var guid = Guid.NewGuid().ToString();

                            var fileName = @"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\GeneritedContract_CreditCard\" + guid + ".xml";
                            float id = 1;
                                foreach (var item in data)
                                {
                                    var _clients = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType == "IndividualRelation")
                        .ToList<Client>();

                                    var _clientsCompany = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType != "IndividualRelation")
                        .ToList<Client>();


                                    string Content = "";
                                    string SubjectWithContent = "";

                                    // For Company

                                    string ContentCompany = "";
                                    string SubjectWithContentCompany = "";
                                    string SubjectResult = "";
                                    Content = Content + $"<Subjects key=\"01\">" + "\n" + "<IndividualRelation>\n" + $"<IdentifierIndividual>{item.Subjects_key__0S__IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n".Replace(" ", "");
                                    Content = Content + $"<RoleOfClient>{item.Subjects_key__0S__IndividualRelation_RoleOfClient}</RoleOfClient>\n";
                                    // Content=Content+ item.Subjects_IndividualRelation_RoleOfClient== "RoleOfClient.Guarantor"? $"<GuaranteeAmount>\n<AmountValue>{item.amount}"
                                    Content = Content + "</IndividualRelation>\n</Subjects>";
                                    SubjectResult = Content;
                                    int step = 1;
                                    foreach (var cl in _clients)
                                    {
                                        step++;

                                        Content = Content + $"\n<Subjects key=\"0{step}\">" + "\n" + "<IndividualRelation>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierIndividual>{cl.PIN.Trim()}</IdentifierIndividual>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";
                                    }
                                    if (_clients.Count > 0)
                                    {
                                        SubjectWithContent = Content;
                                    }
                                    foreach (var cl in _clientsCompany)
                                    {

                                        //ContentCompany = ContentCompany + "\n" + $"<IdentifierCompany> {cl.PIN.Trim()} </IdentifierCompany>" + "\n" + $"<RoleOfClient> {cl.RoleOfClient.Trim()} </RoleOfClient>";
                                        step++;
                                        Content = Content + $"<Subjects key=\"0{step}\">" + "\n" + "<IndividualCompany>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierCompany>{cl.PIN.Trim()}</IdentifierCompany>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";

                                    }
                                    if (_clientsCompany.Count > 0)
                                    {
                                        SubjectWithContentCompany = ContentCompany;
                                    }
                                    if (_clients.Count > 0 || _clientsCompany.Count > 0)
                                    {
                                        SubjectResult = SubjectWithContent + SubjectWithContentCompany;
                                    }


                                    var realend = item.Dates_RealEnd;

                                    var content = template;

                                    content = content.Replace("_Dates_RealEnd_", item.Dates_RealEnd.HasValue ? item.Dates_RealEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_CaseNumber_", item.CaseNumber);
                                    content = content.Replace("_RepaymentCreditCard_", item.RepaymentCreditCard);
                                    content = content.Replace("_CommandIdentifier_", "\"" + item.CaseNumber.ToString() + "\"");
                                    content = content.Replace("_PeriodicityOfPayments_", item.PeriodicityOfPayments);
                                    content = content.Replace("_TotalCreditAmount_AmountValue_", item.TotalCreditAmount_AmountValue.ToString());
                                    content = content.Replace("_TotalCreditAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_StandardInstallmentAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_OverdueAmount_AmountValue_", item.OverdueAmount_AmountValue.ToString());
                                    content = content.Replace("_OverdueAmount_Currency_", item.OverdueAmount_Currency);
                                    content = content.Replace("_OutstandingAmount_AmountValue_", item.OutstandingAmount_AmountValue.ToString());
                                    content = content.Replace("_OutstandingAmount_Currency_", item.OutstandingAmount_Currency);
                                    content = content.Replace("_LoanType_", item.LoanType);
                                    content = content.Replace("_CreditPurpose_", item.CreditPurpose);
                                    content = content.Replace("_CreditCurrency_", item.CreditCurrency);
                                    content = content.Replace("_Dates_Start_", item.Dates_Start.HasValue ? item.Dates_Start.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_ExpectedEnd_", item.Dates_ExpectedEnd.HasValue ? item.Dates_ExpectedEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Overdue_", item.Dates_Overdue.HasValue ? item.Dates_Overdue.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Repayment_", item.Dates_Repayment.HasValue ? item.Dates_Repayment.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_ExpofLimitation_", item.Dates_ExpofLimitation.HasValue ? item.Dates_ExpofLimitation.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Penalty_PenaltySum_AmountValue_", item.Penalty_PenaltySum_AmountValue.ToString());
                                    content = content.Replace("_Penalty_PenaltySum_Currency_", item.Penalty_PenaltySum_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_Currency_", item.Penalty_PenaltyPaid_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_AmountValue_", item.Penalty_PenaltyPaid_AmountValue.ToString());
                                    content = content.Replace("_PhaseofOperation_", item.PhaseofOperation);
                                //content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", item.Subjects_key__0S__IndividualRelation_IdentifierIndividual);
                               // content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", item.Subjects_key__0S__IndividualRelation_RoleOfClient);
                                content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", null);
                                content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", null);
                                    content = content.Replace("_getdate_", DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH:mm:ss"));
                                    content = content.Replace("_CurrentOverduePrincipalAmount_AmountValue_", item.CurrentOverduePrincipalAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOverduePrincipalAmount_Currency_", item.CurrentOverduePrincipalAmount_Currency);
                                    content = content.Replace("_CurrentOverdueInteresetandCommissionAmount_AmountValue_", item.CurrentOverdueInteresetandCommissionAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOverdueInteresetandCommissionAmount_Currency_", item.CurrentOverdueInteresetandCommissionAmount_Currency);
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_AmountValue_", item.CurrentOutstandingPrincipalAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_Currency_", item.CurrentOutstandingPrincipalAmount_Currency);
                                    content = content.Replace("_MaximumNumberofDelinquencyDays_", item.MaximumNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDays_", item.TotalNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDaysInLastYear_", item.TotalNumberofDelinquencyDaysInLastYear.ToString());
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_AmountValue_", item.CurrentUndisbursedLoanAmount_AmountValue.ToString());
                                    content = content.Replace("_LoanPartiallySecuredByDeposit_", item.LoanPartiallySecuredByDeposit);
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_AmountValue_", item.CurrentUndisbursedLoanAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_ContractTerm_", item.ContractTerm);
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_AmountValue_", item.MaxInstallmentAmount.ToString());
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_Currency_", item.MaxInstallmentCurrency);
                                    content = content.Replace("<IdentifierIndividual>_Subjects_IndividualRelation_IdentifierIndividual1_</IdentifierIndividual>", SubjectResult);

                                    content = string.Format("{0}\r\n_contract_\r\n", content);
                                    common = common.Replace("_contract_", content);
                                    id++;
                                }

                                common = common.Replace("_contract_", "");
                                common = common.Replace("_identifier_", CalculateMD5Hash(guid));
                                common = RemoveEmptyNodes(common);
                                System.IO.File.WriteAllText(fileName, common);


                            // es agzavnis
                            UploadFile(fileName);
                        }
                        }

                        //BatchResponse();
                    }
                }
                catch (Exception ex)
                {
                    SendMail("PubInvest_UploadCreditCard: კრედიტინფოს სერვისის გაშვებისას დაფიქსირდა ხარვეზი,მიზეზი: " + System.Environment.NewLine + ex.Message.ToString());
                }
            }












            private static void Upload()
            {
                try
                {
                    using (var db = new CreditinfoServiceEntities())
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var data = db.GetInfoForContracts_IntallmentPI().ToList();


                            if (data.Count <= 0)
                                break;
                            using (var client = new CreditinfoService.ServiceSoapClient())
                            {
                                var common = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_outer.xml");
                                var template = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract.xml");

                                var guid = Guid.NewGuid().ToString();

                                var fileName = @"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\Generated_CreditCard\" + guid + ".xml";
                                float id = 1;
                                foreach (var item in data)
                                {


                                    var _clients = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType == "IndividualRelation")
                        .ToList<Client>();

                                    var _clientsCompany = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType != "IndividualRelation")
                        .ToList<Client>();


                                    string Content = "";
                                    string SubjectWithContent = "";

                                    // For Company

                                    string ContentCompany = "";
                                    string SubjectWithContentCompany = "";
                                    string SubjectResult = "";
                                    Content = Content + $"<Subjects key=\"01\">" + "\n" + "<IndividualRelation>\n" + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                    Content = Content + $"<RoleOfClient>{item.Subjects_IndividualRelation_RoleOfClient}</RoleOfClient>\n";
                                    // Content=Content+ item.Subjects_IndividualRelation_RoleOfClient== "RoleOfClient.Guarantor"? $"<GuaranteeAmount>\n<AmountValue>{item.amount}"
                                    Content = Content + "</IndividualRelation>\n</Subjects>";
                                    SubjectResult = Content;
                                    int step = 1;
                                    foreach (var cl in _clients)
                                    {
                                        step++;

                                        Content = Content + $"\n<Subjects key=\"0{step}\">" + "\n" + "<IndividualRelation>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierIndividual>{cl.PIN.Trim()}</IdentifierIndividual>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";
                                    }
                                    if (_clients.Count > 0)
                                    {
                                        SubjectWithContent = Content;
                                    }
                                    foreach (var cl in _clientsCompany)
                                    {

                                        //ContentCompany = ContentCompany + "\n" + $"<IdentifierCompany> {cl.PIN.Trim()} </IdentifierCompany>" + "\n" + $"<RoleOfClient> {cl.RoleOfClient.Trim()} </RoleOfClient>";
                                        step++;
                                        Content = Content + $"<Subjects key=\"0{step}\">" + "\n" + "<IndividualCompany>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierCompany>{cl.PIN.Trim()}</IdentifierCompany>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";

                                    }
                                    if (_clientsCompany.Count > 0)
                                    {
                                        SubjectWithContentCompany = ContentCompany;
                                    }
                                    if (_clients.Count > 0 || _clientsCompany.Count > 0)
                                    {
                                        SubjectResult = SubjectWithContent + SubjectWithContentCompany;
                                    }


                                    var realend = item.Dates_RealEnd;

                                    var content = template;

                                    content = content.Replace("_Dates_RealEnd_", item.Dates_RealEnd.HasValue ? item.Dates_RealEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_CaseNumber_", item.CaseNumber);
                                    content = content.Replace("_RepaymentInstalment_", item.RepaymentInstalment);
                                    content = content.Replace("_CommandIdentifier_", "\"" + item.CaseNumber.ToString() + "\"");
                                    content = content.Replace("_InstalmentType_", item.InstalmentType);
                                    content = content.Replace("_PeriodicityOfPayments_", item.PeriodicityOfPayments);
                                    content = content.Replace("_TotalCreditAmount_AmountValue_", item.TotalCreditAmount_AmountValue.ToString());
                                    content = content.Replace("_TotalCreditAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_StandardInstallmentCount_", item.StandardInstallmentCount.ToString());
                                    content = content.Replace("_StandardInstallmentAmount_AmountValue_", item.StandardInstallmentAmount.ToString());
                                    content = content.Replace("_StandardInstallmentAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_OverdueInstallmentCount_", item.OverdueInstallmentCount.ToString());
                                    content = content.Replace("_OverdueAmount_AmountValue_", item.OverdueAmount_AmountValue.ToString());
                                    content = content.Replace("_OverdueAmount_Currency_", item.OverdueAmount_Currency);
                                    content = content.Replace("_OutstandingInstallmentCount_", item.OutstandingInstallmentCount.ToString());
                                    content = content.Replace("_OutstandingAmount_AmountValue_", item.OutstandingAmount_AmountValue.ToString());
                                    content = content.Replace("_OutstandingAmount_Currency_", item.OutstandingAmount_Currency);
                                    content = content.Replace("_LoanType_", item.LoanType);
                                    content = content.Replace("_CreditPurpose_", item.CreditPurpose);
                                    content = content.Replace("_CreditCurrency_", item.CreditCurrency);
                                    content = content.Replace("_Dates_Start_", item.Dates_Start.HasValue ? item.Dates_Start.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_ExpectedEnd_", item.Dates_ExpectedEnd.HasValue ? item.Dates_ExpectedEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Overdue_", item.Dates_Overdue.HasValue ? item.Dates_Overdue.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Repayment_", item.Dates_Repayment.HasValue ? item.Dates_Repayment.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Penalty_PenaltySum_AmountValue_", item.Penalty_PenaltySum_AmountValue.ToString());
                                    content = content.Replace("_Penalty_PenaltySum_Currency_", item.Penalty_PenaltySum_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_Currency_", item.Penalty_PenaltyPaid_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_AmountValue_", item.Penalty_PenaltyPaid_AmountValue.ToString());
                                    content = content.Replace("_PhaseofOperation_", item.PhaseofOperation);
                                //content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", item.Subjects_IndividualRelation_IdentifierIndividual);
                               // content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", item.Subjects_IndividualRelation_RoleOfClient);
                                content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", null);
                                content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", null);
                                    content = content.Replace("_getdate_", DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH:mm:ss"));
                                    content = content.Replace("_CurrentOverduePrincipalAmount_AmountValue_", item.CurrentOverduePrincipalAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOverduePrincipalAmount_Currency_", item.CurrentOverduePrincipalAmount_Currency);
                                    content = content.Replace("_CurrentOverdueInteresetandCommissionAmount_AmountValue_", item.CurrentOverdueInteresetandCommissionAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOverdueInteresetandCommissionAmount_Currency_", item.CurrentOverdueInteresetandCommissionAmount_Currency);
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_AmountValue_", item.CurrentOutstandingPrincipalAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_Currency_", item.CurrentOutstandingPrincipalAmount_Currency);
                                    content = content.Replace("_MaximumNumberofDelinquencyDays_", item.MaximumNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDays_", item.TotalNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDaysInLastYear_", item.TotalNumberofDelinquencyDaysInLastYear.ToString());
                                    content = content.Replace("_TotalNumberofOverdueInstallments_", item.TotalNumberofOverdueInstallments.ToString());
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_AmountValue_", item.CurrentUndisbursedLoanAmount_AmountValue.ToString());
                                    content = content.Replace("_LoanPartiallySecuredByDeposit_", item.LoanPartiallySecuredByDeposit);
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_AmountValue_", item.CurrentUndisbursedLoanAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_Currency_", item.TotalCreditAmount_Currency);
                                    content = content.Replace("_ContractTerm_", item.ContractTerm);
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_AmountValue_", item.MaxInstallmentAmount.ToString());
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_Currency_", item.MaxInstallmentCurrency);
                                    content = content.Replace("<IdentifierIndividual>_Subjects_IndividualRelation_IdentifierIndividual1_</IdentifierIndividual>", SubjectResult);


                                    content = string.Format("{0}\r\n_contract_\r\n", content);
                                    common = common.Replace("_contract_", content);
                                    id++;
                                }

                                common = common.Replace("_contract_", "");
                                common = common.Replace("_identifier_", CalculateMD5Hash(guid));
                                common = RemoveEmptyNodes(common);
                                System.IO.File.WriteAllText(fileName, common);


                                // es agzavnis
                                UploadFile(fileName);
                            }


                            //BatchResponse();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendMail("PubInvest_Upload კრედიტინფოს სერვისის გაშვებისას დაფიქსირდა ხარვეზი,მიზეზი: " + System.Environment.NewLine + ex.Message.ToString());
                }
            }
            private static void UploadNonInstalment()
            {
                try
                {
                    using (var db = new CreditinfoServiceEntities())
                    {
                        for (int i = 0; i < 20; i++)
                        {


                            var data = db.GetInfoForContracts_NoNinstalmentPI().ToList();
                            if (data.Count() <= 0)
                                break;
                            using (var client = new CreditinfoService.ServiceSoapClient())
                            {
                                var common = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_outer.xml");
                                var template = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_Noninstalment.xml");

                                var guid = Guid.NewGuid().ToString();

                                var fileName = @"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\Generated_NonInstalment\" + guid + ".xml";
                                float id = 1;
                                foreach (var item in data)
                                {

                                    var _clients = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType == "IndividualRelation")
                        .ToList<Client>();

                                    var _clientsCompany = db.Clients
                        .Where(s => s.ActiveID == item.activeid && s.IndividualType != "IndividualRelation")
                        .ToList<Client>();


                                    string Content = "";
                                    string SubjectWithContent = "";

                                    // For Company

                                    string ContentCompany = "";
                                    string SubjectWithContentCompany = "";
                                    string SubjectResult = "";
                                    Content = Content + $"<Subjects key=\"01\">" + "\n" + "<IndividualRelation>\n" + $"<IdentifierIndividual>{item.Subjects_key__0S__IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                    Content = Content + $"<RoleOfClient>{item.Subjects_key__0S__IndividualRelation_RoleOfClient}</RoleOfClient>\n";
                                    // Content=Content+ item.Subjects_IndividualRelation_RoleOfClient== "RoleOfClient.Guarantor"? $"<GuaranteeAmount>\n<AmountValue>{item.amount}"
                                    Content = Content + "</IndividualRelation>\n</Subjects>";
                                    SubjectResult = Content;
                                    int step = 1;
                                    foreach (var cl in _clients)
                                    {
                                        step++;

                                        Content = Content + $"\n<Subjects key=\"0{step}\">" + "\n" + "<IndividualRelation>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierIndividual>{cl.PIN.Trim()}</IdentifierIndividual>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";
                                    }
                                    if (_clients.Count > 0)
                                    {
                                        SubjectWithContent = Content;
                                    }
                                    foreach (var cl in _clientsCompany)
                                    {

                                        //ContentCompany = ContentCompany + "\n" + $"<IdentifierCompany> {cl.PIN.Trim()} </IdentifierCompany>" + "\n" + $"<RoleOfClient> {cl.RoleOfClient.Trim()} </RoleOfClient>";
                                        step++;
                                        Content = Content + $"<Subjects key=\"0{step}\">" + "\n" + "<IndividualCompany>";// + $"<IdentifierIndividual>{item.Subjects_IndividualRelation_IdentifierIndividual}</IdentifierIndividual>\n";
                                        Content = Content + "\n" + $"<IdentifierCompany>{cl.PIN.Trim()}</IdentifierCompany>" + "\n" + $"<RoleOfClient>{cl.RoleOfClient.Trim()}</RoleOfClient>";
                                        //Content = Content + cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor" ? $"\n <GuaranteeAmount>\n<AmountValue>\n{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>" : "";
                                        if (cl.RoleOfClient.Trim() == "RoleOfClient.Guarantor")
                                        {
                                            Content = Content + $"\n <GuaranteeAmount>\n<AmountValue>{cl.GauarnteeAmount}</AmountValue>\n<Currency>{cl.Currency}</Currency>\n</GuaranteeAmount>";
                                        }
                                        Content = Content + "\n</IndividualRelation>\n</Subjects>\n";

                                    }
                                    if (_clientsCompany.Count > 0)
                                    {
                                        SubjectWithContentCompany = ContentCompany;
                                    }
                                    if (_clients.Count > 0 || _clientsCompany.Count > 0)
                                    {
                                        SubjectResult = SubjectWithContent + SubjectWithContentCompany;
                                    }


                                    var realend = item.Dates_RealEnd;

                                    var content = template;


                                    content = content.Replace("_Dates_RealEnd_", item.Dates_RealEnd.HasValue ? item.Dates_RealEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_CaseNumber_", item.CaseNumber);
                                    content = content.Replace("_RepaymentNoninstalment_", item.RepaymentNoninstalment);
                                    content = content.Replace("_CommandIdentifier_", "\"" + item.CaseNumber.ToString() + "\"");
                                    content = content.Replace("_CreditLimitAmount_AmountValue_", item.CreditLimitAmount_AmountValue.ToString());
                                    content = content.Replace("_CreditLimitAmount_Currency_", item.CreditLimitAmount_Currency);
                                    content = content.Replace("_UsedAmount_AmountValue_", item.UsedAmount_AmountValue.ToString());
                                    content = content.Replace("_UsedAmount_Currency_", item.UsedAmount_Currency);
                                    content = content.Replace("_CreditUsedInMonth_", item.CreditUsedInMonth);
                                    content = content.Replace("_OverdueAmount_AmountValue_", item.OverdueAmount_AmountValue.ToString());
                                    content = content.Replace("_OverdueAmount_Currency_", item.OverdueAmount_Currency);
                                    content = content.Replace("_LoanType_", item.LoanType);
                                    content = content.Replace("_CreditPurpose_", item.CreditPurpose);
                                    content = content.Replace("_CreditCurrency_", item.CreditCurrency);
                                    content = content.Replace("_Dates_Start_", item.Dates_Start.HasValue ? item.Dates_Start.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_ExpectedEnd_", item.Dates_ExpectedEnd.HasValue ? item.Dates_ExpectedEnd.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Overdue_", item.Dates_Overdue.HasValue ? item.Dates_Overdue.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_Repayment_", item.Dates_Repayment.HasValue ? item.Dates_Repayment.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Dates_ExpofLimitation_", item.Dates_ExpofLimitation.HasValue ? item.Dates_ExpofLimitation.Value.ToString("yyyy-MM-ddT00:00:00") : "");
                                    content = content.Replace("_Penalty_PenaltySum_AmountValue_", item.Penalty_PenaltySum_AmountValue.ToString());
                                    content = content.Replace("_Penalty_PenaltySum_Currency_", item.Penalty_PenaltySum_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_Currency_", item.Penalty_PenaltyPaid_Currency);
                                    content = content.Replace("_Penalty_PenaltyPaid_AmountValue_", item.Penalty_PenaltyPaid_AmountValue.ToString());
                                    content = content.Replace("_PhaseofOperation_", item.PhaseofOperation);
                                    //content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", item.Subjects_key__0S__IndividualRelation_IdentifierIndividual);
                                    //content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", item.Subjects_key__0S__IndividualRelation_RoleOfClient);
                                    content = content.Replace("_Subjects_IndividualRelation_IdentifierIndividual_", null);
                                    content = content.Replace("_Subjects_IndividualRelation_RoleOfClient_", null);
                                    //content = content.Replace("_getdate_", DateTime.Now.ToString("2019-08-07T00:00:00")); ;
                                    content = content.Replace("_getdate_", DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH:mm:ss"));
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_AmountValue_", item.CurrentOutstandingPrincipalAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentOutstandingPrincipalAmount_Currency_", item.CurrentOutstandingPrincipalAmount_Currency);
                                    content = content.Replace("_MaximumNumberofDelinquencyDays_", item.MaximumNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDays_", item.TotalNumberofDelinquencyDays.ToString());
                                    content = content.Replace("_TotalNumberofDelinquencyDaysInLastYear_", item.TotalNumberofDelinquencyDaysInLastYear.ToString());
                                    content = content.Replace("_LoanPartiallySecuredByDeposit_", item.LoanPartiallySecuredByDeposit);
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_AmountValue_", item.CurrentUndisbursedLoanAmount_AmountValue.ToString());
                                    content = content.Replace("_CurrentUndisbursedLoanAmount_Currency_", item.CurrentUndisbursedLoanAmount_Currency);
                                    content = content.Replace("_ContractTerm_", item.ContractTerm);
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_AmountValue_", item.MaxInstallmentAmount.ToString());
                                    content = content.Replace("_MaximumAmountOfInstalmentBySchedule_Currency_", item.MaxInstallmentCurrency);
                                    content = content.Replace("<IdentifierIndividual>_Subjects_IndividualRelation_IdentifierIndividual1_</IdentifierIndividual>", SubjectResult);


                                    content = string.Format("{0}\r\n_contract_\r\n", content);
                                    common = common.Replace("_contract_", content);
                                    id++;
                                }

                                common = common.Replace("_contract_", "");
                                common = common.Replace("_identifier_", CalculateMD5Hash(guid));
                                common = RemoveEmptyNodes(common);
                                System.IO.File.WriteAllText(fileName, common);


                                // es agzavnis
                              //  UploadFile(fileName);

                            }
                        }

                        //BatchResponse();
                    }
                }
                catch (Exception ex)
                {
                    SendMail("PubInvest_UploadNonInstalment  კრედიტინფოს სერვისის გაშვებისას დაფიქსირდა ხარვეზი,მიზეზი: " + System.Environment.NewLine + ex.Message.ToString());
                }
            }



            private static void UploadPersonalData()
            {
                try
                {
                    using (var db = new CreditinfoServiceEntities())
                    {
                        var data = db.GetPersonaData_intividualPI().ToList();
                        using (var client = new CreditinfoService.ServiceSoapClient())
                        {
                            var common = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\contract_outer.xml");
                            var template = System.IO.File.ReadAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\Individual.xml");

                            var guid = Guid.NewGuid().ToString();

                            var fileName = @"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\PabInvest\Generated_PersonalData\" + guid + ".xml";
                            float id = 1;
                            foreach (var item in data)
                            {

                                var content = template;

                                content = content.Replace("_CommandIdentifier_", "\"" + id.ToString() + "\"");
                                content = content.Replace("_PersonalData_NationalID_", item.PersonalData_NationalID);
                                content = content.Replace("_PersonalData_Firstname_", item.PersonalData_Firstname);
                                content = content.Replace("_PersonalData_Surname_", item.PersonalData_Surname);
                                content = content.Replace("_PersonalData_Gender_", item.PersonalData_Gender);
                                content = content.Replace("_PersonalData_GeorgianCitizen_", item.PersonalData_GeorgianCitizen);
                                content = content.Replace("_PersonalData_NationalID_", item.PersonalData_NationalID);
                                content = content.Replace("_PersonalData_BorrowerClassification_", item.PersonalData_BorrowerClassification);
                                content = content.Replace("_BirthData_BirthDate_", item.BirthData_BirthDate);
                                content = content.Replace("_[AddressesIndividual_PermanentResidence_AddressBaseChoice_FreeText_AddressValue]_", item.AddressesIndividual_PermanentResidence_AddressBaseChoice_FreeText_AddressValue);
                                content = content.Replace("_idenfitications_IDGeorgianCitizen_IDNumber_", item.idenfitications_IDGeorgianCitizen_IDNumber);


                                //content = content.Replace("_Dates_Start_", item.Dates_Start.HasValue ? item.Dates_Start.Value.ToString("yyyy-MM-ddT00:00:00") : "");

                                content = content.Replace("_getdate_", DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH:mm:ss"));


                                content = string.Format("{0}\r\n_contract_\r\n", content);
                                common = common.Replace("_contract_", content);
                                id++;
                            }

                            common = common.Replace("_contract_", "");
                            common = common.Replace("_identifier_", CalculateMD5Hash(guid));
                            common = RemoveEmptyNodes(common);
                            System.IO.File.WriteAllText(fileName, common);


                            // es agzavnis

                            UploadFile(fileName);

                        }


                        //BatchResponse();
                    }
                }
                catch (Exception ex)
                {
                    SendMail("PubInvest_UploadPersonalData  კრედიტინფოს სერვისის გაშვებისას დაფიქსირდა ხარვეზი,მიზეზი: " + System.Environment.NewLine + ex.Message.ToString());
                }
            }



            public static string RemoveEmptyNodes(string s)
            {
                XElement xml = XElement.Parse(s);
                xml.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();

                return xml.ToString();
            }

            public static string CalculateMD5Hash(string input)
            {
                // step 1, calculate MD5 hash from input
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }

            public static void UploadFile(string file, string cn = "")
            {
                long BUCBegin = 0;
                Zip(file, file + ".zip", 96);

                /*
                using (var client = new CreditinfoService.ServiceSoapClient())
                {
                    client.BatchUploadChunkBegin();
                    client.ClientCredentials.UserName.UserName = "microlend.up";
                    client.ClientCredentials.UserName.Password= "dC75yR77yI41";
                    client.BatchUploadChunk(BUCBegin, 1, GetBytesFromFile(file + ".zip"));
                    var finish = client.BatchUploadChunkFinish(CreditinfoService.BatchPriorityClassEnum.BatchProcessing, BUCBegin);
                } 
                */

                get.creditinfosolutions.Service auth = new get.creditinfosolutions.Service();
                auth.Credentials = new System.Net.NetworkCredential("pabinvest.up", "1tcbYzVm");
                auth.PreAuthenticate = true;
                BUCBegin = auth.BatchUploadChunkBegin();
                auth.BatchUploadChunk(BUCBegin, 1, GetBytesFromFile(file + ".zip"));
                var BUCFinish = auth.BatchUploadChunkFinish(get.creditinfosolutions.BatchPriorityClassEnum.BatchProcessing, BUCBegin);

                long fin = Convert2Long(BUCFinish.SelectNodes("/*[1]/*[1]").Item(0).InnerText);
                System.IO.File.AppendAllText(@"C:\Users\L.Shanava\Desktop\PAB\CreditInfo\Shablons\result.txt", string.Format("{0}\t{1}\t{2}\r\n", file.Replace(".xml", ""), Convert.ToString(fin), cn));

                using (var db = new CreditinfoServiceEntities())
                {
                    if (fin != 0)
                    {
                        var Batch = db.Set<CreditInfoBatchId>();
                        Batch.Add(new CreditInfoBatchId { BatchId = fin, Date = DateTime.Now, IsML = 2 });

                        db.SaveChanges();
                    }
                }

            }

            public static void BatchResponse()
            {
                get.creditinfosolutions.Service auth = new get.creditinfosolutions.Service();
                auth.Credentials = new System.Net.NetworkCredential("pabinvest.up", "1tcbYzVm");
                auth.PreAuthenticate = true;

                //XmlDocument doc = new XmlDocument();
                //XmlNode nod = auth.BatchResponse("425326");
                //string s = nod.OuterXml;
                //System.IO.File.WriteAllText(@"C:\Users\pab\Desktop\Analitics\_Analytics\Creditinfo\ML\Real\creditinfosync\Batch Response\" + Guid.NewGuid().ToString() + ".xml",s);

                XmlDocument doc = new XmlDocument();
                //XmlNode node = auth.BatchResponse("425326");
                //doc.ImportNode(node,true);
                //doc.Save(@"C:\Users\pab\Desktop\Analitics\_Analytics\Creditinfo\ML\Real\creditinfosync\Batch Response\" + Guid.NewGuid().ToString());
            }

            public static void Zip(string SrcFile, string DstFile, int BufferSize)
            {

                FileStream fileStreamIn = new FileStream
                    (SrcFile, FileMode.Open, FileAccess.Read);
                FileStream fileStreamOut = new FileStream
                    (DstFile, FileMode.Create, FileAccess.Write);
                ZipOutputStream zipOutStream = new ZipOutputStream(fileStreamOut);
                byte[] buffer = new byte[BufferSize];
                ZipEntry entry = new ZipEntry(Path.GetFileName(SrcFile));
                zipOutStream.PutNextEntry(entry);
                int size;
                do
                {
                    size = fileStreamIn.Read(buffer, 0, buffer.Length);
                    zipOutStream.Write(buffer, 0, size);
                } while (size > 0);
                zipOutStream.Close();
                fileStreamOut.Close();
                fileStreamIn.Close();
            }
            public static void UnZip(string SrcFile, string DstFile, int BufferSize)
            {
                FileStream fileStreamIn = new FileStream
                    (SrcFile, FileMode.Open, FileAccess.Read);
                ZipInputStream zipInStream = new ZipInputStream(fileStreamIn);
                ZipEntry entry = zipInStream.GetNextEntry();

                FileStream fileStreamOut = new FileStream
                    (DstFile + entry.Name, FileMode.Create, FileAccess.Write);
                int size;
                byte[] buffer = new byte[BufferSize];
                do
                {
                    size = zipInStream.Read(buffer, 0, buffer.Length);
                    fileStreamOut.Write(buffer, 0, size);
                } while (size > 0);
                zipInStream.Close();
                fileStreamOut.Close();
                fileStreamIn.Close();
            }
            public static byte[] GetBytesFromFile(string fullFilePath)
            {
                // this method is limited to 2^32 byte files (4.2 GB)
                FileStream fs = System.IO.File.OpenRead(fullFilePath);
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    return bytes;
                }
                finally
                {
                    fs.Close();
                }
            }
            public static long Convert2Long(String Str1)
            {
                try
                {
                    long LngString = Int64.Parse(Str1);
                    return LngString;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Parameter is not in required format");
                    return -1;
                }
            }
        }
    }


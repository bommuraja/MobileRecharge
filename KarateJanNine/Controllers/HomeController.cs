
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KarateJanNine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {

                DatasForWalletBalance objDatasForWalletBalance = new DatasForWalletBalance();
                objDatasForWalletBalance.id = "95317608";
                objDatasForWalletBalance.token = "ZbfFt6JWuRsdhySrCG80eoVI4UpcYNQP9AlwqXKz";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                string URI = "https://allbills.in/api/wallet-balance";

                var json = new JavaScriptSerializer().Serialize(objDatasForWalletBalance);
                using (WebClient wc = new WebClient())
                {

                    string HtmlResult = wc.UploadString(URI, json);
                    dynamic data = JObject.Parse(HtmlResult);
                    ViewData["Balance"] = JObject.Parse(HtmlResult)["Wallet"];

                    //return Json(data.wallet, JsonRequestBehavior.AllowGet);

                }

                //DatasForRecharge objDatasForWalletBalance = new DatasForRecharge();
                //objDatasForWalletBalance.id = "95317608";
                //objDatasForWalletBalance.token = "ZbfFt6JWuRsdhySrCG80eoVI4UpcYNQP9AlwqXKz";
                //objDatasForWalletBalance.RechargeTo = "9345617197";
                //objDatasForWalletBalance.ProviderId = "1";
                //objDatasForWalletBalance.Amount = "10";

                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                //string URI = "https://allbills.in/api/recharge";

                //var json = new JavaScriptSerializer().Serialize(objDatasForWalletBalance);
                //using (WebClient wc = new WebClient())
                //{

                //    string HtmlResult = wc.UploadString(URI, json);
                //    //dynamic data = JObject.Parse(HtmlResult);
                //    //ViewData["Balance"] = JObject.Parse(HtmlResult)["Wallet"];

                //    //return Json(data.wallet, JsonRequestBehavior.AllowGet);

                //}



                return View();
            }
            catch (Exception ex)
            {

                return View();

            }

        }

        public class WalletBalance
        {
            public string id { get; set; }
            public string token { get; set; }
        }

        public class DatasForRecharge
        {
            public int CustomerID { get; set; }
            public string MobileNumber { get; set; }
            public string NetworkName { get; set; }
            public string NetworkID { get; set; }
            public string RechargeAmount { get; set; }
            public string DateAndTime { get; set; }
            public string RechargeStatus { get; set; }
            public string RechargeStatusDescription { get; set; }
            public string RechargeReferenceIDFromAPI { get; set; }
            public string RechargeReferenceIDFromNetwork { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LastModifiedDate { get; set; }
            public string LastModifiedBy { get; set; }


        }
        public class DatasForLogin
        {
            public string userName { get; set; }
            public string password { get; set; }


        }

        public class ReachApiRecharge
        {
            public string format { get; set; }
            public string token { get; set; }
            public string mobile { get; set; }
            public string amount { get; set; }
            public string opid { get; set; }
            public string urid { get; set; }
        }

        public ActionResult GetCustomerWalletBalance(string customerID)
        {
            var CustomerID = Convert.ToInt32(customerID);
            RechargeEntities objRechargeEntities = new RechargeEntities();
            var WalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;
            return Json(WalletBalanceAmount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RechargeFunction(DatasForRecharge objDatasForRecharge)
        {
            RechargeEntities objRechargeEntities = new RechargeEntities();
            LoginResponse objLoginResponse = new LoginResponse();
            int NetworkID = Convert.ToInt32(objDatasForRecharge.NetworkID);
          
            try
            {
                // 1. Recharge Table Entry
                Recharge newRecharge = new Recharge();
                newRecharge.CustomerID = objDatasForRecharge.CustomerID;
                newRecharge.MobileNumber = objDatasForRecharge.MobileNumber;
                newRecharge.NetworkName = objDatasForRecharge.NetworkName;
                newRecharge.RechargeAmount = objDatasForRecharge.RechargeAmount;
                newRecharge.DateAndTime = objDatasForRecharge.DateAndTime;
                newRecharge.RechargeStatus = objDatasForRecharge.RechargeStatus;
                newRecharge.RechargeStatusDescription = objDatasForRecharge.RechargeStatusDescription;
                newRecharge.RechargeReferenceIDFromAPI = objDatasForRecharge.RechargeReferenceIDFromAPI;
                newRecharge.RechargeReferenceIDFromNetwork = objDatasForRecharge.RechargeReferenceIDFromNetwork;
                newRecharge.CreatedDate = objDatasForRecharge.CreatedDate;
                newRecharge.CreatedBy = objDatasForRecharge.CreatedBy;
                newRecharge.LastModifiedDate = objDatasForRecharge.LastModifiedDate;
                newRecharge.LastModifiedBy = objDatasForRecharge.LastModifiedBy;
                objRechargeEntities.Recharges.Add(newRecharge);
                objRechargeEntities.SaveChanges();
                if (objDatasForRecharge.RechargeStatus.ToLower() != "failed")
                {
                    // 2. Wallet Transaction
                    var LastWalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == objDatasForRecharge.CustomerID).FirstOrDefault().WalletBalance;

                    WalletTransaction objWalletTransaction = new WalletTransaction();
                    objWalletTransaction.CustomerID = objDatasForRecharge.CustomerID;
                    objWalletTransaction.IsCredit = false;
                    objWalletTransaction.WalletTransactionDate = objDatasForRecharge.DateAndTime;
                    objWalletTransaction.WalletTransactionReferenceDescription = "Recharge";
                    objWalletTransaction.WalletTransactionReferenceID = newRecharge.RechargeID.ToString();
                    objWalletTransaction.WalletTransactionAmount = objDatasForRecharge.RechargeAmount;
                    objWalletTransaction.WalletTransactionDescription = "Debit transaction for mobile recharge amount";
                    objWalletTransaction.WalletBalance = (Convert.ToDecimal(LastWalletBalanceAmount) - Convert.ToDecimal(objDatasForRecharge.RechargeAmount)).ToString();
                    objWalletTransaction.CreatedDate = objDatasForRecharge.DateAndTime;
                    objWalletTransaction.CreatedBy = objDatasForRecharge.CreatedBy;
                    objRechargeEntities.WalletTransactions.Add(objWalletTransaction);
                    objRechargeEntities.SaveChanges();

                    // 3. Commission Tranction
                    var LastCommissionBalanceAmount = objRechargeEntities.CommissionTransactions.OrderByDescending(n => n.CommissionTransactionID).Where(m => m.CustomerID == objDatasForRecharge.CustomerID).FirstOrDefault().CommissionBalance;
                    var ThisRechargeCommisionPercentage = objRechargeEntities.Commissions.OrderByDescending(n => n.CommissionID).Where(m => m.CustomerID == objDatasForRecharge.CustomerID && m.ProviderID == NetworkID).FirstOrDefault().CommissionPercentage;
                    var ThisRechargeCommissionAmount = (Convert.ToDecimal(objDatasForRecharge.RechargeAmount) * Convert.ToDecimal(ThisRechargeCommisionPercentage)) / 100;


                    CommissionTransaction objCommissionTransaction = new CommissionTransaction();
                    objCommissionTransaction.CustomerID = objDatasForRecharge.CustomerID;
                    objCommissionTransaction.IsCredit = true;
                    objCommissionTransaction.CommissionTransactionDate = objDatasForRecharge.DateAndTime;
                    objCommissionTransaction.CommissionTransactionReferenceDescription = "WalletTransaction";
                    objCommissionTransaction.CommissionTransactionID = objWalletTransaction.WalletTransactionID;
                    objCommissionTransaction.CommissionTransactionAmount = ThisRechargeCommissionAmount.ToString();
                    objCommissionTransaction.CommissionTransactionDescription = "Credit transaction for mobile recharge commission";
                    objCommissionTransaction.CommissionBalance = (Convert.ToDecimal(LastCommissionBalanceAmount) + Convert.ToDecimal(ThisRechargeCommissionAmount)).ToString(); ;
                    objCommissionTransaction.CreatedDate = objDatasForRecharge.DateAndTime;
                    objCommissionTransaction.CreatedBy = objDatasForRecharge.CreatedBy;
                    objRechargeEntities.CommissionTransactions.Add(objCommissionTransaction);
                    objRechargeEntities.SaveChanges();

                }
                var WalletBalanceAmount = "0.00";
                var ProfitBalanceAmount = "0.00";

                WalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == objDatasForRecharge.CustomerID).FirstOrDefault().WalletBalance;
                WalletBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(WalletBalanceAmount));

                ProfitBalanceAmount = objRechargeEntities.CommissionTransactions.OrderByDescending(n => n.CommissionTransactionID).Where(m => m.CustomerID == objDatasForRecharge.CustomerID).FirstOrDefault().CommissionBalance;
                ProfitBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(ProfitBalanceAmount));

                objLoginResponse.CustomerID = objDatasForRecharge.CustomerID.ToString();
                objLoginResponse.WalletBalance = WalletBalanceAmount;
                objLoginResponse.ProfitBalance = ProfitBalanceAmount;

                objLoginResponse.StatusDescription = objDatasForRecharge.RechargeStatusDescription;

                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var exceptionDetails = ex;
                objLoginResponse.CustomerID = "-99";
                objLoginResponse.WalletBalance = ex.StackTrace;
                objLoginResponse.StatusDescription = ex.Message;
                //objLoginResponse.CustomerID = ex.InnerException;
                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);

            }





        }

        public class WalletBalanceForDisplay
        {
            public string WalletBalance { get; set; }
            public string ProfitBalance { get; set; }
        }
        public ActionResult WalletBalanceFunction(DatasForWalletBalance objDatasForWalletBalance)
        {
            try
            {
                string URI = "https://allbills.in/api/wallet-balance";

                var json = new JavaScriptSerializer().Serialize(objDatasForWalletBalance);
                using (WebClient wc = new WebClient())
                {

                    string HtmlResult = wc.UploadString(URI, json);
                    dynamic data = JObject.Parse(HtmlResult);
                    //"Message":"String Content",
                    //"isError":"true",
                    //"wallet":"String Content",
                    //"TimeStamp":"String Content",
                    //"Partnerid":"String Content"


                    return Json(data.wallet, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult LoginFunction(DatasForLogin objDatasForLogin)
        {
            var CustomerID = 0;
            LoginResponse objLoginResponse = new LoginResponse();

            try
            {
                RechargeEntities objRechargeEntities = new RechargeEntities();

                var Customer = objRechargeEntities.Customers.Where(m => m.UserName == objDatasForLogin.userName && m.Password == objDatasForLogin.password).ToList();
                var WalletBalanceAmount = "0.00";
                var ProfitBalanceAmount = "0.00";
                if (Customer.Count == 1)
                {
                    CustomerID = Customer[0].CustomerID;
                    WalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;
                    WalletBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(WalletBalanceAmount));

                    ProfitBalanceAmount = objRechargeEntities.CommissionTransactions.OrderByDescending(n => n.CommissionTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().CommissionBalance;
                    ProfitBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(ProfitBalanceAmount));
                }
                objLoginResponse.CustomerID = CustomerID.ToString();
                objLoginResponse.WalletBalance = WalletBalanceAmount;
                objLoginResponse.ProfitBalance = ProfitBalanceAmount;
                objLoginResponse.StatusDescription = "Logged in successfully";
                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exceptionDetails = ex;
                objLoginResponse.CustomerID = "-99";
                objLoginResponse.WalletBalance = "0";
                objLoginResponse.ProfitBalance = "0";
                objLoginResponse.StatusDescription = "Login failed";
                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);

            }


        }

        public ActionResult MoveProfitToBalanceFunction(TransferProfitToWallet objTransferProfitToWallet)
        {
            int CustomerID = Convert.ToInt32(objTransferProfitToWallet.CustomerID);
            RechargeEntities objRechargeEntities = new RechargeEntities();
            try
            {
                // 3. Commission Tranction
                var LastCommissionBalanceAmount = objRechargeEntities.CommissionTransactions.OrderByDescending(n => n.CommissionTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().CommissionBalance;


                CommissionTransaction objCommissionTransaction = new CommissionTransaction();
                objCommissionTransaction.CustomerID = CustomerID;
                objCommissionTransaction.IsCredit = false;
                objCommissionTransaction.CommissionTransactionDate = objTransferProfitToWallet.DateAndTime;
                objCommissionTransaction.CommissionTransactionReferenceDescription = "CommissionTransaction";
                objCommissionTransaction.CommissionTransactionID = 0;
                objCommissionTransaction.CommissionTransactionAmount = LastCommissionBalanceAmount;
                objCommissionTransaction.CommissionTransactionDescription = "Commission move to wallet";
                objCommissionTransaction.CommissionBalance = "0" ;
                objCommissionTransaction.CreatedDate = objTransferProfitToWallet.DateAndTime;
                objCommissionTransaction.CreatedBy = objTransferProfitToWallet.CreatedBy;
                objRechargeEntities.CommissionTransactions.Add(objCommissionTransaction);
                objRechargeEntities.SaveChanges();

                var LastWalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;

                WalletTransaction objWalletTransaction = new WalletTransaction();
                objWalletTransaction.CustomerID = CustomerID;
                objWalletTransaction.IsCredit = true;
                objWalletTransaction.WalletTransactionDate = objTransferProfitToWallet.DateAndTime;
                objWalletTransaction.WalletTransactionReferenceDescription = "CommissionTransaction";
                objWalletTransaction.WalletTransactionReferenceID = objCommissionTransaction.CommissionTransactionID.ToString();
                objWalletTransaction.WalletTransactionAmount = LastCommissionBalanceAmount;
                objWalletTransaction.WalletTransactionDescription = "Credit transaction move commision to wallet";
                objWalletTransaction.WalletBalance = (Convert.ToDecimal(LastWalletBalanceAmount) + Convert.ToDecimal(LastCommissionBalanceAmount)).ToString();
                objWalletTransaction.CreatedDate = objTransferProfitToWallet.DateAndTime;
                objWalletTransaction.CreatedBy = objTransferProfitToWallet.CreatedBy;
                objRechargeEntities.WalletTransactions.Add(objWalletTransaction);
                objRechargeEntities.SaveChanges();

                LoginResponse objLoginResponse = new LoginResponse();

                var WalletBalanceAmount = "0.00";
                var ProfitBalanceAmount = "0.00";

                WalletBalanceAmount = objRechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;
                WalletBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(WalletBalanceAmount));

                ProfitBalanceAmount = objRechargeEntities.CommissionTransactions.OrderByDescending(n => n.CommissionTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().CommissionBalance;
                ProfitBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(ProfitBalanceAmount));

                objLoginResponse.CustomerID = CustomerID.ToString();
                objLoginResponse.WalletBalance = WalletBalanceAmount;
                objLoginResponse.ProfitBalance = ProfitBalanceAmount;

                objLoginResponse.StatusDescription = "Commission moved to wallet";

                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(objTransferProfitToWallet, JsonRequestBehavior.AllowGet);

            }
        }

        public class LoginResponse
        {
            public string CustomerID { get; set; }
            public string WalletBalance { get; set; }
            public string ProfitBalance { get; set; }

            public string StatusDescription { get; set; }
        }

        public class DatasForWalletBalance
        {
            public string id { get; set; }
            public string token { get; set; }
        }


    }

    public class DatasForWalletBalance
    {
        public string id { get; set; }
        public string token { get; set; }
    }

    public class DatasForRecharge
    {
        public string id { get; set; }
        public string token { get; set; }
        public string RechargeTo { get; set; }
        public string ProviderId { get; set; }
        public string Amount { get; set; }

    }

    public class TransferProfitToWallet
    {
        public string CustomerID { get; set; }
        public string ProfitBalance { get; set; }
        public string WalletBalance { get; set; }
        public string DateAndTime { get; set; }
        public string CreatedBy { get; set; }
    }
}

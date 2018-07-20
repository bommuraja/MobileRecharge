
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
            public string id { get; set; }
            public string token { get; set; }
            public string RechargeTo { get; set; }
            public string ProviderId { get; set; }
            public string Amount { get; set; }
            public string customerID { get; set; }

        }
        public class DatasForLogin
        {
            public string userName { get; set; }
            public string password { get; set; }


        }
        public ActionResult RechargeFunction(DatasForRecharge objDatasForRecharge)
        {
            WalletBalanceForDisplay objWalletBalanceForDisplay = new WalletBalanceForDisplay();

            try
            {

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;



                string URI = "https://allbills.in/api/recharge";

                var json = new JavaScriptSerializer().Serialize(objDatasForRecharge);

                using (WebClient wc = new WebClient())
                {
                    var CustomerID = Convert.ToInt32(objDatasForRecharge.customerID);
                    var ProviderID = Convert.ToInt32(objDatasForRecharge.ProviderId);
                    todorechargeEntities objtodorechargeEntities = new todorechargeEntities();


                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var WalletBalanceAmount = objtodorechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;
                    var ProfitBalanceAmount = objtodorechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().ComissionTotal;
                    objWalletBalanceForDisplay.WalletBalance = "0.00";
                    objWalletBalanceForDisplay.ProfitBalance = "0.00";
                    if (Convert.ToDouble(WalletBalanceAmount) > Convert.ToDouble(objDatasForRecharge.Amount))
                    {
                        string HtmlResult = wc.UploadString(URI, json);
                        dynamic data = JObject.Parse(HtmlResult);
                        // 1 . Recharge Table Entry
                        Recharge newRecharge = new Recharge();
                        newRecharge.Message = data.Message;
                        newRecharge.isError = data.isError;
                        newRecharge.TimeStamp = data.TimeStamp;
                        newRecharge.Partnerid = data.Partnerid;
                        newRecharge.RechargeTo = data.RechargeTo;
                        newRecharge.OperatorName = data.OperatorName;
                        newRecharge.Amount = data.Amount;
                        newRecharge.ProviderId = data.ProviderId;
                        newRecharge.Profit = data.Profit;
                        newRecharge.OperatorReference = data.OperatorReference;
                        newRecharge.Status = data.Status;
                        newRecharge.Statement = data.Statement;
                        newRecharge.PaymentId = data.PaymentId;
                       
                        objtodorechargeEntities.Recharges.Add(newRecharge);
                        objtodorechargeEntities.SaveChanges();

                        // 2. WalletTransaction Table Entry
                        WalletTransaction objWalletTransaction = new WalletTransaction();
                        objWalletTransaction.IsRecharge = true;
                        objWalletTransaction.TransactionTypeID = newRecharge.RechargeID;
                        objWalletTransaction.CustomerID = CustomerID;
                        objWalletTransaction.Date = DateTime.Now.ToShortDateString();
                        objWalletTransaction.Reference = "test";
                        objWalletTransaction.Remark = newRecharge.Status;
                        objWalletTransaction.RechargeAmount = objDatasForRecharge.Amount;
                        var CommissionPercentage = objtodorechargeEntities.Commissions.Where(m => m.CustomerID == CustomerID && m.ProviderID == ProviderID).SingleOrDefault().CommissionPercentage;
                        objWalletTransaction.ComissionAmount = ((Convert.ToDecimal(CommissionPercentage) * Convert.ToDecimal(objDatasForRecharge.Amount)) / 100).ToString();
                        objWalletTransaction.WalletTransactionAmount = (Convert.ToDecimal(objWalletTransaction.RechargeAmount)).ToString();
                        if (newRecharge.Status != "failure")
                        {
                            objWalletTransaction.WalletBalance = (Convert.ToDecimal(WalletBalanceAmount) - Convert.ToDecimal(objWalletTransaction.WalletTransactionAmount)).ToString();
                            objWalletBalanceForDisplay.WalletBalance = String.Format("{0:0.00}", (Convert.ToDecimal(WalletBalanceAmount) - Convert.ToDecimal(objWalletTransaction.WalletTransactionAmount)));

                            objWalletTransaction.ComissionTotal = (Convert.ToDecimal(ProfitBalanceAmount) + Convert.ToDecimal(objWalletTransaction.ComissionAmount)).ToString();
                            objWalletBalanceForDisplay.ProfitBalance = String.Format("{0:0.00}", (Convert.ToDecimal(ProfitBalanceAmount) + Convert.ToDecimal(objWalletTransaction.ComissionAmount)));


                        }
                        else
                        {
                            objWalletTransaction.WalletBalance = Convert.ToDecimal(WalletBalanceAmount).ToString();
                            objWalletBalanceForDisplay.WalletBalance = String.Format("{0:0.00}", Convert.ToDecimal(WalletBalanceAmount));

                            objWalletTransaction.ComissionTotal = Convert.ToDecimal(ProfitBalanceAmount).ToString();
                            objWalletBalanceForDisplay.ProfitBalance = String.Format("{0:0.00}", Convert.ToDecimal(ProfitBalanceAmount));


                        }
                        objtodorechargeEntities.WalletTransactions.Add(objWalletTransaction);
                        objtodorechargeEntities.SaveChanges();
                    }

                    return Json(objWalletBalanceForDisplay, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                return Json(objWalletBalanceForDisplay, JsonRequestBehavior.AllowGet);
            }

            return View();
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
                todorechargeEntities objtodorechargeEntities = new todorechargeEntities();

                var Customer = objtodorechargeEntities.Customers.Where(m => m.UserName == objDatasForLogin.userName && m.Password == objDatasForLogin.password).ToList();
                var WalletBalanceAmount = "0.00";
                var ProfitBalanceAmount = "0.00";
                if (Customer.Count == 1)
                {
                    CustomerID = Customer[0].CustomerID;
                    WalletBalanceAmount = objtodorechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().WalletBalance;
                    WalletBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(WalletBalanceAmount));

                    ProfitBalanceAmount = objtodorechargeEntities.WalletTransactions.OrderByDescending(n => n.WalletTransactionID).Where(m => m.CustomerID == CustomerID).FirstOrDefault().ComissionTotal;
                    ProfitBalanceAmount = String.Format("{0:0.00}", Convert.ToDecimal(ProfitBalanceAmount));
                }
                objLoginResponse.CustomerID = CustomerID.ToString();
                objLoginResponse.WalletBalance = WalletBalanceAmount;
                objLoginResponse.ProfitBalance = ProfitBalanceAmount;
                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exceptionDetails = ex;
                objLoginResponse.CustomerID = "-99";
                return Json(objLoginResponse, JsonRequestBehavior.AllowGet);

            }


        }

        public ActionResult MoveProfitToBalanceFunction(TransferProfitToWallet objTransferProfitToWallet)
        {
            var CustomerID = 0;

            try
            {
                todorechargeEntities objtodorechargeEntities = new todorechargeEntities();
                CustomerID = Convert.ToInt32(objTransferProfitToWallet.CustomerID);
                var LastWalletTransaction = objtodorechargeEntities.WalletTransactions.Where(m => m.CustomerID == CustomerID).OrderByDescending(n=>n.WalletTransactionID).FirstOrDefault();

                var NewWalletTransaction = new WalletTransaction();
                NewWalletTransaction.TransactionTypeID = -21;
                NewWalletTransaction.CustomerID = CustomerID;
                NewWalletTransaction.Date = DateTime.Today.ToString();
                NewWalletTransaction.IsRecharge = false;
                NewWalletTransaction.IsProfitTransfer = true;
                NewWalletTransaction.WalletTransactionAmount = objTransferProfitToWallet.ProfitBalance;
                NewWalletTransaction.WalletBalance = (Convert.ToDecimal(objTransferProfitToWallet.ProfitBalance) + Convert.ToDecimal(objTransferProfitToWallet.WalletBalance)).ToString();
                NewWalletTransaction.RechargeAmount = "0.00";
                NewWalletTransaction.ComissionAmount = "0.00";
                NewWalletTransaction.ComissionTotal = "0.00";
                objtodorechargeEntities.WalletTransactions.Add(NewWalletTransaction);
                objtodorechargeEntities.SaveChanges();

                return Json(objTransferProfitToWallet, JsonRequestBehavior.AllowGet);
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
    }
}

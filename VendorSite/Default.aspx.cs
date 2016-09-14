using ChadWS.WCF.DataContracts;
using EncryptStringSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VendorSite.Proxies;

namespace VendorSite
{
    public partial class Default : System.Web.UI.Page
    {
        private static string VenID { get; set; }

        private async void SendErrorEmail(string message, string subject, List<string> eTo)
        {

            using (var utilityClient = new UtilityClient())
            {
                await utilityClient.SendMessageEmailAsync(message, subject, eTo);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["id"] != null)
                {
                    try
                    {
                        string qs = HttpUtility.UrlDecode(Request.QueryString["id"].ToString()).Replace(' ', '+');
                        VenID = StringCipher.DecryptText(qs, Properties.Settings.Default.DecryptSalt);
                    }
                    catch (Exception ex)
                    {

                        SendErrorEmail(ex.Message, "Vendor Confirmation app error", new List<string> { "patrick.rumery@chadwellsupply.com" });

                        VenID = string.Empty;
                    }

                    if (CheckVendorID(VenID))
                        RadAjaxPanel1.Visible = true;
                    else
                        RadAjaxPanel1.Visible = false;
                }
                else
                {
                    RadAjaxPanel1.Visible = false;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<View_vendor_site_data> lst = GetData();

            RadGrid1.DataSource = lst;
            RadGrid2.DataSource = lst.Take(1);
            //   RadGrid1.Rebind();
        }

        protected static List<View_vendor_site_data> GetData()
        {
            List<View_vendor_site_data> rslt = new List<View_vendor_site_data>();
            if (!string.IsNullOrWhiteSpace(VenID))
            {
                using (var poClient = new PurchaseOrderClient())
                {
                    rslt.AddRange(poClient.POTracking_GetVendorSiteDataByVendorID(VenID));
                }
                //DataTable dt = ws.POTracking_GetUnconfirmedPOsByVendorAsync("");
                //  rsp =  ws.POTracking_GetUnconfirmedPOsByVendor("");
                //return List<ChadWS.UnconfirmedPO> lst = 
            }
            return rslt;
        }

        protected void Notify(string title, string text, int delay)
        {
            RadNotification1.Title = title;
            RadNotification1.Text = text;
            RadNotification1.AutoCloseDelay = delay;
            RadNotification1.Show();
        }

        /// <summary>
        /// Confirmation date is datetime.now
        /// </summary>
        /// <param name="poNum"></param>
        /// <param name="vendorID"></param>
        /// <returns></returns>
        protected async Task ConfirmPO(string poNum, string vendorID)
        {
            try
            {
                using(var vendorClient = new VendorClient())
                {
                    await vendorClient.POTracking_VendorConfirmPOAsync(vendorID, poNum, DateTime.Now, DateTime.Parse("1/1/1900"));
                }
                //await ws.POTracking_InsertVendorTrackingAsync(vendorID, poNum, GetIPAddress(), DateTime.Now);
              
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected async Task ConfirmETA(string poNum, string vendorID, DateTime confDate)
        {
            try
            {
                using (var vendorClient = new VendorClient())
                {
                    await vendorClient.POTracking_VendorConfirmPOAsync(vendorID, poNum, DateTime.Now, confDate);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        protected async void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
 string vendorID = string.Empty;
                    string poNum = string.Empty;
                 
            try
            {
               
                switch (e.CommandName)
                {
                    case "ConfirmPO":
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                             vendorID = item["Vendor_ID"].Text;
                             poNum = item["PO_Num"].Text;
                             
                            TableCell cell = item["EDD"];
                            //  TableCell cell = item["ColumnUniqueName"];
                            RadDatePicker textBox = item.FindControl("radVenConfirmDT") as RadDatePicker;

                            if (textBox.DbSelectedDate != null)
                            {
                                DateTime dt = (DateTime)textBox.DbSelectedDate;
                                await ConfirmPO(poNum, vendorID);
                                await ConfirmETA(poNum, vendorID, dt);

                            //   await Helper.SendEmailToPoOwner(poNum, vendorID);
                                RadGrid1.Rebind();
                            }
                            else
                            {

                                await ConfirmPO(poNum, vendorID);
                               // await Helper.SendEmailToPoOwner(poNum, vendorID);
                                RadGrid1.Rebind();
                            }
                            break;
                        }
                    case "ConfirmETA":
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                             vendorID = item["Vendor_ID"].Text;
                             poNum = item["PO_Num"].Text;
                         
                            TableCell cell = item["EDD"];
                            //  TableCell cell = item["ColumnUniqueName"];
                            RadDatePicker textBox = item.FindControl("radVenConfirmDT") as RadDatePicker;
                            if (textBox.DbSelectedDate != null)
                            {


                                DateTime dt = (DateTime)textBox.DbSelectedDate;
                                /// await ConfirmPO(poNum, vendorID);
                                await ConfirmETA(poNum, vendorID, dt);
                            //  await  Helper.SendEmailToPoOwner(poNum, vendorID);
                                RadGrid1.Rebind();
                            }
                            else
                            {
                                Notify("ETA Date is required", "ETA Date is required", 4000);
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                Button btn = (Button)item["ConfirmPO"].Controls[0];
                RadDatePicker rdp = (RadDatePicker)item["EDD"].Controls[1];
                // DateTime sentDT = (DateTime)item["PO_SentDT"].Text;
                string status = item["PO_status"].Text;
                TableCell cell = (TableCell)item["ConfirmPO"];

               // rdp.MinDate = DateTime.Now.AddDays(1);  rob said to let them put in whatever day they want.

                if (string.IsNullOrWhiteSpace(btn.Text))
                    btn.Visible = false;
                else if(status != "Sent")
                {
                    
                    btn.CommandName = "ConfirmETA";
                    btn.Text = "Confirm ETA Date";
                    btn.Visible = true;
                    rdp.DateInput.CausesValidation = true;
                    btn.CausesValidation = true;
                }
                else
                {
                   
                    //btn.conf
                    rdp.DateInput.CausesValidation = false;
                    btn.CausesValidation = false;
                    btn.CommandName = "ConfirmPO";
                    btn.Text = "Confirm PO Receipt";
                    btn.Visible = true;
                    
                }

            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        protected bool CheckVendorID(string vendorID)
        {
            using (var vendorClient = new VendorClient())
            {
                var ven = vendorClient.GetVendorInfo(vendorID);
                if (!string.IsNullOrEmpty(ven.Vendor_ID))
                    return true;
                else
                    return false;
            }
        }

    }
}
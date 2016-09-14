using ChadWS.WCF.Contracts;
using ChadWS.WCF.DataContracts;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace VendorSite.Proxies
{
    public class PurchaseOrderClient : ExceptionHandlingClientBase<IPurchaseOrder>
    {
        public PurchaseOrderClient() : base("NetTcpBinding_IPurchaseOrder") {}

        public List<View_vendor_site_data> POTracking_GetVendorSiteDataByVendorID(string vendorID)
        {
            using (WindowsIdentity.GetCurrent().Impersonate())
            {
                return Channel.POTracking_GetVendorSiteDataByVendorID(vendorID);
            }
        }

        public string GetUser2EntByUnpostedPoNum(string poNum)
        {
            using (WindowsIdentity.GetCurrent().Impersonate())
            {
                return Channel.GetUser2EntByUnpostedPoNum(poNum);
            }
        }
    }
}
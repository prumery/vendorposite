using ChadWS.WCF.Contracts;
using ChadWS.WCF.DataContracts;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace VendorSite.Proxies
{
    public class VendorClient : ExceptionHandlingClientBase<IVendor>
    {
        public VendorClient() : base("NetTcpBinding_IVendor") { }

        public Task<bool> POTracking_VendorConfirmPOAsync(string vendorID, string poNum, DateTime confirmDT, DateTime estimatedDeliveryDT)
        {
            return Task.Run(() => POTracking_VendorConfirmPO(vendorID, poNum, confirmDT, estimatedDeliveryDT));
        }

        protected bool POTracking_VendorConfirmPO(string vendorID, string poNum, DateTime confirmDT, DateTime estimatedDeliveryDT)
        {
            using (WindowsIdentity.GetCurrent().Impersonate())
            {
                return Channel.POTracking_VendorConfirmPO(vendorID, poNum, confirmDT, estimatedDeliveryDT);
            }
        }

        public View_Vendor_Info_Base GetVendorInfo(string vendorID)
        {
            using (WindowsIdentity.GetCurrent().Impersonate())
            {
                return Channel.GetVendorInfo(vendorID);
            }
        }
    }
}
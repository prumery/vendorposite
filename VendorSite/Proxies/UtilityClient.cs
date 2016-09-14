using ChadWS.WCF.Contracts;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace VendorSite.Proxies
{
    public class UtilityClient : ExceptionHandlingClientBase<IUtility>
    {
        public UtilityClient() : base("NetTcpBinding_IUtility") { }

        public Task SendMessageEmailAsync(string message, string subject, List<string> eTo)
        {
            return Task.Run(() => SendMessageEmailAsync(message, subject, eTo));
        }

        public void SendMessageEmail(string message, string subject, List<string> eTo)
        {
            using (WindowsIdentity.GetCurrent().Impersonate())
            {
                Channel.SendMessageEmail(message, subject, eTo);
            }
        }
    }
}
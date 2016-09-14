using System;
using System.ServiceModel;

namespace VendorSite.Proxies
{
    public abstract class ExceptionHandlingClientBase<T> : ClientBase<T>, IDisposable where T : class
    {
        public ExceptionHandlingClientBase(string endpointConfigurationName) : base (endpointConfigurationName) { }

        public void Dispose()
        {
            bool success = false;
            try
            {
                if (State != CommunicationState.Faulted)
                {
                    Close();
                    success = true;
                }
            }
            finally
            {
                if (!success)
                    Abort();
            }
        }
    }
}
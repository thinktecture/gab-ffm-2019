using Microsoft.Azure.WebJobs.Host.Config;

namespace TcpListenerTrigger
{
    public class TcpListenerTriggerExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<TcpListenerTriggerAttribute>().BindToTrigger(new TcpListenerTriggerBindingProvider());
        }
    }
}

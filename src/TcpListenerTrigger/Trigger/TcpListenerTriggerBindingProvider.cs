using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace TcpListenerTrigger
{
    public class TcpListenerTriggerBindingProvider : ITriggerBindingProvider
    {
        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<TcpListenerTriggerAttribute>(false);

            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            if (parameter.ParameterType != typeof(string))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can't bind TcpListenerTriggerAttribute to type '{0}'.", parameter.ParameterType));
            }

            return
                Task.FromResult<ITriggerBinding>(new TcpListenerTriggerBinding(context.Parameter, context.Parameter.Member.Name));
        }
    }
}

using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TcpListenerTrigger
{
    public class TcpListenerTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameterInfo;
        private readonly string _parameterName;

        public TcpListenerTriggerBinding(ParameterInfo parameterInfo, string parameterName)
        {
            _parameterInfo = parameterInfo;
            _parameterName = parameterName;
        }

        public Type TriggerValueType => typeof(string);
        public IReadOnlyDictionary<string, Type> BindingDataContract { get; }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                {"data", value}
            };

            var valueBinder = new TcpListenerValueBinder((string)value);
            return Task.FromResult<ITriggerData>(new TriggerData(valueBinder, bindingData));
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context) =>
            Task.FromResult<IListener>(new TcpListener(context.Executor));

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor()
        {
            Name = _parameterName,
            DisplayHints = new ParameterDisplayHints()
            {
                Prompt = "Some Prompt",
                Description = "Some Descriptor",
                DefaultValue = "Some DefaultValue"
            }
        };

        private class TcpListenerValueBinder : IValueBinder
        {
            private string _value;

            public TcpListenerValueBinder(string value)
            {
                _value = value;
            }

            public Type Type => typeof(string);

            public Task<object> GetValueAsync() => Task.FromResult<object>(_value);
            public Task SetValueAsync(object value, CancellationToken cancellationToken)
            {
                _value = value.ToString();
                return Task.CompletedTask;
            }

            public string ToInvokeString() => _value;
        }
    }
}

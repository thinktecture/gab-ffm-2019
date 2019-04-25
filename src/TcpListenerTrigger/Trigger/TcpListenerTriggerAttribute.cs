using Microsoft.Azure.WebJobs.Description;
using System;

namespace TcpListenerTrigger
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public sealed class TcpListenerTriggerAttribute : Attribute
    {
    }
}

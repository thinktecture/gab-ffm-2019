using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TcpListenerTrigger
{
    public class TcpListener : IListener
    {
        private System.Net.Sockets.TcpListener _listener;
        private readonly ITriggeredFunctionExecutor _executor;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _listenerTask;

        public TcpListener(ITriggeredFunctionExecutor executor)
        {
            _executor = executor;
        }
        public void Cancel() { }
        public void Dispose() => throw new NotImplementedException();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _listener = new System.Net.Sockets.TcpListener(IPAddress.Any, 80);
            _listener.Start();
            _listenerTask = ListenAsync(_cancellationTokenSource.Token);

            return _listenerTask.IsCompleted ? _listenerTask : Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _listener.Stop();

            return Task.CompletedTask;
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            // Buffer for reading data
            var bytes = new Byte[256];
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                var stream = client.GetStream();
                int i;
                while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    var data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    await _executor.TryExecuteAsync(new TriggeredFunctionData() { TriggerValue = data }, CancellationToken.None);
                }
                client.Close();
            }
        }
    }
}

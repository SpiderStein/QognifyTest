using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace QognifyTest
{
    internal class Subscriber
    {
        public Subscriber(LoggingServer loggingServer)
        {
            loggingServer.Subscribe(SecurityEventHandler);

            async Task SecurityEventHandler(ISecurityEvent securityEvent)
            {
                await Task.Run(() =>
                {
                    Print(securityEvent);
                }).ConfigureAwait(false);
            }

            void Print(ISecurityEvent securityEvent)
            {
                var JsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
                var eventJson = JsonSerializer.Serialize<object>(securityEvent, JsonSerializerOptions);
                Console.WriteLine($"{eventJson}{Environment.NewLine}{Environment.NewLine}");
            }
        }
    }
}
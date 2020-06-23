using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace QognifyTest
{
    internal class LoggingServer
    {
        private ConcurrentQueue<ISecurityEvent> SecurityEvents { get; set; }
        private IList<Func<ISecurityEvent, Task>> SecurityEventHandlers { get; set; }
        public LoggingServer(
            IEnumerable<ISecurityEventsProducer> producers,
            IList<Func<ISecurityEvent, Task>> securityEventHandlers)
        {
            this.SecurityEvents = new ConcurrentQueue<ISecurityEvent>();
            /*
                Relating to the operation above, in a real life scenario I will choose RX lib's 'Subject' like data structure
                that is also thread safe, and insert it by DI
            */
            this.SecurityEventHandlers = securityEventHandlers;
            SubscribeToProducers(producers);




            void SubscribeToProducers(IEnumerable<ISecurityEventsProducer> producers)
            {
                foreach (var producer in producers)
                {
                    producer.Subscribe(SecurityEventHandler);
                }
            }

            async Task SecurityEventHandler(ISecurityEvent securityEvent)
            {
                // The implementation will be changed, when using ConcurrentQueue alternative that fits better to our case

                await Task.Run(async () =>
                {
                    this.SecurityEvents.Enqueue(securityEvent);
                    ISecurityEvent? dequeuedEvent;
                    if (this.SecurityEvents.TryDequeue(out dequeuedEvent))
                    {
                        await Produce(dequeuedEvent, this.SecurityEventHandlers);
                    }
                }).ConfigureAwait(false);
            }

            async Task Produce(ISecurityEvent securityEvent, IList<Func<ISecurityEvent, Task>> securityEventHandlers)
            {
                await InvokeSecurityEventHandlersInParallel(securityEventHandlers, securityEvent);

                async Task InvokeSecurityEventHandlersInParallel(
                    IList<Func<ISecurityEvent, Task>> securityEventHandlers,
                    ISecurityEvent securityEvent)
                {
                    var handlersExecutions = new List<Task>();
                    foreach (var handler in securityEventHandlers)
                    {
                        handlersExecutions.Add(handler(securityEvent));
                    }

                    await Task.WhenAll(handlersExecutions);
                }
            }
        }

        public void Subscribe(Func<ISecurityEvent, Task> securityEventHandler)
        {
            this.SecurityEventHandlers.Add(securityEventHandler);
        }
    }
}
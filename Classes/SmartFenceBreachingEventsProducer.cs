using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QognifyTest
{
    internal class SmartFenceBreachingEventsProducer : ISecurityEventsProducer
    {
        private IEnumerable<SmartFenceBreaching> SmartFenceBreachingEvents { get; set; }
        private IList<Func<ISecurityEvent, Task>> SmartFenceBreachingEventHandlers { get; set; }
        public SmartFenceBreachingEventsProducer(
            IEnumerable<SmartFenceBreaching> smartFenceBreachingEvents,
            IList<Func<ISecurityEvent, Task>> smartFenceBreachingEventHandlers)
        {
            this.SmartFenceBreachingEvents = smartFenceBreachingEvents;
            this.SmartFenceBreachingEventHandlers = smartFenceBreachingEventHandlers;
        }
        public void Subscribe(Func<ISecurityEvent, Task> smartFenceBreachingEventHandler)
        {
            this.SmartFenceBreachingEventHandlers.Add(smartFenceBreachingEventHandler);
        }

        public async Task Produce()
        {
            foreach (var smartFenceBreachingEvent in this.SmartFenceBreachingEvents)
            {
                await InvokeSmartFenceBreachingEventHandlersInParallel(this.SmartFenceBreachingEventHandlers, smartFenceBreachingEvent);
            }

            async Task InvokeSmartFenceBreachingEventHandlersInParallel(IList<Func<ISecurityEvent, Task>> smartFenceBreachingEventHandlers,
                SmartFenceBreaching smartFenceBreachingEvent)
            {
                var handlersExecutions = new List<Task>();
                foreach (var handler in smartFenceBreachingEventHandlers)
                {
                    handlersExecutions.Add(handler(smartFenceBreachingEvent));
                }

                await Task.WhenAll(handlersExecutions);
            }
        }
    }
}
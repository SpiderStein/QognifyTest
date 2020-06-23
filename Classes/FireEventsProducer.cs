using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QognifyTest
{
    internal class FireEventsProducer : ISecurityEventsProducer
    {
        private IEnumerable<Fire> FireEvents { get; set; }
        private IList<Func<ISecurityEvent, Task>> FireEventHandlers { get; set; }
        public FireEventsProducer(
            IEnumerable<Fire> fireEvents,
            IList<Func<ISecurityEvent, Task>> fireEventHandlers)
        {
            this.FireEvents = fireEvents;
            this.FireEventHandlers = fireEventHandlers;
        }
        public void Subscribe(Func<ISecurityEvent, Task> fireEventHandler)
        {
            this.FireEventHandlers.Add(fireEventHandler);
        }

        public async Task Produce()
        {
            foreach (var fireEvent in this.FireEvents)
            {
                await InvokeFireEventHandlersInParallel(this.FireEventHandlers, fireEvent);
            }

            async Task InvokeFireEventHandlersInParallel(IList<Func<ISecurityEvent, Task>> fireEventHandlers,
                Fire fireEvent)
            {
                var handlersExecutions = new List<Task>();
                foreach (var handler in fireEventHandlers)
                {
                    handlersExecutions.Add(handler(fireEvent));
                }

                await Task.WhenAll(handlersExecutions);
            }
        }
    }
}
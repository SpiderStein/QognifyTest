using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QognifyTest
{
    internal class VideoAnomalyEventsProducer : ISecurityEventsProducer
    {
        private IEnumerable<VideoAnomaly> VideoAnomalyEvents { get; set; }
        private IList<Func<ISecurityEvent, Task>> VideoAnomalyEventHandlers { get; set; }
        public VideoAnomalyEventsProducer(
            IEnumerable<VideoAnomaly> videoAnomalyEvents,
            IList<Func<ISecurityEvent, Task>> videoAnomalyEventHandlers
            )
        {
            this.VideoAnomalyEvents = videoAnomalyEvents;
            this.VideoAnomalyEventHandlers = videoAnomalyEventHandlers;
        }
        public void Subscribe(Func<ISecurityEvent, Task> videoAnomalyEventHandler)
        {
            this.VideoAnomalyEventHandlers.Add(videoAnomalyEventHandler);
        }

        public async Task Produce()
        {
            foreach (var videoAnomalyEvent in this.VideoAnomalyEvents)
            {
                await InvokeVideoAnomalyEventHandlersInParallel(this.VideoAnomalyEventHandlers, videoAnomalyEvent);
            }

            async Task InvokeVideoAnomalyEventHandlersInParallel(IList<Func<ISecurityEvent, Task>> videoAnomalyEventHandlers,
                VideoAnomaly videoAnomalyEvent)
            {
                var handlersExecutions = new List<Task>();
                foreach (var handler in videoAnomalyEventHandlers)
                {
                    handlersExecutions.Add(handler(videoAnomalyEvent));
                }

                await Task.WhenAll(handlersExecutions);
            }
        }
    }
}
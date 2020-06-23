using System.Collections.Generic;
using System.Threading.Tasks;

namespace QognifyTest
{
    internal class Program
    {
        private LoggingServer LoggingServer { get; set; }
        private Subscriber Subscriber { get; set; }
        private IEnumerable<ISecurityEventsProducer> Producers { get; set; }
        public Program(
            LoggingServer loggingServer,
            Subscriber subscriber,
            IEnumerable<ISecurityEventsProducer> producers)
        {
            this.LoggingServer = loggingServer;
            this.Subscriber = subscriber;
            this.Producers = producers;
        }
        public async Task Run()
        {
            var producersProduceMethodExecutions = new List<Task>();
            foreach (var producer in this.Producers)
            {
                producersProduceMethodExecutions.Add(producer.Produce());
            }
            await Task.WhenAll(producersProduceMethodExecutions);
        }
    }
}
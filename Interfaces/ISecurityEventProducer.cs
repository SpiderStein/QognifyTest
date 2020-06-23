using System;
using System.Threading.Tasks;

namespace QognifyTest
{
    internal interface ISecurityEventsProducer
    {
        void Subscribe(Func<ISecurityEvent, Task> securityEventHandler);
        Task Produce();
    }
}
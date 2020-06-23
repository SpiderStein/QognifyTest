using System;

namespace QognifyTest
{
    internal class SmartFenceBreaching : ISecurityEvent
    {
        public Guid Id { get; set; }
        public Guid FenceId { get; set; }
    }
}
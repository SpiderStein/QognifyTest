using System;

namespace QognifyTest
{
    internal class Fire : ISecurityEvent
    {
        public Guid Id { get; set; }
        public Size Size { get; set; }
    }
}
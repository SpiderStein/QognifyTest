using System;

namespace QognifyTest
{
    internal class VideoAnomaly : ISecurityEvent
    {
        public Guid Id { get; set; }
        public Quadrilateral FirstFrameLocation { get; set; }
    }
}
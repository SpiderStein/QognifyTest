using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

namespace QognifyTest
{
    internal class EntryPoint
    {
        private static async Task Main()
        {
            var iocContainer = GetFilledIocContainer(containerBuilder: new ContainerBuilder());
            using var scope = iocContainer.BeginLifetimeScope();
            await scope.Resolve<Program>().Run();

            IContainer GetFilledIocContainer(ContainerBuilder containerBuilder)
            {
                containerBuilder.Register(c => new List<Fire>() { GenerateFireEvent(), GenerateFireEvent() })
                    .As<IEnumerable<Fire>>();
                containerBuilder.Register(c => new List<Func<ISecurityEvent, Task>>())
                    .As<IList<Func<ISecurityEvent, Task>>>();
                containerBuilder.RegisterType<FireEventsProducer>().AsSelf().SingleInstance();
                containerBuilder.Register(c => new List<SmartFenceBreaching>(){ GenerateSmartFenceBreachingEvent(),
                    GenerateSmartFenceBreachingEvent() }).As<IEnumerable<SmartFenceBreaching>>();
                containerBuilder.RegisterType<SmartFenceBreachingEventsProducer>().AsSelf().SingleInstance();
                containerBuilder.Register(c => new List<VideoAnomaly>()
                    { GenerateVideoAnomalyEvent(), GenerateVideoAnomalyEvent() }).As<IEnumerable<VideoAnomaly>>();
                containerBuilder.RegisterType<VideoAnomalyEventsProducer>().AsSelf().SingleInstance();
                containerBuilder.Register(c => new List<ISecurityEventsProducer>()
                    {c.Resolve<FireEventsProducer>(), c.Resolve<SmartFenceBreachingEventsProducer>(),
                    c.Resolve<VideoAnomalyEventsProducer>() }).As<IEnumerable<ISecurityEventsProducer>>();
                containerBuilder.RegisterType<LoggingServer>().AsSelf().SingleInstance();
                containerBuilder.RegisterType<Subscriber>().AsSelf();
                containerBuilder.RegisterType<Program>().AsSelf();

                return containerBuilder.Build();

                Fire GenerateFireEvent()
                {
                    return new Fire
                    {
                        Id = Guid.NewGuid(),
                        Size = Size.Medium
                    };
                }

                SmartFenceBreaching GenerateSmartFenceBreachingEvent()
                {
                    return new SmartFenceBreaching
                    {
                        Id = Guid.NewGuid(),
                        FenceId = Guid.NewGuid()
                    };
                }

                VideoAnomaly GenerateVideoAnomalyEvent()
                {
                    return new VideoAnomaly
                    {
                        Id = Guid.NewGuid(),
                        FirstFrameLocation = GenerateQuadrilateral()
                    };

                    Quadrilateral GenerateQuadrilateral()
                    {
                        return new Quadrilateral
                        {
                            Edge1 = new Point2D { Coordinate1 = 1, Coordinate2 = 1 },
                            Edge2 = new Point2D { Coordinate1 = 1, Coordinate2 = 1 },
                            Edge3 = new Point2D { Coordinate1 = 1, Coordinate2 = 1 },
                            Edge4 = new Point2D { Coordinate1 = 1, Coordinate2 = 1 },
                        };
                    }
                }
            }
        }
    }
}
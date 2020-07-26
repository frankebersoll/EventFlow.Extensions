using System.Threading;
using EventFlow.Configuration;
using EventFlow.Extensions;
using Xunit;

namespace EventFlow.Functional.Tests
{
    public class CustomerTests
    {
        [Fact]
        public void Test()
        {
            IEventFlowOptions eventFlow = EventFlowOptions.New
                .AddDefaults(typeof(CustomerTests).Assembly);

            CustomerDefinition.Define(eventFlow);

            IRootResolver resolver = eventFlow.CreateResolver();
            ICommandBus bus = resolver.Resolve<ICommandBus>();

            CustomerId aggregateId = CustomerId.New;
            bus.PublishAsync(new CreateCustomer(aggregateId), CancellationToken.None);
            bus.PublishAsync(new DeleteCustomer(aggregateId), CancellationToken.None);
        }
    }
}

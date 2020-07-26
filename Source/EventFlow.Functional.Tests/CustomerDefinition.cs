using EventFlow.Aggregates;
using EventFlow.Commands;
using EventFlow.Core;
using Agg = EventFlow.Functional.FnAgg<
    EventFlow.Functional.Tests.CustomerAggregate,
    EventFlow.Functional.Tests.CustomerId,
    EventFlow.Functional.Tests.CustomerState>;

namespace EventFlow.Functional.Tests
{
    public class CustomerId : Identity<CustomerId>
    {
        public CustomerId(string value) : base(value)
        {
        }
    }

    public enum CustomerState
    {
        Initial,
        Created,
        Deleted
    }

    public class CustomerAggregate : FnAggregateRoot<CustomerAggregate, CustomerId, CustomerState>
    {
        public CustomerAggregate(CustomerId id, IStateApplier stateApplier) : base(id, stateApplier)
        {
        }
    }

    public abstract class CustomerCommand : Command<CustomerAggregate, CustomerId>
    {
        protected CustomerCommand(CustomerId aggregateId) : base(aggregateId)
        {
        }
    }

    public abstract class CustomerEvent : IAggregateEvent<CustomerAggregate, CustomerId>
    {
    }

    public class CreateCustomer : CustomerCommand
    {
        public CreateCustomer(CustomerId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CustomerCreated : CustomerEvent
    {
    }

    public class DeleteCustomer : CustomerCommand
    {
        public DeleteCustomer(CustomerId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CustomerDeleted : CustomerEvent
    {
    }

    public static class CustomerDefinition
    {
        public static void Define(IEventFlowOptions options)
        {
            var definition = Agg.DefineAggregate(

                () => CustomerState.Initial,

                Agg.Handlers(
                    Agg.Handle<CreateCustomer>(c => new CustomerCreated()),
                    Agg.Handle<DeleteCustomer>(c => new CustomerDeleted())
                ),

                Agg.Transitions(
                    Agg.Transition<CustomerCreated>(() => CustomerState.Created),
                    Agg.Transition<CustomerDeleted>(() => CustomerState.Deleted)
                )
            );

            Agg.RegisterAggregate(definition, options);
        }
    }
}

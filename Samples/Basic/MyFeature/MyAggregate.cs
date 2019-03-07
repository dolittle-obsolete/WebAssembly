using Dolittle.Domain;
using Dolittle.Runtime.Events;

namespace Basic.MyFeature
{

    public class MyAggregate : AggregateRoot
    {
        public MyAggregate(EventSourceId id) : base(id)
        {}

        public void DoStuff()
        {
            Apply(new MyEvent());

        }

        public void Delete()
        {
            Apply(new AnimalDeleted());
        }

    }
}
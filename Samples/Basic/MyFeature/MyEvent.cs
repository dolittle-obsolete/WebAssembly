using System;
using Dolittle.Events;

namespace Basic.MyFeature
{

    public class MyEvent : IEvent
    {
        public MyEvent()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
            Number = 1;
            Text = "Hello";
        }

        public MyEvent(Guid id, DateTimeOffset created, int number, string text)
        {
            Id = id;
            Created = created;
            Number = number;
            Text = text;
        }

        public Guid Id { get; }
        public DateTimeOffset Created { get; }
        public int Number { get; }
        public string Text { get; }
    }
}
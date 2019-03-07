using System;
using Dolittle.Commands;

namespace Basic.MyFeature
{
    public class DeleteAnimal : ICommand
    {
        public Guid Animal { get; set; }
    }
}
using System;
using Dolittle.ReadModels;

namespace Basic.MyFeature
{
    public class Animal : IReadModel
    {
        public Guid Id { get; set; }
        public string Species { get; set; }
        public string Name { get; set; }
    }
}
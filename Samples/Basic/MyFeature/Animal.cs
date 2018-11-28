using Dolittle.ReadModels;

namespace Basic.MyFeature
{


    public class Animal : IReadModel
    {
        public string Species { get; set; }
        public string Name { get; set; }
    }
}
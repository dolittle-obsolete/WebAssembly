using Dolittle.Logging;
using Dolittle.Types;

namespace Basic
{
    public class Foo : IFoo
    {
        private readonly IInstancesOf<IAmSomething> _somethings;
        private readonly ILogger _logger;

        public Foo(ILogger logger, IInstancesOf<IAmSomething> somethings)
        {
            _somethings = somethings;
            _logger = logger;
        }

        public void DoStuff()
        {
            _logger.Information("Doing stuff");
            _somethings.ForEach(_ => _logger.Information($"The hashcode is : {_.GetHashCode()}"));
        }
    }
}
using Mediator;
using Northgard.GameWorld.Abstraction.Behaviours;

namespace Northgard.GameWorld.Mediation.Commands
{
    internal class FindNaturalDistrictMCmd : ICommand<INaturalDistrictBehaviour>
    {
        public string NaturalDistrictId { get; }
        public FindNaturalDistrictMCmd(string naturalDistrictId)
        {
            NaturalDistrictId = naturalDistrictId;
        }
    }
}
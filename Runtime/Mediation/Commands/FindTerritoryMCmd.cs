using Mediator;
using Northgard.GameWorld.Abstraction.Behaviours;

namespace Northgard.GameWorld.Mediation.Commands
{
    internal class FindTerritoryMCmd : ICommand<ITerritoryBehaviour>
    {
        public string TerritoryId { get; }
        public FindTerritoryMCmd(string territoryId)
        {
            TerritoryId = territoryId;
        }
    }
}
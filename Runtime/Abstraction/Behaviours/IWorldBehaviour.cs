using System.Collections.Generic;
using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface IWorldBehaviour : IGameObjectBehaviour<World>
    {
        IEnumerable<ITerritoryBehaviour> Territories { get; }
        void AddTerritory(ITerritoryBehaviour territory);
        void RemoveTerritory(ITerritoryBehaviour territory);
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        public delegate void WorldBehaviourDelegate(IWorldBehaviour worldBehaviour);
    }
}
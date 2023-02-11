using Northgard.Core.GameObjectBase;
using Northgard.Enterprise.Entities.WorldEntities;
using UnityEngine;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface IWorldBehaviour : IGameObjectBehaviour<World>
    {
        ITerritoryBehaviour[][] Territories { get; }
        void AddTerritory(ITerritoryBehaviour territory, Vector2Int pointInWorld);
        void RemoveTerritory(Vector2Int pointInWorld);
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        public delegate void WorldBehaviourDelegate(IWorldBehaviour worldBehaviour);
    }
}
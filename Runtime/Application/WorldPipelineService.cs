using System.Collections.Generic;
using System.Linq;
using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Configurations;
using Northgard.GameWorld.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Application
{
    internal class WorldPipelineService : MonoBehaviour, IWorldPipelineService
    {
        [SerializeField] private WorldPipelineConfig config;
        [SerializeField] private Transform worldPlace;
        public IEnumerable<World> WorldPrefabs => config.WorldPrefabs.Select(wp => wp.Data);
        public IWorldBehaviour World { get; private set; }
        public void SetWorld(string worldId)
        {
            if (World != null)
            {
                DestroyWorld();
            }
            World = InstantiateWorld(worldId);
        }

        public void DestroyWorld()
        {
            World.Destroy();
            World = null;
        }

        private IWorldBehaviour InstantiateWorld(string worldId)
        {
            var worldPrefab = config.FindWorldPrefab(worldId);
            var worldInstance = worldPrefab.Instantiate();
            worldInstance.SetPosition(worldPlace.position);
            worldInstance.SetRotation(worldPlace.rotation);
            return worldInstance;
        }

        private ITerritoryBehaviour InstantiateTerritory(string territoryId, Vector3 initialPosition, Quaternion initialRotation)
        {
            var territoryPrefab = config.FindTerritoryPrefab(territoryId);
            var territoryInstance = territoryPrefab.Instantiate();
            territoryInstance.SetPosition(initialPosition);
            territoryInstance.SetRotation(initialRotation);
            return territoryInstance;
        }
    }
}
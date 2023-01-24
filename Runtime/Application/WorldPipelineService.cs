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
        public IEnumerable<Territory> TerritoryPrefabs => config.TerritoryPrefabs.Select(wp => wp.Data);
        public IEnumerable<NaturalDistrict> NaturalDistrictPrefabs =>
            config.NaturalDistrictPrefabs.Select(wp => wp.Data);
        public IWorldBehaviour World { get; private set; }
        public void SetWorld(World world)
        {
            if (World != null)
            {
                DestroyWorld();
            }
            World = InstantiateWorld(world);
        }

        public void DestroyWorld()
        {
            World.Destroy();
            World = null;
        }

        private IWorldBehaviour InstantiateWorld(World world)
        {
            var worldPrefab = config.FindWorldPrefab(world.prefabId);
            var worldInstance = worldPrefab.Instantiate(world);
            return worldInstance as IWorldBehaviour;
        }

        public ITerritoryBehaviour InstantiateTerritory(Territory territory)
        {
            var territoryPrefab = config.FindTerritoryPrefab(territory.prefabId);
            var territoryInstance = territoryPrefab.Instantiate(territory);
            return territoryInstance as ITerritoryBehaviour;
        }

        public INaturalDistrictBehaviour InstantiateNaturalDistrict(NaturalDistrict naturalDistrict)
        {
            var naturalDistrictPrefab = config.FindNaturalDistrictPrefab(naturalDistrict.prefabId);
            var naturalDistrictInstance = naturalDistrictPrefab.Instantiate(naturalDistrict);
            return naturalDistrictInstance as INaturalDistrictBehaviour;
        }
    }
}
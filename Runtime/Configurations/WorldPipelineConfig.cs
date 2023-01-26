using System.Collections.Generic;
using System.Linq;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Application.Behaviours;
using UnityEngine;

namespace Northgard.GameWorld.Configurations
{
    [CreateAssetMenu]
    internal class WorldPipelineConfig : ScriptableObject
    {
        [SerializeField] private WorldBehaviour[] worldPrefabs;
        [SerializeField] private TerritoryBehaviour[] territoryPrefabs;
        [SerializeField] private NaturalDistrictBehaviour[] naturalDistrictPrefabs;
        public IEnumerable<IWorldBehaviour> WorldPrefabs => worldPrefabs;
        public IEnumerable<ITerritoryBehaviour> TerritoryPrefabs => territoryPrefabs;
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistrictPrefabs => naturalDistrictPrefabs;
        
        public IWorldBehaviour FindWorldPrefab(string worldId)
        {
            return worldPrefabs.FirstOrDefault(wp => wp.Data.prefabId == worldId);
        }

        public ITerritoryBehaviour FindTerritoryPrefab(string territoryId)
        {
            return territoryPrefabs.FirstOrDefault(wp => wp.Data.prefabId == territoryId);
        }

        public INaturalDistrictBehaviour FindNaturalDistrictPrefab(string naturalDistrictId)
        {
            return naturalDistrictPrefabs.FirstOrDefault(wp => wp.Data.prefabId == naturalDistrictId);
        }
    }
}
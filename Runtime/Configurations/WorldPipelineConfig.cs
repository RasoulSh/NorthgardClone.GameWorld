using System.Linq;
using JetBrains.Annotations;
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
        public IWorldBehaviour[] WorldPrefabs => worldPrefabs as IWorldBehaviour[];
        public ITerritoryBehaviour[] TerritoryPrefabs => territoryPrefabs as ITerritoryBehaviour[];
        public INaturalDistrictBehaviour[] NaturalDistrictPrefabs => naturalDistrictPrefabs as INaturalDistrictBehaviour[];
        
        public IWorldBehaviour FindWorldPrefab(string worldId)
        {
            return worldPrefabs.FirstOrDefault(wp => wp.Data.id == worldId);
        }

        public ITerritoryBehaviour FindTerritoryPrefab(string territoryId)
        {
            return territoryPrefabs.FirstOrDefault(wp => wp.Data.id == territoryId);
        }

        public INaturalDistrictBehaviour FindNaturalDistrictPrefab(string naturalDistrictId)
        {
            return naturalDistrictPrefabs.FirstOrDefault(wp => wp.Data.id == naturalDistrictId);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Abstraction
{
    public interface IWorldPipelineService
    {
        IEnumerable<World> WorldPrefabs { get; }
        IEnumerable<Territory> TerritoryPrefabs { get; }
        IEnumerable<NaturalDistrict> NaturalDistrictPrefabs { get; }
        public IWorldBehaviour World { get; }
        void SetWorld(World world);

        ITerritoryBehaviour InstantiateTerritory(Territory territory);
        INaturalDistrictBehaviour InstantiateNaturalDistrict(NaturalDistrict naturalDistrict);
        void DestroyWorld();
    }
}
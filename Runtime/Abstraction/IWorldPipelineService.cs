using System.Collections.Generic;
using Northgard.Enterprise.Entities.WorldEntities;
using Northgard.GameWorld.Abstraction.Behaviours;
using UnityEngine;

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
        void DestroyTerritory(string id);
        void DestroyNaturalDistrict(string id);
        void DestroyWorld();
        void Initialize();
        IEnumerable<ITerritoryBehaviour> Territories { get; }
        IEnumerable<INaturalDistrictBehaviour> NaturalDistricts { get; }
        ITerritoryBehaviour FindTerritory(string territoryId);
        INaturalDistrictBehaviour FindNaturalDistrict(string naturalDistrictId);
        GameObject GenerateFakeNaturalDistrict(string prefabId);
    }
}
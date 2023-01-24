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
        void SetWorld(string worldId);

        ITerritoryBehaviour InstantiateTerritory(string territoryId, Vector3 initialPosition,
            Quaternion initialRotation);
        INaturalDistrictBehaviour InstantiateNaturalDistrict(string naturalDistrictId, Vector3 initialPosition,
            Quaternion initialRotation);
        void DestroyWorld();
    }
}
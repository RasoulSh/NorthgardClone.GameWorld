using System.Collections.Generic;
using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface ITerritoryBehaviour : IGameObjectBehaviour<Territory>
    {
        IEnumerable<INaturalDistrictBehaviour> NaturalDistricts { get; }
        new ITerritoryBehaviour Instantiate();

        void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;

        public delegate void TerritoryBehaviourDelegate(ITerritoryBehaviour territoryBehaviour);
        public delegate void TerritoryNaturalDistrictDelegate(ITerritoryBehaviour territory,
            INaturalDistrictBehaviour naturalDistrict);
    }
}
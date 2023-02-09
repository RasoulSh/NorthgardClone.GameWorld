using System.Collections.Generic;
using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Enums;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface ITerritoryBehaviour : IGameObjectBehaviour<Territory>
    {
        IEnumerable<INaturalDistrictBehaviour> NaturalDistricts { get; }
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;
        void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        public delegate void TerritoryBehaviourDelegate(ITerritoryBehaviour territoryBehaviour);
        public delegate void TerritoryNaturalDistrictDelegate(ITerritoryBehaviour territory,
            INaturalDistrictBehaviour naturalDistrict);
    }
}
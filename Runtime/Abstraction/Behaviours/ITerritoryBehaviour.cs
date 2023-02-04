using System.Collections.Generic;
using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Enums;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface ITerritoryBehaviour : IGameObjectBehaviour<Territory>
    {
        IEnumerable<INaturalDistrictBehaviour> NaturalDistricts { get; }
        // IDictionary<WorldDirection, ITerritoryBehaviour> ConnectedTerritories { get; }
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        event TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;
        // event TerritoryConnectionDelegate OnTerritoryConnectionAdded;
        // event TerritoryConnectionDelegate OnTerritoryConnectionRemoved;
        void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict);
        // void AddTerritoryConnection(ITerritoryBehaviour connection, WorldDirection connectDirection);
        // void RemoveTerritoryConnection(ITerritoryBehaviour connection);
        public delegate void TerritoryBehaviourDelegate(ITerritoryBehaviour territoryBehaviour);
        public delegate void TerritoryNaturalDistrictDelegate(ITerritoryBehaviour territory,
            INaturalDistrictBehaviour naturalDistrict);
        // public delegate void TerritoryConnectionDelegate(ITerritoryBehaviour territoryBehaviour, ITerritoryBehaviour connection, WorldDirection connectDirection);
    }
}
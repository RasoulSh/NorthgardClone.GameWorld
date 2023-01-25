using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Mediation.Commands;
using UnityEngine;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class TerritoryBehaviour : GameObjectBehaviour<Territory>, ITerritoryBehaviour
    {
        private List<TerritoryBehaviour> _connectedTerritories;
        private List<NaturalDistrictBehaviour> _naturalDistricts;
        private List<NaturalDistrictBehaviour> naturalDistricts =>
            _naturalDistricts ??= new List<NaturalDistrictBehaviour>();
        private List<TerritoryBehaviour> connectedTerritories =>
            _connectedTerritories ??= new List<TerritoryBehaviour>();
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistricts => naturalDistricts;
        public IEnumerable<ITerritoryBehaviour> ConnectedTerritories => connectedTerritories;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;
        public event ITerritoryBehaviour.TerritoryConnectionDelegate OnTerritoryConnectionAdded;
        public event ITerritoryBehaviour.TerritoryConnectionDelegate OnTerritoryConnectionRemoved;

        public new void Destroy()
        {
            foreach (var naturalDistrict in naturalDistricts)
            {
                naturalDistrict.Destroy();
            }
            base.Destroy();
        }

        public void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict) =>
            AddNaturalDistrict(naturalDistrict, false);
        
        private void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict, bool ignoreNotify)
        {
            naturalDistricts.Add(naturalDistrict as NaturalDistrictBehaviour);
            if (ignoreNotify == false)
            {
                UpdateNaturalDistricts();
                OnNaturalDistrictAdded?.Invoke(this, naturalDistrict);   
            }
        }
        
        public void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict)
        {
            naturalDistricts.Remove(naturalDistrict as NaturalDistrictBehaviour);
            UpdateNaturalDistricts();
            OnNaturalDistrictRemoved?.Invoke(this, naturalDistrict);
        }

        public void AddTerritoryConnection(ITerritoryBehaviour connection) => AddTerritoryConnection(connection, false);
        
        private void AddTerritoryConnection(ITerritoryBehaviour connection, bool ignoreNotify)
        {
            connectedTerritories.Add(connection as TerritoryBehaviour);
            if (ignoreNotify == false)
            {
                UpdateConnectedTerritories();
                OnTerritoryConnectionAdded?.Invoke(this, connection);   
            }
        }

        public void RemoveTerritoryConnection(ITerritoryBehaviour connection)
        {
            connectedTerritories.Remove(connection as TerritoryBehaviour);
            UpdateConnectedTerritories();
            OnTerritoryConnectionRemoved?.Invoke(this, connection);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateNaturalDistricts();
            UpdateConnectedTerritories();
        }

        private void UpdateNaturalDistricts()
        {
            #if UNITY_EDITOR
            if (Data == null)
            {
                return;
            }
            #endif
            Data.naturalDistricts = naturalDistricts.Select(district => district != null ? district.Data.id : null).ToList();
        }
        
        private void UpdateConnectedTerritories()
        {
#if UNITY_EDITOR
            if (Data == null)
            {
                return;
            }
#endif
            Data.connectedTerritories = connectedTerritories.Select(t => t != null ? t.Data.id : null).ToList();
        }

        public override void Initialize(Territory initialData)
        {
            if (initialData.isInstance == false)
            {
                return;
            }
            base.Initialize(initialData);
            foreach (var territoryId in initialData.connectedTerritories)
            {
                var territory =
                    Mediator.Mediator.Send<FindTerritoryMCmd, ITerritoryBehaviour>(new FindTerritoryMCmd(territoryId));
                AddTerritoryConnection(territory, true);
            }

            foreach (var naturalDistrictId in initialData.naturalDistricts)
            {
                var naturalDistrict =
                    Mediator.Mediator.Send<FindNaturalDistrictMCmd, INaturalDistrictBehaviour>(
                        new FindNaturalDistrictMCmd(naturalDistrictId));
                AddNaturalDistrict(naturalDistrict, true);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Mediation.Commands;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class TerritoryBehaviour : GameObjectBehaviour<Territory>, ITerritoryBehaviour
    {
        [Inject] private ILogger _logger;
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
            if (naturalDistrict == null)
            {
                _logger.LogError("You are trying to add a null natural district to this territory", this);
                return;
            }
            if (naturalDistricts.Contains(naturalDistrict as NaturalDistrictBehaviour))
            {
                _logger.LogWarning("The natural district has been added to this territory already", this);
                return;
            }
            naturalDistricts.Add(naturalDistrict as NaturalDistrictBehaviour);
            if (ignoreNotify == false)
            {
                UpdateNaturalDistricts();
                OnNaturalDistrictAdded?.Invoke(this, naturalDistrict);   
            }
        }
        
        public void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict)
        {
            if (naturalDistrict == null)
            {
                _logger.LogError("You are trying to remove a null natural district", this);
                return;
            }

            if (naturalDistricts.Contains(naturalDistrict as NaturalDistrictBehaviour) == false)
            {
                _logger.LogWarning("The natural district you are trying to remove doesn't include in this territory", this);
                return;
            }
            naturalDistricts.Remove(naturalDistrict as NaturalDistrictBehaviour);
            UpdateNaturalDistricts();
            OnNaturalDistrictRemoved?.Invoke(this, naturalDistrict);
        }

        public void AddTerritoryConnection(ITerritoryBehaviour connection) => AddTerritoryConnection(connection, false);
        
        private void AddTerritoryConnection(ITerritoryBehaviour connection, bool ignoreNotify)
        {
            if (connection == null)
            {
                _logger.LogError("You are trying to add a null territory connection to this territory", this);
                return;
            }
            if (_connectedTerritories.Contains(connection as TerritoryBehaviour))
            {
                _logger.LogWarning("The territory connection has been added to this territory already", this);
                return;
            }
            connectedTerritories.Add(connection as TerritoryBehaviour);
            if (ignoreNotify == false)
            {
                UpdateConnectedTerritories();
                OnTerritoryConnectionAdded?.Invoke(this, connection);   
            }
        }

        public void RemoveTerritoryConnection(ITerritoryBehaviour connection)
        {
            if (connection == null)
            {
                _logger.LogError("You are trying to remove a null territory connection", this);
                return;
            }

            if (_connectedTerritories.Contains(connection as TerritoryBehaviour) == false)
            {
                _logger.LogWarning("You are trying to remove a territory connection that doesn't exist", this);
                return;
            }
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
            if (initialData == null)
            {
                _logger.LogError("You are trying to initialize a territory using null data", this);
                return;
            }
            if (initialData.isInstance == false)
            {
                _logger.LogError("You are trying to initialize a territory that is not an instance", this);
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
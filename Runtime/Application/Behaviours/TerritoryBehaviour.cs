using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Enums;
using Northgard.GameWorld.Mediation.Commands;
using Northgard.GameWorld.ValueObjects;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class TerritoryBehaviour : GameObjectBehaviour<Territory>, ITerritoryBehaviour
    {
        [Inject] private ILogger _logger;
        private Dictionary<WorldDirection, ITerritoryBehaviour> _connectedTerritories;
        private List<NaturalDistrictBehaviour> _naturalDistricts;
        private List<NaturalDistrictBehaviour> naturalDistricts =>
            _naturalDistricts ??= new List<NaturalDistrictBehaviour>();
        private Dictionary<WorldDirection, ITerritoryBehaviour> connectedTerritories =>
            _connectedTerritories ??= new Dictionary<WorldDirection, ITerritoryBehaviour>();
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistricts => naturalDistricts;
        public IDictionary<WorldDirection, ITerritoryBehaviour> ConnectedTerritories => connectedTerritories;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;

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

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateNaturalDistricts();
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
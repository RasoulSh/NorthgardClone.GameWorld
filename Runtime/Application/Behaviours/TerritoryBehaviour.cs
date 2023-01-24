using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.Core.Common.UnityExtensions.UnityReadOnlyField;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class TerritoryBehaviour : GameObjectBehaviour<Territory>, ITerritoryBehaviour
    {
        [SerializeField] [ReadOnlyField] private List<NaturalDistrictBehaviour> naturalDistricts;
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistricts => naturalDistricts;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;

        public new ITerritoryBehaviour Instantiate() => base.Instantiate() as TerritoryBehaviour;

        public new void Destroy()
        {
            foreach (var naturalDistrict in naturalDistricts)
            {
                naturalDistrict.Destroy();
            }
            base.Destroy();
        }

        public void AddNaturalDistrict(INaturalDistrictBehaviour naturalDistrict)
        {
            naturalDistricts.Add(naturalDistrict as NaturalDistrictBehaviour);
            UpdateNaturalDistricts();
            OnNaturalDistrictAdded?.Invoke(this, naturalDistrict);
        }
        
        public void RemoveNaturalDistrict(INaturalDistrictBehaviour naturalDistrict)
        {
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
            Data.naturalDistricts = naturalDistricts.Select(district => district.Data).ToList();
        }
    }
}
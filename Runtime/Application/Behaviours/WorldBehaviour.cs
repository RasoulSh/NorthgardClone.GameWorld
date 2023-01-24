using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class WorldBehaviour : GameObjectBehaviour<World>, IWorldBehaviour
    {
        [SerializeField] private List<TerritoryBehaviour> territories;
        public IEnumerable<ITerritoryBehaviour> Territories => territories;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        public IWorldBehaviour Instantiate() => base.Instantiate() as WorldBehaviour;

        public new void Destroy()
        {
            foreach (var territory in territories)
            {
                territory.Destroy();
            }
            base.Destroy();
        }

        public void AddTerritory(ITerritoryBehaviour territory)
        {
            territories.Add(territory as TerritoryBehaviour);
            UpdateTerritories();
            OnTerritoryAdded?.Invoke(territory);
        }

        public void RemoveTerritory(ITerritoryBehaviour territory)
        {
            territories.Remove(territory as TerritoryBehaviour);
            UpdateTerritories();
            OnTerritoryRemoved?.Invoke(territory);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateTerritories();
        }

        private void UpdateTerritories()
        {
            if (Data == null)
            {
                return;
            }
            Data.territories = territories.Select(territory => territory.Data).ToList();
        }

        protected override void Initialize(World initialData)
        {
            if (initialData.isInstance)
            {
                base.Initialize(initialData);
            }
        }
    }
}
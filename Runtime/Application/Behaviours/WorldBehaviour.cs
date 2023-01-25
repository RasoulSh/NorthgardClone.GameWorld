using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Mediation.Commands;
using UnityEngine;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class WorldBehaviour : GameObjectBehaviour<World>, IWorldBehaviour
    {
        private List<TerritoryBehaviour> _territories;
        private List<TerritoryBehaviour> territories => _territories ??= new List<TerritoryBehaviour>();
        public IEnumerable<ITerritoryBehaviour> Territories => territories;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        public new IWorldBehaviour Instantiate() => base.Instantiate() as WorldBehaviour;

        public new void Destroy()
        {
            foreach (var territory in territories)
            {
                territory.Destroy();
            }
            base.Destroy();
        }

        public void AddTerritory(ITerritoryBehaviour territory) => AddTerritory(territory, false);

        private void AddTerritory(ITerritoryBehaviour territory, bool ignoreNotify)
        {
            territories.Add(territory as TerritoryBehaviour);
            if (ignoreNotify == false)
            {
                UpdateTerritories();
                OnTerritoryAdded?.Invoke(territory);   
            }
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
            #if UNITY_EDITOR
            if (Data == null)
            {
                return;
            }
            #endif
            Data.territories = territories.Select(territory => territory != null ? Data.id : null).ToList();
        }

        public override void Initialize(World initialData)
        {
            if (initialData.isInstance == false)
            {
                return;
            }
            base.Initialize(initialData);
            foreach (var territoryId in initialData.territories)
            {
                var territory = Mediator.Mediator.Send<FindTerritoryMCmd, ITerritoryBehaviour>(new FindTerritoryMCmd(territoryId));
                AddTerritory(territory, true);
            }
        }
    }
}
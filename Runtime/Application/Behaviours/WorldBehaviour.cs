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
    internal class WorldBehaviour : GameObjectBehaviour<World>, IWorldBehaviour
    {
        [Inject] private ILogger _logger;
        private List<TerritoryBehaviour> _territories;
        private List<TerritoryBehaviour> territories => _territories ??= new List<TerritoryBehaviour>();
        public IEnumerable<ITerritoryBehaviour> Territories => territories;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;

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
            if (territory == null)
            {
                _logger.LogError("You can't add a null territory", this);
                return;
            }
            if (territories.Contains(territory as TerritoryBehaviour))
            {
                _logger.LogWarning("The territory has been added to this world already", this);
                return;
            }
            territories.Add(territory as TerritoryBehaviour);
            if (ignoreNotify == false)
            {
                UpdateTerritories();
                OnTerritoryAdded?.Invoke(territory);   
            }
        }

        public void RemoveTerritory(ITerritoryBehaviour territory)
        {
            if (territory == null)
            {
                _logger.LogError("The territory you want to remove is null", this);
                return;
            }
            if (territories.Contains(territory as TerritoryBehaviour) == false)
            {
                _logger.LogError("The territory you want to remove is not contained in this world", this);
                return;
            }
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
            if (initialData == null)
            {
                _logger.LogError("You are trying to initialize a world using null data", this);
                return;
            }
            if (initialData.isInstance == false)
            {
                _logger.LogWarning("You are trying to initialize a world that is not an instance", this);
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
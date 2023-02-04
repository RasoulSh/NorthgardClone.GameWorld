using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using UnityEngine;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

namespace Northgard.GameWorld.Application
{
    internal class WorldEditorService : MonoBehaviour, IWorldEditorService
    {
        [Inject] private ILogger _logger;
        public event IWorldBehaviour.WorldBehaviourDelegate OnWorldChanged;
        public event IWorldBehaviour.WorldBehaviourDelegate OnWorldPositionChanged;
        public event IWorldBehaviour.WorldBehaviourDelegate OnWorldRotationChanged;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        public event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;

        private IWorldBehaviour _world;

        public IWorldBehaviour World
        {
            get => _world;
            set
            {
                if (value == null)
                {
                    _logger.LogError("You can't set world to null", this);
                    return;
                }
                if (_world != null)
                {
                    UnsubscribeDelegates();
                }
                _world = value;
                OnWorldChanged?.Invoke(_world);
                SubscribeDelegates();
            }
        }

        private void SubscribeDelegates()
        {
            _world.OnPositionChanged += _OnWorldPositionChanged;
            _world.OnRotationChanged += _OnWorldRotationChanged;
            _world.OnTerritoryAdded += _OnTerritoryAdded;
            _world.OnTerritoryRemoved += _OnTerritoryRemoved;
            foreach (var territoryRow in _world.Territories)
            {
                foreach (var territory in territoryRow)
                {
                    if (territory == null)
                    {
                        continue;
                    }
                    territory.OnNaturalDistrictAdded += OnNaturalDistrictAdded;
                    territory.OnNaturalDistrictRemoved += OnNaturalDistrictRemoved;
                }
            }
        }

        private void UnsubscribeDelegates()
        {
            _world.OnPositionChanged -= _OnWorldPositionChanged;
            _world.OnRotationChanged -= _OnWorldRotationChanged;
            _world.OnTerritoryAdded -= _OnTerritoryAdded;
            _world.OnTerritoryRemoved -= _OnTerritoryRemoved;
            foreach (var territoryRow in _world.Territories)
            {
                foreach (var territory in territoryRow)
                {
                    if (territory == null)
                    {
                        continue;
                    }

                    territory.OnNaturalDistrictAdded -= OnNaturalDistrictAdded;
                    territory.OnNaturalDistrictRemoved -= OnNaturalDistrictRemoved;
                }
            }
        }

        private void _OnWorldPositionChanged(IGameObjectBehaviour<World> world)
        {
            OnWorldPositionChanged?.Invoke(world as IWorldBehaviour);
        }
        
        private void _OnWorldRotationChanged(IGameObjectBehaviour<World> world)
        {
            OnWorldRotationChanged?.Invoke(world as IWorldBehaviour);
        }
        
        private void _OnTerritoryAdded(ITerritoryBehaviour territory)
        {
            OnTerritoryAdded?.Invoke(territory);
            territory.OnNaturalDistrictAdded += OnNaturalDistrictAdded;
            territory.OnNaturalDistrictRemoved += OnNaturalDistrictRemoved;
        }
        
        private void _OnTerritoryRemoved(ITerritoryBehaviour territory)
        {
            OnTerritoryRemoved?.Invoke(territory);
            territory.OnNaturalDistrictAdded -= OnNaturalDistrictAdded;
            territory.OnNaturalDistrictRemoved -= OnNaturalDistrictRemoved;
        }
    }
}
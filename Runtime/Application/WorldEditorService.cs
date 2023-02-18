using System.Linq;
using DesignPatterns.AbstractPatterns.StatePattern;
using Northgard.Core.Data;
using Northgard.Core.GameObjectBase;
using Northgard.Enterprise.DataSets;
using Northgard.Enterprise.Entities.WorldEntities;
using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Abstraction.Behaviours;
using UnityEngine;
using Zenject;
using ILogger = Northgard.Core.Infrastructure.Logger.ILogger;

namespace Northgard.GameWorld.Application
{
    internal class WorldEditorService : MonoBehaviour, IWorldEditorService
    {
        [Inject] private ILogger _logger;
        [Inject] private IRepository<WorldDataset> _worldRepository;
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

        public void SaveWorld(string worldName)
        {
            var territories = World.Territories.SelectMany(tArr => tArr.Where(t => t != null)).ToList();
            var worldDataset = new WorldDataset()
            {
                id = worldName,
                world = World.Data,
                territories = territories.Select(t => t.Data).ToList(),
                naturalDistricts = territories.SelectMany(t => t.NaturalDistricts.Select(nd => nd.Data)).ToList()
            };
            _worldRepository.CreateOrUpdate(worldDataset);
            _worldRepository.SaveChanges();
        }

        public WorldDataset LoadWorld(string worldName)
        {
            if (_worldRepository.Exists(worldName) == false)
            {
                _logger.LogError("Couldn't find the world with name : " + worldName, this);
                return null;
            }
            var world = _worldRepository.Read(worldName);
            return world;
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
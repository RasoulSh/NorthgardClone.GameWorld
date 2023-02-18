using System.Collections.Generic;
using System.Linq;
using Mediator;
using Northgard.Enterprise.Entities.WorldEntities;
using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Application.Behaviours;
using Northgard.GameWorld.Configurations;
using Northgard.GameWorld.Mediation.Commands;
using UnityEngine;
using Zenject;
using ILogger = Northgard.Core.Infrastructure.Logger.ILogger;

namespace Northgard.GameWorld.Application
{
    internal class WorldPipelineService : MonoBehaviour, IWorldPipelineService,
        ICommandHandler<FindTerritoryMCmd, ITerritoryBehaviour>,
        ICommandHandler<FindNaturalDistrictMCmd, INaturalDistrictBehaviour>
    {
        [Inject] private DiContainer _container;
        [Inject] private ILogger _logger;
        [SerializeField] private WorldPipelineConfig config;
        [SerializeField] private Transform worldPlace;
        public IEnumerable<World> WorldPrefabs => config.WorldPrefabs.Select(wp => wp.Data);
        public IEnumerable<Territory> TerritoryPrefabs => config.TerritoryPrefabs.Select(wp => wp.Data);
        public IEnumerable<NaturalDistrict> NaturalDistrictPrefabs =>
            config.NaturalDistrictPrefabs.Select(wp => wp.Data);
        public IWorldBehaviour World { get; private set; }
        private World _worldData;
        private Dictionary<Territory, ITerritoryBehaviour> _territories;
        private Dictionary<NaturalDistrict, INaturalDistrictBehaviour> _naturalDistricts;
        public IEnumerable<ITerritoryBehaviour> Territories => _territories.Values;
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistricts => _naturalDistricts.Values;

        private void OnEnable()
        {
            Mediator.Mediator.Subscribe(this);
        }

        private void OnDisable()
        {
            Mediator.Mediator.Unsubscribe(this);
        }
        
        public void SetWorld(World world)
        {
            if (world == null)
            {
                _logger.LogError("You cannot set world to null", this);
                return;
            }
            if (World != null)
            {
                DestroyWorld();
            }
            _territories = new Dictionary<Territory, ITerritoryBehaviour>();
            _naturalDistricts = new Dictionary<NaturalDistrict, INaturalDistrictBehaviour>();
            World = InstantiateWorld(world);
        }

        public void DestroyWorld()
        {
            if (World == null)
            {
                _logger.LogError("There is no world to destroy", this);
                return;
            }
            World.Destroy();
            World = null;
        }

        private IWorldBehaviour InstantiateWorld(World world)
        {
            var worldPrefab = config.FindWorldPrefab(world.prefabId);
            var worldInstance = worldPrefab.Instantiate();
            _worldData = world.isInstance ? world : worldInstance.Data;
            return worldInstance as IWorldBehaviour;
        }

        public ITerritoryBehaviour InstantiateTerritory(Territory territory)
        {
            if (territory == null)
            {
                _logger.LogError("The territory you want to instantiate in null", this);
                return null;
            }
            var territoryPrefab = config.FindTerritoryPrefab(territory.prefabId);
            var territoryInstance = territoryPrefab.Instantiate();
            _territories.Add(territory.isInstance ? territory : territoryInstance.Data, territoryInstance as ITerritoryBehaviour);
            return territoryInstance as ITerritoryBehaviour;
        }

        public INaturalDistrictBehaviour InstantiateNaturalDistrict(NaturalDistrict naturalDistrict)
        {
            if (naturalDistrict == null)
            {
                _logger.LogError("The natural district you want to instantiate in null", this);
                return null;
            }
            var naturalDistrictPrefab = config.FindNaturalDistrictPrefab(naturalDistrict.prefabId);
            var naturalDistrictInstance = naturalDistrictPrefab.Instantiate();
            _naturalDistricts.Add(naturalDistrict.isInstance ? naturalDistrict : naturalDistrictInstance.Data, naturalDistrictInstance as INaturalDistrictBehaviour);
            return naturalDistrictInstance as INaturalDistrictBehaviour;
        }

        public void DestroyTerritory(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("The territory id is not valid", this);
                return;
            }
            var territory = FindTerritory(id);
            if (territory == null)
            {
                _logger.LogError("Couldn't find territory with id : " + id, this);
                return;
            }

            var territoryPoint = territory.Data.pointInWorld;
            var ndIds = territory.Data.naturalDistricts;
            var ndsToRemove = _naturalDistricts.Keys.Where(nd => ndIds.Contains(nd.id)).ToList();
            foreach (var naturalDistrict in ndsToRemove)
            {
                _naturalDistricts.Remove(naturalDistrict);
            }
            _territories.Remove(territory.Data);
            territory.Destroy();
            World.RemoveTerritory(territoryPoint);
        }
        
        public void DestroyNaturalDistrict(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("The natural district id is not valid", this);
                return;
            }
            var naturalDistrict = FindNaturalDistrict(id);
            if (naturalDistrict == null)
            {
                _logger.LogError("Couldn't find natural district with id : " + id, this);
                return;
            }
            _naturalDistricts.Remove(naturalDistrict.Data);
            naturalDistrict.Destroy();
        }

        public void Initialize()
        {
            foreach (var naturalDistrict in _naturalDistricts)
            {
                (naturalDistrict.Value as NaturalDistrictBehaviour).Initialize(naturalDistrict.Key);
            }
            foreach (var territory in _territories)
            {
                (territory.Value as TerritoryBehaviour).Initialize(territory.Key);
            }
            (World as WorldBehaviour).Initialize(_worldData);
        }

        public ITerritoryBehaviour FindTerritory(string territoryId)
        {
            return _territories.FirstOrDefault(t => t.Key.id == territoryId).Value;
        }

        public INaturalDistrictBehaviour FindNaturalDistrict(string naturalDistrictId)
        {
            return _naturalDistricts.FirstOrDefault(nd => nd.Key.id == naturalDistrictId).Value;
        }

        public GameObject GenerateFakeNaturalDistrict(string prefabId)
        {
            var prefab = config.NaturalDistrictPrefabs.First(ndp => ndp.Data.prefabId == prefabId);
            return prefab.CloneFakeInstance();
        }

        public ITerritoryBehaviour Handle(FindTerritoryMCmd data)
        {
            return FindTerritory(data.TerritoryId);
        }

        public INaturalDistrictBehaviour Handle(FindNaturalDistrictMCmd data)
        {
            return FindNaturalDistrict(data.NaturalDistrictId);
        }
    }
}
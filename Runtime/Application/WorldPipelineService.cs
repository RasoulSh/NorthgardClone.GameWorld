using System.Collections.Generic;
using System.Linq;
using Mediator;
using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Application.Behaviours;
using Northgard.GameWorld.Configurations;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Mediation.Commands;
using UnityEngine;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

namespace Northgard.GameWorld.Application
{
    internal class WorldPipelineService : MonoBehaviour, IWorldPipelineService,
        ICommandHandler<FindTerritoryMCmd, ITerritoryBehaviour>,
        ICommandHandler<FindNaturalDistrictMCmd, INaturalDistrictBehaviour>
    {
        [Inject] private ILogger _logger;
        [SerializeField] private WorldPipelineConfig config;
        [SerializeField] private Transform worldPlace;
        public IEnumerable<World> WorldPrefabs => config.WorldPrefabs.Select(wp => wp.Data);
        public IEnumerable<Territory> TerritoryPrefabs => config.TerritoryPrefabs.Select(wp => wp.Data);
        public IEnumerable<NaturalDistrict> NaturalDistrictPrefabs =>
            config.NaturalDistrictPrefabs.Select(wp => wp.Data);
        public IWorldBehaviour World { get; private set; }
        private World worldData;
        private Dictionary<Territory, ITerritoryBehaviour> territores;
        private Dictionary<NaturalDistrict, INaturalDistrictBehaviour> naturalDistricts;
        public IEnumerable<ITerritoryBehaviour> Territories => territores.Values;
        public IEnumerable<INaturalDistrictBehaviour> NaturalDistricts => naturalDistricts.Values;
        
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
            territores = new Dictionary<Territory, ITerritoryBehaviour>();
            naturalDistricts = new Dictionary<NaturalDistrict, INaturalDistrictBehaviour>();
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
            worldData = world.isInstance ? world : worldInstance.Data;
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
            territores.Add(territory.isInstance ? territory : territoryInstance.Data, territoryInstance as ITerritoryBehaviour);
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
            naturalDistricts.Add(naturalDistrict.isInstance ? naturalDistrict : naturalDistrictInstance.Data, naturalDistrictInstance as INaturalDistrictBehaviour);
            return naturalDistrictInstance as INaturalDistrictBehaviour;
        }
        
        public void Initialize()
        {
            foreach (var naturalDistrict in naturalDistricts)
            {
                (naturalDistrict.Value as NaturalDistrictBehaviour).Initialize(naturalDistrict.Key);
            }
            foreach (var territory in territores)
            {
                (territory.Value as TerritoryBehaviour).Initialize(territory.Key);
            }
            (World as WorldBehaviour).Initialize(worldData);
        }

        public ITerritoryBehaviour FindTerritory(string territoryId)
        {
            return territores.FirstOrDefault(t => t.Key.id == territoryId).Value;
        }

        public INaturalDistrictBehaviour FindNaturalDistrict(string naturalDistrictId)
        {
            return naturalDistricts.FirstOrDefault(nd => nd.Key.id == naturalDistrictId).Value;
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
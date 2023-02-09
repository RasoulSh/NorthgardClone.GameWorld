using System.Collections.Generic;
using System.Linq;
using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Northgard.GameWorld.Mediation.Commands;
using Northgard.GameWorld.ValueObjects;
using UnityEngine;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class WorldBehaviour : GameObjectBehaviour<World>, IWorldBehaviour
    {
        [Inject] private ILogger _logger;
        private ITerritoryBehaviour[][] _territories;

        private ITerritoryBehaviour[][] territories
        {
            get
            {
                if (_territories != null)
                {
                    return _territories;
                }

                _territories = new ITerritoryBehaviour[Data.size.x][];
                for (int x = 0; x < Data.size.x; x++)
                {
                    _territories[x] = new ITerritoryBehaviour[Data.size.y];
                }
                return _territories;
            }
        }
        public ITerritoryBehaviour[][] Territories => territories;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        public event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;

        public new void Destroy()
        {
            foreach (var territoryRow in territories)
            {
                foreach (var territory in territoryRow)
                {
                    if (territory == null)
                    {
                        continue;
                    }
                    territory.Destroy();   
                }
            }
            base.Destroy();
        }

        public void AddTerritory(ITerritoryBehaviour territory, Vector2Int pointInWorld) => AddTerritory(territory, pointInWorld, false);

        private void AddTerritory(ITerritoryBehaviour territory, Vector2Int pointInWorld, bool ignoreNotify)
        {
            if (territory == null)
            {
                _logger.LogError("You can't add a null territory", this);
                return;
            }
            if (TerritoriesContains(territory))
            {
                _logger.LogWarning("The territory has been added to this world already", this);
                return;
            }
            if (territories[pointInWorld.x][pointInWorld.y] != null)
            {
                _logger.LogError("There is already a territory in this point", this);
                return;
            }

            territory.Data.pointInWorld = pointInWorld;
            territories[pointInWorld.x][pointInWorld.y] = territory;
            if (ignoreNotify == false)
            {
                UpdateTerritories();
                OnTerritoryAdded?.Invoke(territory);   
            }
        }

        private bool TerritoriesContains(ITerritoryBehaviour territoryBehaviour)
        {
            foreach (var territoryRow in territories)
            {
                if (territoryRow.Contains(territoryBehaviour))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveTerritory(Vector2Int pointInWorld)
        {
            var territory = territories[pointInWorld.x][pointInWorld.y];
            if (territory == null)
            {
                _logger.LogError("There is no territory in this point", this);
                return;
            }

            territories[pointInWorld.x][pointInWorld.y] = null;
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
            Data.territories = new List<IdPointPair>();
            for (int x = 0; x < Data.size.x; x++)
            {
                for (int y = 0; y < Data.size.y; y++)
                {
                    var territory = territories[x][y];
                    Data.territories.Add(new IdPointPair(){id = territory?.Data.id, point = new Vector2(x, y)});
                }
            }
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
            for (int x = 0; x < Data.size.x; x++)
            {
                for (int y = 0; y < Data.size.y; y++)
                {
                    var point = new Vector2(x, y);
                    var territoryId = initialData.territories.Find(t =>  t.point == point).id;
                    if (territoryId != null)
                    {
                        var territory = Mediator.Mediator.Send<FindTerritoryMCmd, ITerritoryBehaviour>(new FindTerritoryMCmd(territoryId));
                        territories[x][y] = territory;
                    }
                }
            }
        }
    }
}
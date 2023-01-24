using System.Collections.Generic;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Abstraction
{
    public interface IWorldPipelineService
    {
        IEnumerable<World> WorldPrefabs { get; }
        public IWorldBehaviour World { get; }
        void SetWorld(string worldId);
        void DestroyWorld();
    }
}
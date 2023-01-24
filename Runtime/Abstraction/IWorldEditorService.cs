using Northgard.GameWorld.Abstraction.Behaviours;

namespace Northgard.GameWorld.Abstraction
{
    public interface IWorldEditorService
    {
        IWorldBehaviour World { get; set; }
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldChanged;
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldPositionChanged;
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldRotationChanged;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;
        void NewWorld();
    }
}
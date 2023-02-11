using Northgard.Enterprise.DataSets;
using Northgard.GameWorld.Abstraction.Behaviours;

namespace Northgard.GameWorld.Abstraction
{
    public interface IWorldEditorService
    {
        IWorldBehaviour World { get; set; }
        void SaveWorld(string worldName);
        WorldDataset LoadWorld(string worldName);
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldChanged;
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldPositionChanged;
        event IWorldBehaviour.WorldBehaviourDelegate OnWorldRotationChanged;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryAdded;
        event ITerritoryBehaviour.TerritoryBehaviourDelegate OnTerritoryRemoved;
        event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictAdded;
        event ITerritoryBehaviour.TerritoryNaturalDistrictDelegate OnNaturalDistrictRemoved;
    }
}
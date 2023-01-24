using Northgard.GameWorld.Abstraction;
using Northgard.GameWorld.Application;
using UnityEngine;
using Zenject;

namespace Northgard.GameWorld
{
    [RequireComponent(typeof(WorldPipelineService))]
    [RequireComponent(typeof(WorldEditorService))]
    public class GameWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWorldEditorService>().To<WorldEditorService>()
                .FromInstance(GetComponent<WorldEditorService>()).AsSingle();
            Container.Bind<IWorldPipelineService>().To<WorldPipelineService>()
                .FromInstance(GetComponent<WorldPipelineService>()).AsSingle();
        }
    }
}
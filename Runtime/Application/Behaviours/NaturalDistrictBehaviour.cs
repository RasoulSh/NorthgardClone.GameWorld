using Northgard.Core.GameObjectBase;
using Northgard.Enterprise.Entities.WorldEntities;
using Northgard.GameWorld.Abstraction.Behaviours;
using Zenject;
using ILogger = Northgard.Core.Infrastructure.Logger.ILogger;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class NaturalDistrictBehaviour : GameObjectBehaviour<NaturalDistrict>, INaturalDistrictBehaviour
    {
        [Inject] private ILogger _logger;
        public override void Initialize(NaturalDistrict initialData)
        {
            if (initialData == null)
            {
                _logger.LogError("You are trying to initialize a natural district using null data", this);
                return;
            }
            if (initialData.isInstance == false)
            {
                _logger.LogError("You are trying to initialize a natural district that is not an instance", this);
                return;
            }
            base.Initialize(initialData);
        }
    }
}
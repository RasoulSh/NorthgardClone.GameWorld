using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;
using Zenject;
using ILogger = Northgard.Core.Abstraction.Logger.ILogger;

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
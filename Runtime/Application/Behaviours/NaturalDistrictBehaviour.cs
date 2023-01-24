using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class NaturalDistrictBehaviour : GameObjectBehaviour<NaturalDistrict>, INaturalDistrictBehaviour
    {
        protected override void Initialize(NaturalDistrict initialData)
        {
            if (initialData.isInstance)
            {
                base.Initialize(initialData);
            }
        }
    }
}
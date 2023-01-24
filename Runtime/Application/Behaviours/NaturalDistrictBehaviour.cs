using Northgard.Core.Application.Behaviours;
using Northgard.GameWorld.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Application.Behaviours
{
    internal class NaturalDistrictBehaviour : GameObjectBehaviour<NaturalDistrict>, INaturalDistrictBehaviour
    {
        public new INaturalDistrictBehaviour Instantiate() => base.Instantiate() as INaturalDistrictBehaviour;
    }
}
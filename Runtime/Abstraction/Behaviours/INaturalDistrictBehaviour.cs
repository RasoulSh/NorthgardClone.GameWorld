using Northgard.Core.Abstraction.Behaviours;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface INaturalDistrictBehaviour : IGameObjectBehaviour<NaturalDistrict>
    {
        new INaturalDistrictBehaviour Instantiate();
        public delegate void NaturalDistrictDelegate(INaturalDistrictBehaviour naturalDistrictBehaviour);
    }
}
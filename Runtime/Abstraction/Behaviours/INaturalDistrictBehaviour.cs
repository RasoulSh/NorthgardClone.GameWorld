using Northgard.Core.GameObjectBase;
using Northgard.Enterprise.Entities.WorldEntities;

namespace Northgard.GameWorld.Abstraction.Behaviours
{
    public interface INaturalDistrictBehaviour : IGameObjectBehaviour<NaturalDistrict>
    {
        public delegate void NaturalDistrictDelegate(INaturalDistrictBehaviour naturalDistrictBehaviour);
    }
}
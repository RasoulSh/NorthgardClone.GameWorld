using System;
using Northgard.Core.Entities;
using Northgard.GameWorld.Enums;

namespace Northgard.GameWorld.Entities
{
    [Serializable]
    public class NaturalDistrict : GameObjectEntity
    {
        public NaturalResources naturalResource;

        public delegate void NaturalDistrictDelegate(NaturalDistrict naturalDistrict);
    }
}
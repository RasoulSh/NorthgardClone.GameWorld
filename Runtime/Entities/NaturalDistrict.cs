using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Northgard.Core.Entities;
using Northgard.GameWorld.Enums;

namespace Northgard.GameWorld.Entities
{
    [Serializable]
    public class NaturalDistrict : GameObjectEntity
    {
        [JsonIgnore][XmlIgnore] public string PrefabId => prefabId;
        [JsonIgnore][XmlIgnore] public string Title => title;
        public NaturalResources naturalResource;

        public delegate void NaturalDistrictDelegate(NaturalDistrict naturalDistrict);
    }
}
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Northgard.Core.Common.UnityExtensions.UnityReadOnlyField;
using Northgard.Core.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Entities
{
 
    [Serializable]
    public class Territory : GameObjectEntity
    {
        [JsonIgnore][XmlIgnore] public string PrefabId => prefabId;
        [JsonIgnore][XmlIgnore] public string Title => title;
        public bool isCoast;
        public int buildingCapacity;
        [HideInInspector] public List<string> naturalDistricts;
        [HideInInspector] public List<string> connectedTerritories;
        [ReadOnlyField] public bool isDiscovered;

        public delegate void TerritoryDelegate(Territory territory);
        public delegate void TerritoryIdDelegate(string territoryId);
    }
}
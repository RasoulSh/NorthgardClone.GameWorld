using System;
using System.Collections.Generic;
using Northgard.Core.Common.UnityExtensions.UnityReadOnlyField;
using Northgard.Core.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Entities
{
 
    [Serializable]
    public class Territory : GameObjectEntity
    {
        public bool isCoast;
        public int buildingCapacity;
        [HideInInspector] public List<NaturalDistrict> naturalDistricts;
        [ReadOnlyField] public bool isDiscovered;

        public delegate void TerritoryDelegate(Territory territory);
        public delegate void TerritoryIdDelegate(string territoryId);
    }
}
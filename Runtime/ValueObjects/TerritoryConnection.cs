using System;
using Northgard.GameWorld.Enums;

namespace Northgard.GameWorld.ValueObjects
{
    [Serializable]
    public class TerritoryConnection
    {
        public string territoryId;
        public WorldDirection connectionDirection;
    }
}
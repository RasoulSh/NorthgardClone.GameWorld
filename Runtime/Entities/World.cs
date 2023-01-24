using System;
using System.Collections.Generic;
using Northgard.Core.Entities;
using UnityEngine;

namespace Northgard.GameWorld.Entities
{
    [Serializable]
    public class World : GameObjectEntity
    {
        public string title;
        [HideInInspector] public List<Territory> territories;

        public delegate void WorldDelegate(World world);
        public delegate void WorldIdDelegate(string worldId);
        public delegate void WorldBoundsDelegate(Bounds worldBounds);
    }   
}
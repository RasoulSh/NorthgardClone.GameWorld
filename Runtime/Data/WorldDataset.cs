using System;
using System.Collections.Generic;
using Northgard.GameWorld.Entities;

namespace Northgard.GameWorld.Data
{
    [Serializable]
    public class WorldDataset
    {
        public World world;
        public List<Territory> territories;
        public List<NaturalDistrict> naturalDistricts;
    }
}
using System.Collections.Generic;
using System.Linq;

namespace TGJ2021.InGame.Rocks
{
    public class BreakRockCounter
    {
        private List<RockMeta> _brokeRockMetas = new List<RockMeta>();

        public void AddBreakRock(RockMeta meta)
        {
            _brokeRockMetas.Add(meta);
        }

        public int GetBrokeRockCount(RockSize size, RockType type)
        {
            return _brokeRockMetas.Count(m => m.Size == size && m.Type == type);
        }
    }
}
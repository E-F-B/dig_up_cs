using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace TGJ2021.InGame.Rocks
{
    public class RockCounter
    {
        private readonly List<RockMeta> _rareRocks = new List<RockMeta>();

        private readonly List<RockMeta> _rockMetas = new List<RockMeta>();

        private int _baseLine = 60;
        private float _baseRareRate = 0.2F;

        private RockType _rockType;

        public RockCounter(RockType rockType)
        {
            _rockMetas.Add(new RockMeta(RockSize.Small, RockType.Spread));
            
            // レア岩の数は固定である

            for (int i = 0; i < 100; i++)
            {
                var smallRare = new RockMeta(RockSize.Small, RockType.Rare);
                _rareRocks.Add(smallRare);
            }
            
            for (int i = 0; i < 20; i++)
            {
                var smallRare = new RockMeta(RockSize.Middle, RockType.Rare);
                _rareRocks.Add(smallRare);
            }
            
            for (int i = 0; i < 1; i++)
            {
                var smallRare = new RockMeta(RockSize.Large, RockType.Rare);
                _rareRocks.Add(smallRare);
            }
            
            _rockType = rockType;
        }

        public RockMeta Build()
        {
            var size = LotteryRockSize();
            var meta = new RockMeta(size, LotteryRockType(size));
            _rockMetas.Add(meta);
            return meta;
        }

        private RockType LotteryRockType(RockSize size)
        {
            //float currentRate = _baseRareRate * (_baseLine / _rockMetas.Count);
            float currentRate = _baseRareRate;
            // レア当選
            if (Random.Range(0F, 1F) < currentRate)
            {
                // 所定のサイズのレアが枯渇してるなら取れない
                var rareIndex = _rareRocks.FindIndex(n => n.Size == size);
                if (rareIndex != -1)
                {
                    _rareRocks.RemoveAt(rareIndex);
                    return RockType.Rare;
                }
                Debug.Log("枯渇！");
            }

            return _rockType;
        }

        private RockSize LotteryRockSize()
        {
            var rand = Random.Range(0, 100);
            if (rand < 1)
            {
                return RockSize.Large;
            }
            else if (rand < 30)
            {
                return RockSize.Middle;
            }
            else
            {
                return RockSize.Small;
            }
        }
    }
}
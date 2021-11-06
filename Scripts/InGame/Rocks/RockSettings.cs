using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TGJ2021.InGame.ShotStrategy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TGJ2021.InGame.Rocks
{
    [Serializable]
    public class Lottery
    {
        [InfoBox("抽選成功確率")]
        [Range(0, 100)] 
        public int rate;

        public int score;

        public bool IsElected() => Random.Range(0, 100) < rate;
    }
    
    [Serializable]
    public class RockMap
    {
        public RockSize Size;
        public RockType Type;
        public RockBehaviour Prefab;
        public int Life;
        public List<Lottery> LotteryScore;
        public DanmakuSetting DanmakuSetting;

        public int GetScore()
        {
            var lottely = LotteryScore.OrderBy(l => l.rate)
                .FirstOrDefault(l => l.IsElected());

            return lottely?.score ?? 0;
        }
    }
    
    
    [CreateAssetMenu]
    public class RockSettings : ScriptableObject
    {
        [SerializeField] private List<RockMap> _rockMaps;

        public RockMap FindRock(RockSize size, RockType type)
        {
            return _rockMaps.FirstOrDefault(m => m.Size == size && m.Type == type);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using UnityEngine;
using Random = UnityEngine.Random;


namespace TGJ2021.InGame.Rocks
{
    public class RockSpawner : IDisposable
    {
        private readonly List<RockSpawnPoint> _spawnPositions = new List<RockSpawnPoint>();
        private readonly RockFactory _rockFactory;
        private readonly ISubscriber<RockBreakMessage> _subscriber;
        private readonly BreakRockCounter _breakRockCounter;
        private readonly Dictionary<int, RockCounter> _rockCounters = new Dictionary<int, RockCounter>();

        private int RockSizeCache { get; }
        private int RockTypeCache { get; }
        
        public RockSpawner(Transform spawnPositions, RockFactory rockFactory, ISubscriber<RockBreakMessage> rockBreakSubscriber, BreakRockCounter rockCounter)
        {
            foreach (var spawnPosition in spawnPositions.GetComponentsInChildren<RockSpawnPoint>())
            {
                _spawnPositions.Add(spawnPosition);
            }

            _rockFactory = rockFactory;
            _subscriber = rockBreakSubscriber;
            _breakRockCounter = rockCounter;
            
            _subscriber.Subscribe((message =>
            {
                _breakRockCounter.AddBreakRock(message.Meta);
                SpawnNearPoint(message.BreakPosition);
            }));
            
            RockSizeCache = Enum.GetValues(typeof(RockSize)).Length;
            RockTypeCache = Enum.GetValues(typeof(RockType)).Length;
        }

        private void SpawnNearPoint(Vector3 breakPosition)
        {
            var sorted = _spawnPositions.OrderBy(t => Vector3.Distance(t.transform.position, breakPosition)).First();
            var rockMeta = sorted.Build();
            _rockFactory.Create(rockMeta, sorted.transform.position);
        }

        public async UniTask InitializeSpawnAsync(int loopCount)
        {
            // 6:3:1でやりたい
            int rate = loopCount / 10;
            
            for (int i = 0; i < loopCount; i++)
            {
                foreach (var spawnPosition in _spawnPositions)
                {
                    RockMeta meta;
                    if (i < rate * 8)
                    {
                        meta = new RockMeta(RockSize.Small, (RockType)Random.Range(0, 2));
                    }
                    else
                    {
                        meta = new RockMeta(RockSize.Middle, (RockType)Random.Range(0, 2));
                    }
                    _rockFactory.Create(meta, spawnPosition.transform.position);
                }

                await UniTask.Delay(100);
            }
        }

        public void Dispose()
        {
        }
    }
}
using System;
using UnityEngine;

namespace TGJ2021.InGame.Rocks
{
    public class RockSpawnPoint : MonoBehaviour
    {
        [SerializeField] private RockType _rockType;
        
        
        private RockCounter _rockCounter;

        private void Awake()
        {
            _rockCounter = new RockCounter(_rockType);
        }

        public RockMeta Build()
        {
            return _rockCounter.Build();
        }
    }
}
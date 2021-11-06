using System.Collections.Generic;
using UnityEngine;

namespace TGJ2021
{
    public class BulletCounter
    {
        private List<IBullet> _bullets = new List<IBullet>();

        public IBullet this[int i]
        {
            get
            {
                return _bullets[i];
            }
        }

        public float ScoreRate => Mathf.Clamp(1F + Count / 200F, 1F, 2F);

        public int Count => _bullets.Count;
        
        public void AddBullet(IBullet bullet)
        {
            _bullets.Add(bullet);
        }

        public void RemoveBullet(IBullet bullet)
        {
            _bullets.Remove(bullet);
        }
        
        public void ForceBanishAll()
        {
            // 強制的にすべての弾を消す
            for (var i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].ForceBanish();
            }
            _bullets.Clear();            
        }
    }
}
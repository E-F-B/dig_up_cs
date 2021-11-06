using Otoshiai.Utility;
using UnityEngine;

namespace TGJ2021.InGame.ShotStrategy
{
    public class SpreadShot : IDanmaku
    {
        private readonly IBulletFactory _bulletFactory;
        private readonly SpreadShotMeta _spreadShotMeta;

        public SpreadShot(IBulletFactory bulletFactory, SpreadShotMeta shotMeta)
        {
            _bulletFactory = bulletFactory;
            _spreadShotMeta = shotMeta;
        }

        public void Emit(Vector3 spawnPosition, Vector3 targetPosition, bool isBroke)
        {
            var dir = BulletMath.EulerAngle(spawnPosition, targetPosition);
            var count = _spreadShotMeta.BulletCount.Random();
            if (isBroke)
            {
                count /= 2;
            }
            var range = _spreadShotMeta.ShotEulerAngle / 2;
            for (int i = 0; i < count; i++)
            {
                int fix = Random.Range(-range, range);

                var moveParameter = new MoveParameter(dir + fix, _spreadShotMeta.BulletSpeed.Random(),
                    _spreadShotMeta.ShotPower, 1);
                _bulletFactory.Create(_spreadShotMeta.Meta, moveParameter, spawnPosition);
            }
        }
    }

    public readonly struct SpreadShotMeta
    {
        public readonly IntRange BulletCount;
        public readonly FloatRange BulletSpeed;

        /// <summary>
        /// 弾のメタ情報
        /// </summary>
        public readonly BulletMeta Meta;

        /// <summary>
        /// 角度の上限
        /// </summary>
        public readonly int ShotEulerAngle;

        /// <summary>
        /// 弾の火力
        /// </summary>
        public readonly int ShotPower;

        public SpreadShotMeta(IntRange bulletCount, FloatRange bulletSpeed, BulletMeta meta, int shotEulerAngle, int shotPower)
        {
            BulletCount = bulletCount;
            BulletSpeed = bulletSpeed;
            Meta = meta;
            ShotEulerAngle = shotEulerAngle;
            ShotPower = shotPower;
        }
    }
}
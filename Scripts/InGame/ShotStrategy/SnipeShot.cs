using Otoshiai.Utility;
using UnityEngine;

namespace TGJ2021.InGame.ShotStrategy
{
    public class SnipeShot : IDanmaku
    {
        private readonly IBulletFactory _bulletFactory;
        private readonly SnipeShotMeta _spreadShotMeta;

        public SnipeShot(IBulletFactory bulletFactory, SnipeShotMeta shotMeta)
        {
            _bulletFactory = bulletFactory;
            _spreadShotMeta = shotMeta;
        }

        public void Emit(Vector3 spawnPosition, Vector3 targetPosition, bool isBroke)
        {
            var target = BulletMath.EulerAngle(spawnPosition, targetPosition);

            int count = _spreadShotMeta.BulletCount.Random();
            if (isBroke)
            {
                count /= 2;
            }
            int interval = _spreadShotMeta.ShotAngleInterval;
            int centerIndex = (count - 1) / 2;
            float speed = _spreadShotMeta.BulletSpeed.Random();
            for (int i = -centerIndex; i <= centerIndex; i++)
            {
                var moveParameter = new MoveParameter(target + (interval * i), speed, _spreadShotMeta.ShotPower, 1);
                _bulletFactory.Create(_spreadShotMeta.Meta, moveParameter, spawnPosition);
            }
        }
    }

    public readonly struct SnipeShotMeta
    {
        public readonly IntRange BulletCount;
        public readonly FloatRange BulletSpeed;

        /// <summary>
        /// 弾のメタ情報
        /// </summary>
        public readonly BulletMeta Meta;

        /// <summary>
        /// 弾の感覚
        /// </summary>
        public readonly int ShotAngleInterval;

        /// <summary>
        /// 弾の火力
        /// </summary>
        public readonly int ShotPower;

        public SnipeShotMeta(IntRange bulletCount, FloatRange bulletSpeed, BulletMeta meta, int shotEulerAngle, int shotPower)
        {
            BulletCount = bulletCount;
            BulletSpeed = bulletSpeed;
            Meta = meta;
            ShotAngleInterval = shotEulerAngle;
            ShotPower = shotPower;
        }
    }
}
using System;

namespace TGJ2021.InGame.ShotStrategy
{
    [Serializable]
    public class DanmakuSetting
    {
        public ShotType ShotType;
        
        public int BulletCountMin, BulletCountMax;
        public float BulletSpeedMin, BulletSpeedMax;

        public BulletType Type;
        public BulletUser User;
        public int Color;
        public bool Random;

        /// <summary>
        /// 弾の感覚
        /// </summary>
        public int ShotAngleInterval;

        /// <summary>
        /// 弾の火力
        /// </summary>
        public int ShotPower;

        public SnipeShotMeta BuildSnipeShotMeta()
        {
            var bulletColor = Random ? BulletColor.RandomColor : new BulletColor(Color);
            var bulletMeta = new BulletMeta(Type, User, bulletColor);
            return new SnipeShotMeta(new IntRange(BulletCountMin, BulletCountMax),
                new FloatRange(BulletSpeedMin, BulletSpeedMax), bulletMeta, ShotAngleInterval, ShotPower);
        }
        
        public SpreadShotMeta BuildSpreadShotMeta()
        {
            var bulletColor = Random ? BulletColor.RandomColor : new BulletColor(Color);
            var bulletMeta = new BulletMeta(Type, User, bulletColor);
            return new SpreadShotMeta(new IntRange(BulletCountMin, BulletCountMax),
                new FloatRange(BulletSpeedMin, BulletSpeedMax), bulletMeta, ShotAngleInterval, ShotPower);
        }
    }
}
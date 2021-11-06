using UnityEngine;

namespace TGJ2021
{
    public enum BulletType
    {
        Circle_SS,
        Circle_S,
        Circle_M,
        Diamond_SS,
        Diamond_S,
        Diamond_M,
        Marisa_Shot_Left,
        Marisa_Shot_Right,
        BigWave
    }

    public readonly struct BulletMeta
    {
        public readonly BulletType Type;
        public readonly BulletUser User;
        public readonly BulletColor Color;

        public BulletMeta(BulletType bulletType, BulletUser user, BulletColor color)
        {
            Type = bulletType;
            User = user;
            Color = color;
        }
    }

    public readonly struct BulletColor
    {
        public const int RANDOM = 100;
        
        public readonly int color;

        public BulletColor(int color = 0)
        {
            this.color = color;
        }

        public static BulletColor RandomColor => new BulletColor(RANDOM);
    }
}
using Otoshiai.Utility;
using UnityEngine;

namespace TGJ2021
{
    public readonly struct MoveParameter
    {
        public readonly float Direction;
        public readonly float Speed;
        public readonly int Power;
        public readonly int BreakPoint;

        public readonly Vector3 Velocity;

        public MoveParameter(float direction, float speed, int power, int breakPoint)
        {
            Direction = direction;
            Speed = speed;
            Power = power;
            BreakPoint = breakPoint;
            Velocity = BulletMath.Forward(direction) * speed;
        }
    }
}
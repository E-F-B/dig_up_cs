using UnityEngine;

namespace TGJ2021.InGame.ShotStrategy
{
    public interface IDanmaku
    {
        void Emit(Vector3 spawnPosition, Vector3 targetPosition, bool isBroke);
    }
}
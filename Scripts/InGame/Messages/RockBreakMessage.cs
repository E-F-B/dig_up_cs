using TGJ2021.InGame.Rocks;
using TGJ2021.InGame.Scores;
using UnityEngine;

namespace TGJ2021.InGame.Messages
{
    public class RockBreakMessage
    {
        public RockMeta Meta;
        public Vector3 BreakPosition;
        public int ScorePoint;
    }
}
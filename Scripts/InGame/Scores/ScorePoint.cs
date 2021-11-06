using System;
using Random = UnityEngine.Random;

namespace TGJ2021.InGame.Scores
{
    [Serializable]
    public class ScorePoint
    {
        public int minPoint;
        public int maxPoint;

        private int _scoreCache = -1;

        public int Point
        {
            get
            {
                if (_scoreCache == -1)
                {
                    _scoreCache = Random.Range(minPoint, maxPoint);
                }

                return _scoreCache;
            }
        }
    }

}
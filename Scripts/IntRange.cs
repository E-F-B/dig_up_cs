namespace TGJ2021
{
    public readonly struct IntRange
    {
        public readonly int Min;
        public readonly int Max;

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Random() => UnityEngine.Random.Range(Min, Max);
    }
    
    public readonly struct FloatRange
    {
        public readonly float Min;
        public readonly float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Random() => UnityEngine.Random.Range(Min, Max);
    }
}
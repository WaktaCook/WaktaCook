namespace WaktaCook.Core
{
    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static Range operator *(Range range, float multiplier)
        {
            return new Range(range.min * multiplier, range.max * multiplier);
        }
    }
}
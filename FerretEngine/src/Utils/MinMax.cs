namespace FerretEngine.Utils
{
    /// <summary>
    /// Stores the minimum and maximum values of an evaluated collection.
    /// </summary>
    public class MinMax
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMax()
        {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public void Evaluate(float n)
        {
            if (n < Min) Min = n;
            if (n > Max) Max = n;
        }
    }
}
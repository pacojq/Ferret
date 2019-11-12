using System;
using System.Runtime.CompilerServices;

namespace FerretEngine.Utils
{
    public class FeRandom
    {
        public static int Seed
        {
            get => _seed;
            set
            {
                _rand = new Random(value);
                _seed = value;
            }
        }
        private static int _seed;

        private static Random _rand;




        internal static void Initialize()
        {
            _seed = Environment.TickCount;
            _rand = new Random(_seed);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(float percent)
        {
            return _rand.NextDouble() <= percent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Next()
        {
            return (float) _rand.NextDouble();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextInt()
        {
            return _rand.Next();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Next(float max)
        {
            return (float) _rand.NextDouble() * max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(float min, float max)
        {
            return min + (float) _rand.NextDouble() * (max - min);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RangeInt(int min, int max)
        {
            return min + _rand.Next(max - min);
        }
    }
}
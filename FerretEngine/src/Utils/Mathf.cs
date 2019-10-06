using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace FerretEngine.Utils
{
    public static class Mathf
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PointDirection(Vector2 p0, Vector2 p1)
        {
            return (float) Math.Atan2(p1.Y - p0.Y, p1.X - p0.X);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PointDistance(Vector2 p0, Vector2 p1)
        {
            return Vector2.Distance(p0, p1);
        }
        
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float f)
        {
            return (float) Math.Floor(f);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Floor(Vector2 vec)
        {
            return new Vector2(Mathf.Floor(vec.X), Mathf.Floor(vec.Y));
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
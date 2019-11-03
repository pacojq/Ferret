using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace FerretEngine.Utils
{
    public static class FeMath
    {

        public const float PI = (float) Math.PI;
        
        private const float DEG_TO_RAD = PI / 180f;
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DegToRad(float deg)
        {
            return deg * DEG_TO_RAD;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RadToDeg(float rad)
        {
            return rad / DEG_TO_RAD;
        }
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Direction(Vector2 p0, Vector2 p1)
        {
            return ATan2(p1.Y - p0.Y, p1.X - p0.X);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 p0, Vector2 p1)
        {
            return Vector2.Distance(p0, p1);
        }
        
        
        
        
        
        
        
        /// <summary>
        /// Returns the Sin of an angle in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float degrees)
        {
            return (float) Math.Sin(DegToRad(degrees));
        }
        
        /// <summary>
        /// Returns the Cos of an angle in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float degrees)
        {
            return (float) Math.Cos(DegToRad(degrees));
        }
        
        /// <summary>
        /// Returns the Tan of an angle in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float degrees)
        {
            return (float) Math.Tan(DegToRad(degrees));
        }
        
        /// <summary>
        /// Returns the angle in degrees for a given Sin value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ASin(float value)
        {
            return RadToDeg((float)Math.Asin(value));
        }
        
        /// <summary>
        /// Returns the angle in degrees for a given Cos value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ACos(float value)
        {
            return RadToDeg((float)Math.Acos(value));
        }
        
        /// <summary>
        /// Returns the angle in degrees for a given Tan value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ATan(float value)
        {
            return RadToDeg((float)Math.Atan(value));
        }
        
        /// <summary>
        /// Returns the angle in degrees for the given x and y values.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns>The angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ATan2(float y, float x)
        {
            return RadToDeg((float)Math.Atan2(y, x));
        }
        
        
        
        
        
        
        
        
        
        
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float f)
        {
            return (float) Math.Floor(f);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(float f)
        {
            return (int) Floor(f);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Floor(Vector2 vec)
        {
            return new Vector2(FeMath.Floor(vec.X), FeMath.Floor(vec.Y));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceil(float f)
        {
            return (float) Math.Ceiling(f);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(float f)
        {
            return (int) Ceil(f);
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
        
        
        
        
        
        
        /// <summary>
        /// Performs a bilinear interpolation between four points
        /// </summary>
        /// <param name="botLeft">Bottom-left point</param>
        /// <param name="botRight">Bottom-right point</param>
        /// <param name="topLeft">Top-left point</param>
        /// <param name="topRight">Top-right point</param>
        /// <param name="xPercent"></param>
        /// <param name="yPercent"></param>
        /// <returns></returns>
        public static float Blerp(float botLeft, float botRight, float topLeft, float topRight, float xPercent, float yPercent) {
            return Lerp(
                Lerp(botLeft, botRight, xPercent), 
                Lerp(topLeft, topRight, xPercent), 
                yPercent
            );
        }
        
        /// <summary>
        /// Interpolates two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float percent) {
            return a + (b - a) * percent;
        }
        
        /// <summary>
        /// Interpolates two colors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static Color Lerp(Color a, Color b, float percent)
        {
            return new Color(
                Lerp(a.R, b.R, percent), 
                Lerp(a.G, b.G, percent), 
                Lerp(a.B, b.B, percent), 
                Lerp(a.A, b.A, percent)
            );
        }

        
    }
}
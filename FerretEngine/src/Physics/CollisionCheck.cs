using System;
using System.Runtime.CompilerServices;
using FerretEngine.Components.Colliders;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;

namespace FerretEngine.Physics
{
    public static class CollisionCheck
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BoxWithBox(BoxCollider collider, BoxCollider other)
        {
            return !( 
                        (collider.Right <= other.Left)
                    ||  (collider.Bottom <= other.Top)
                    ||  (collider.Left >= other.Right)
                    ||  (collider.Top >= other.Bottom) 
                );
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BoxWithPoint(BoxCollider collider, PointCollider other)
        {
            return !( 
                        (collider.Right <= other.Left)
                    ||  (collider.Bottom <= other.Top)
                    ||  (collider.Left >= other.Right)
                    ||  (collider.Top >= other.Bottom) 
                );
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PointWithPoint(PointCollider collider, PointCollider other)
        {
            return Math.Abs(collider.Left - other.Left) < 1f 
                   || Math.Abs(collider.Top - other.Top) < 1f;
        }
    }
}
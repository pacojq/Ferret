using System;
using System.Runtime.CompilerServices;
using FerretEngine.Components.Colliders;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;

namespace FerretEngine.Physics
{
    public static class CollisionCheck
    {
        /*
            COLLISION CHECK IMPLEMENTATIONS
         
                        Box    Circle    Point
            Box      |   x       x         x
            Circle   |           x         x
            Point    |                     x
         */
        
        
        
        
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
        public static bool BoxWithCircle(BoxCollider collider, CircleCollider other)
        {
            float closestX = FeMath.Clamp(other.Position.X, collider.Left, collider.Right);
            float closestY = FeMath.Clamp(other.Position.Y, collider.Top, collider.Bottom);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = other.Position.X - closestX;
            float distanceY = other.Position.Y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (other.Radius * other.Radius);
        }
        
        
        
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleWithCircle(CircleCollider collider, CircleCollider other)
        {
            return FeMath.Distance(collider.Position, other.Position) <= collider.Radius + other.Radius;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleWithPoint(CircleCollider collider, PointCollider other)
        {
            return FeMath.Distance(collider.Position, other.Position) <= collider.Radius;
        }
        
        
        
        
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PointWithPoint(PointCollider collider, PointCollider other)
        {
            return Math.Abs(collider.Left - other.Left) < 1f 
                   || Math.Abs(collider.Top - other.Top) < 1f;
        }
    }
}
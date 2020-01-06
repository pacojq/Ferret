using FerretEngine.Graphics;
using FerretEngine.Physics;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components.Colliders
{
    public sealed class CircleCollider : Collider
    {
        public float Radius { get; }
        
        public override float Top => this.Position.Y - Radius;
        
        public override float Bottom => this.Top + Height;
        public override float Left => this.Position.X - Radius;
        public override float Right => this.Left + Width;


        public CircleCollider(float radius)
        {
            Radius = radius;
            Width = radius * 2;
            Height = radius * 2;
            Center = Vector2.Zero;
        }


        internal override void DebugDraw(float deltaTime)
        {
            FeDraw.Color = Color.Red;
            // TODO draw circle
            FeDraw.Rect(Left, Top, Width, Height, true);
            FeDraw.Color = Color.White;
        }

        
        
        internal override bool Accept(Collider other)
        {
            return other.CollidesWith(this);
        }
        
        internal override bool CollidesWith(BoxCollider other)
        {
            return CollisionCheck.BoxWithCircle(other, this);
        }

        internal override bool CollidesWith(CircleCollider other)
        {
            return CollisionCheck.CircleWithCircle(other, this);
        }
        
        internal override bool CollidesWith(PointCollider other)
        {
            return CollisionCheck.CircleWithPoint(this, other);
        }
    }
}
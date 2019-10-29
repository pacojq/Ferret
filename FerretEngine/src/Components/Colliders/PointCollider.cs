
using FerretEngine.Graphics;
using FerretEngine.Physics;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components.Colliders
{
    public sealed class PointCollider : Collider
    {
        public override float Top => this.Position.Y - Center.Y;
        
        public override float Bottom => this.Top + Height;
        public override float Left => this.Position.X - Center.X;
        public override float Right => this.Left + Width;


        public PointCollider(Vector2 center)
        {
            Width = 1;
            Height = 1;
            Center = center;
        }
        
        
        internal override void DebugDraw(float deltaTime)
        {
            FeDraw.Color = Color.Red;
            FeDraw.Rect(Left, Top, Width, Height, true);
            FeDraw.Color = Color.White;
        }

        
        
        internal override bool Accept(Collider other)
        {
            return other.CollidesWith(this);
        }
        
        internal override bool CollidesWith(BoxCollider other)
        {
            return CollisionCheck.BoxWithPoint(other, this);
        }
        
        internal override bool CollidesWith(CircleCollider other)
        {
            return CollisionCheck.CircleWithPoint(other, this);
        }

        internal override bool CollidesWith(PointCollider other)
        {
            return CollisionCheck.PointWithPoint(this, other);
        }
    }
}
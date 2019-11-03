
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Physics;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components.Colliders
{
    public sealed class BoxCollider : Collider
    {
        public override float Top => this.Position.Y - Center.Y;
        
        public override float Bottom => this.Top + Height;
        public override float Left => this.Position.X - Center.X;
        public override float Right => this.Left + Width;


        public BoxCollider(float width, float height, Vector2 center)
        {
            Assert.IsTrue(width>0 && height>0, 
                "Box colliders must have positive width and height values!");
            
            Width = width;
            Height = height;
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
            return CollisionCheck.BoxWithBox(this, other);
        }
        
        internal override bool CollidesWith(CircleCollider other)
        {
            return CollisionCheck.BoxWithCircle(this, other);
        }
        
        internal override bool CollidesWith(PointCollider other)
        {
            return CollisionCheck.BoxWithPoint(this, other);
        }
    }
}
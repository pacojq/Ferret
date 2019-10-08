
using FerretEngine.Graphics;
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
            Width = width;
            Height = height;
            Center = center;
        }
        
        
        internal override void DebugDraw(float deltaTime)
        {
            FeDraw.Rect(Left, Top, Width, Height, true);
        }

        
        
        internal override bool Accept(Collider other)
        {
            return other.CollidesWith(this);
        }
        
        internal override bool CollidesWith(BoxCollider other)
        {
            Vector2 relPos = other.Position - this.Position;
            
            if (this.Right <= relPos.X + other.Left) return false;
            if (this.Bottom <= relPos.Y + other.Top) return false;
            if (this.Left >= relPos.X + other.Right) return false;
            if (this.Top >= relPos.Y + other.Bottom) return false;
            return true;
        }
    }
}
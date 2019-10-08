using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using FerretEngine.Graphics;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox.Box
{
    public class BoxEntity : Entity
    {
        public BoxEntity()
        {
            Sprite sprite = FeGraphics.LoadSprite("box");
            Bind(new SpriteRenderer(sprite));
            
            Bind(new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero));
        }
    }
}
using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox.Box
{
    public class BoxEntity : Entity
    {
        public BoxEntity()
        {
            Sprite sprite = FeGraphics.LoadSprite("box");
            Bind(new SpriteRenderer(sprite));

            BoxCollider col = new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero);
            Bind(col);
            
            col.OnCollisionEnter += o => FeLog.Debug($"BOX ENTER: {this}");
            col.OnCollisionExit += o => FeLog.Debug($"BOX EXIT: {this}");
        }
    }
}
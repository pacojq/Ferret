using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            Sprite sprite = FeGraphics.LoadSprite("char01");
            Bind(new SpriteRenderer(sprite));

            BoxCollider col = new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero);
            Bind(col);

            col.OnCollisionEnter += o => FeLog.Debug("ENTER");
            col.OnCollisionExit += o => FeLog.Debug("EXIT");
            
            Bind(new PlayerComponent());
        }
    }
}
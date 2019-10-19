using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Loading;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            Sprite sprite = SpriteLoader.LoadSprites("character/charAtlas.feAsset")[0];
            Bind(new SpriteRenderer(sprite));

            BoxCollider col = new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero);
            Bind(col);

            col.OnCollisionEnter += o => FeLog.Debug($"PLAYER ENTER: {this}");
            col.OnCollisionExit += o => FeLog.Debug($"PLAYER EXIT: {this}");
            
            Bind(new PlayerComponent());
        }
    }
}
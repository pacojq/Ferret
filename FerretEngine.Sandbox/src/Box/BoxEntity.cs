using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Content;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using FerretEngine.Logging;
using FerretEngine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Sandbox.Box
{
    public class BoxEntity : Entity
    {
        public BoxEntity(Material mat = null)
        {
            if (mat == null)
                mat = Material.Default;
            
            Sprite sprite = FeContent.LoadSprite("box.png");
            Bind(new SpriteRenderer(sprite)
            {
                Material = mat
            });


            for (int i = 0; i < 8; i++)
            {
                Texture2D tex = new Texture2D(FeGraphics.GraphicsDevice, 24, 24);

                Color[] colors = new Color[24 * 24];
                for (int j = 0; j < 24 * 24; j++)
                    colors[j] = new Color(255, 255, 255, i * 32);
                tex.SetData(colors);

                Bind(new SpriteRenderer(new Sprite(tex))
                {
                    LocalPosition = new Vector2((i+1) * 32, 0)
                });
            }




            BoxCollider col = new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero);
            Bind(col);
            
            col.OnCollisionEnter += o => FeLog.Debug($"BOX ENTER: {this}");
            col.OnCollisionExit += o => FeLog.Debug($"BOX EXIT: {this}");
        }
    }
}
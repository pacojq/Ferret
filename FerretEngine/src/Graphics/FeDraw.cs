using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public static class FeDraw
    {
        private static FeGraphics _graphics;

        private static bool _isOpen;
        
        
        
        internal static void Initialize(FeGraphics graphics)
        {
            _graphics = graphics;
        }


        public static void Sprite(Sprite sprite, Vector2 position)
        {
            _graphics.SpriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.ClipRect,
                    Color.White
                );
        }
        
        public static void SpriteExt(Sprite sprite, Vector2 position, Color color, float rotation,
                Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _graphics.SpriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.ClipRect,
                    color,
                    rotation,
                    origin,
                    scale,
                    effects,
                    layerDepth
                );
        }
    }
}
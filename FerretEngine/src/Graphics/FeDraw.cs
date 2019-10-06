using System.Runtime.CompilerServices;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public static class FeDraw
    {
        private static FeGraphics _graphics;
        
        
        internal static void Initialize(FeGraphics graphics)
        {
            _graphics = graphics;
        }


        
        
        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sprite(Sprite sprite, Vector2 position)
        {
            _graphics.SpriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.ClipRect,
                    Color.White
                );
        }
        
        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
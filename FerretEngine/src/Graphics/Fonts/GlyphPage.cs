using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Fonts
{
    internal class GlyphPage : IDisposable
    {
        public Texture2D Texture { get; }

        public GlyphPage(GraphicsDevice graphicsDevice, int textureSize)
        {
            TextureSize = textureSize;
            nodes.Add(new Rectangle(0, 0, textureSize, textureSize));
            colors = new Color[textureSize * textureSize];
            for (int i = textureSize * textureSize; i-- > 0;)
                colors[i] = new Color(255, 0, 255, 0);

            Texture = new Texture2D(graphicsDevice, textureSize, textureSize, false, SurfaceFormat.Color);
        }

        public bool Pack(int w, int h, out Rectangle rect)
        {
            //  allocate an extra pixel on each side to prevent bleed
            w += 2;
            h += 2;

            for (int i = 0; i < nodes.Count; ++i)
            {
                if (w <= nodes[i].Width && h <= nodes[i].Height)
                {
                    var node = nodes[i];
                    nodes.RemoveAt(i);
                    rect = new Rectangle(node.X, node.Y, w, h);
                    nodes.Add(new Rectangle(rect.Right, rect.Y, node.Right - rect.Right, rect.Height));
                    nodes.Add(new Rectangle(rect.X, rect.Bottom, rect.Width, node.Bottom - rect.Bottom));
                    nodes.Add(new Rectangle(rect.Right, rect.Bottom, node.Right - rect.Right, node.Bottom - rect.Bottom));

                    //  pad in for result
                    rect.X += 1;
                    rect.Y += 1;
                    rect.Width -= 1;
                    rect.Height -= 1;
                    return true;
                }
            }

            rect = Rectangle.Empty;
            return false;
        }

        public void RenderGlyph(int width, int height, byte[] bitmap, int x, int y)
        {
            for (int by = 0; by < height; by++)
            {
                for (int bx = 0; bx < width; bx++)
                {
                    var src = by * width + bx;
                    var dest = (by + y) * TextureSize + bx + x;
                    float col = 255;
                    
                    // If we are not using BlendState.NonPremultiplied, weird stuff happen
                    //if (bitmap[src] < 32) // This if statement fixes some weird low-alpha background rendering
                    //    col = 0;
                    
                    colors[dest] = new Color(col, col, col, bitmap[src]);
                }
            }

            Texture.SetData(colors);
        }

        public void Dispose()
        {
            Texture.Dispose();
        }

        private readonly int TextureSize;
        private readonly Color[] colors;
        private readonly List<Rectangle> nodes = new List<Rectangle>();
        
    }
}
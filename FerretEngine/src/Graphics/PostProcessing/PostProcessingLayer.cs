using FerretEngine.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.PostProcessing
{
    internal struct PostProcessingLayer
    {
        public RenderTarget2D RenderTarget { get; }
        public Material Material { get; }

        public PostProcessingLayer(Material material)
        {
            Material = material;
            RenderTarget = new RenderTarget2D(
                    FeGraphics.GraphicsDevice, 
                    FeGraphics.Resolution.WindowWidth, 
                    FeGraphics.Resolution.WindowHeight
                );
        }

    }
}
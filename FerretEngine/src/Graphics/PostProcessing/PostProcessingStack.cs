using System.Collections.Generic;
using FerretEngine.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.PostProcessing
{
    public class PostProcessingStack
    {
        private readonly List<PostProcessingLayer> _layers;

        internal PostProcessingStack()
        {
            _layers = new List<PostProcessingLayer>();
        }
        
        public void PushLayer(Material mat)
        {
            PostProcessingLayer layer = new PostProcessingLayer(mat);
            _layers.Add(layer);
        }

        internal RenderTarget2D Render(RenderTarget2D origin, SpriteBatch sb)
        {
            RenderTarget2D target = origin;
            
            foreach (var layer in _layers)
            {
                FeGraphics.GraphicsDevice.SetRenderTarget(layer.RenderTarget);
                FeGraphics.GraphicsDevice.Clear(FeGame.Instance.ClearColor);
                
                layer.Material.Bind();
                sb.Begin(
                    SpriteSortMode.Texture, 
                    BlendState.NonPremultiplied,//BlendState.AlphaBlend, 
                    SamplerState.PointClamp, 
                    DepthStencilState.Default,
                    RasterizerState.CullNone,
                    layer.Material.Effect
                );
            
            
                Rectangle rect = new Rectangle(0, 0, FeGraphics.Resolution.WindowWidth, FeGraphics.Resolution.WindowHeight);
                sb.Draw(target, rect, Color.White);
            
                sb.End();

                target = layer.RenderTarget;
            }

            return target;
        }
    }
}
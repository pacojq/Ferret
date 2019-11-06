using FerretEngine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Renderers
{
    public class DefaultRenderer : Renderer
    {
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;

        public DefaultRenderer() : base(RenderSurface.Default, SpriteSortMode.Deferred)
        {
            BlendState = BlendState.NonPremultiplied;//BlendState.AlphaBlend, 
            SamplerState = SamplerState.LinearClamp;
        }
        

        public override void Render(Scene scene, float deltaTime)
        {
            foreach (var entity in scene.Entities)
            {
                entity.Draw(deltaTime);
            }
        }

    }
}
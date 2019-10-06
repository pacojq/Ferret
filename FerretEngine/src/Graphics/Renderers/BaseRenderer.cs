using FerretEngine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Renderers
{
    public class BaseRenderer : Renderer
    {
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;
        public Camera Camera;

        public BaseRenderer()
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.LinearClamp;
            Camera = new Camera(FeGame.Width, FeGame.Height);
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
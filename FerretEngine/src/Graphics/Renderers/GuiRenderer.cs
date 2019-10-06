using FerretEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Renderers
{
    public class GuiRenderer : Renderer
    {
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;

        public GuiRenderer() : base(RenderSurface.Gui, SpriteSortMode.Deferred)
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.LinearClamp;
            Camera = new Camera(FeGame.Width, FeGame.Height, Vector2.Zero);
        }
        

        public override void Render(Scene scene, float deltaTime)
        {
            foreach (var entity in scene.Entities)
            {
                entity.DrawGUI(deltaTime);
            }
        }

    }
}
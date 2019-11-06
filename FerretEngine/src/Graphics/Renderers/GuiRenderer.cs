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
            BlendState = BlendState.NonPremultiplied;//BlendState.AlphaBlend;
            SamplerState = SamplerState.LinearClamp;

            Vector2 pos = new Vector2(FeGame.Width/2f, FeGame.Height/2f);
            Camera = new Camera(pos);
        }
        

        public override void Render(Scene scene, float deltaTime)
        {
            foreach (var entity in scene.Entities)
            {
                entity.DrawGUI(deltaTime);
            }
        }
        
        public override void BeforeRender(Scene scene)
        {
            base.BeforeRender(scene);
            Camera.Update();
        }

    }
}
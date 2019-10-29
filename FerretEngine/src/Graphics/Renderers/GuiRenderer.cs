﻿using FerretEngine.Core;
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
            Camera = new Camera(new Vector2(FeGraphics.Resolution.VirtualWidth/2f, FeGraphics.Resolution.VirtualHeight/2f));
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
﻿using FerretEngine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Renderers
{
    public class DebugRenderer : Renderer
    {
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;

        public DebugRenderer() : base(RenderSurface.Default, SpriteSortMode.Deferred)
        {
            BlendState = BlendState.NonPremultiplied; // BlendState.AlphaBlend;
            SamplerState = SamplerState.LinearClamp;
        }
        

        public override void Render(Scene scene, float deltaTime)
        {
            if (FeGame.Instance.PhysicsDebugDraw)
                scene.Space.DebugDraw(deltaTime);
        }
        
    }
}
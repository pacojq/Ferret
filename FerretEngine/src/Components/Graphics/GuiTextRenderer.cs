﻿using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Fonts;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components.Graphics
{
    public class GuiTextRenderer : Component
    {
        public string Text { get; set; }
        
        public Font Font { get; set; }
        public Color Color { get; set; }
        
        public FeDraw.VAlign VAlign { get; set; }
        
        public FeDraw.HAlign HAlign { get; set; }
        
        
        
        public GuiTextRenderer(string text)
        {
            Text = text;
            Font = FeGraphics.DefaultFont;
            Color = Color.White;
            HAlign = FeDraw.HAlign.Centre;
            VAlign = FeDraw.VAlign.Centre;
        }



        public override void DrawGUI(float deltaTime)
        {
            Vector2 pos = Position - Entity.Scene.MainCamera.Position;
            
            Font temp = FeDraw.Font;
            FeDraw.Font = Font;
            
            FeDraw.SetHAlign(HAlign);
            FeDraw.SetVAlign(VAlign);
            
            FeDraw.TextExt(Text, pos, Color);
            
            FeDraw.Font = temp;
        }
    }
}
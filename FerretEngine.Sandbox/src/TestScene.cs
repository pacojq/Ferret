using System;
using FerretEngine.Core;
using FerretEngine.Logging;
using FerretEngine.Sandbox.Box;
using FerretEngine.Sandbox.Player;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox
{
    public class TestScene : Scene
    {
        public TestScene()
        {
            BackgroundColor = Color.DimGray;

            /*
            Random rand = new Random();
            for (int i = 0; i < 150; i++)
            {
                PlayerEntity e = new PlayerEntity();
                e.Position = new Vector2(rand.Next(SandboxGame.Width), rand.Next(SandboxGame.Height));
                AddEntity(e);
            }
            */
            AddEntity(new PlayerEntity());
            
            BoxEntity box = new BoxEntity();
            box.Position = new Vector2(64, 64);
            AddEntity(box);
        }

        public override void Begin()
        {
            base.Begin();
            FeLog.Info("Test scene entered!");
        }
    }
}
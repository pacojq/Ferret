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
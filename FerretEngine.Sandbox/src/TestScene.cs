using FerretEngine.Core;
using FerretEngine.Logging;
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
        }

        public override void Begin()
        {
            base.Begin();
            FeLog.Info("Test scene entered!");
        }
    }
}
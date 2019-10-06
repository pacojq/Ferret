using FerretEngine.Core;
using FerretEngine.Logging;
using FerretEngine.Sandbox.Player;

namespace FerretEngine.Sandbox
{
    public class TestScene : Scene
    {
        public TestScene()
        {
            AddEntity(new PlayerEntity());
        }

        public override void Begin()
        {
            base.Begin();
            FeLog.Info("Test scene entered!");
        }
    }
}
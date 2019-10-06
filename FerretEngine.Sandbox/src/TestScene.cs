using FerretEngine.Core;
using FerretEngine.Sandbox.Player;

namespace FerretEngine.Sandbox
{
    public class TestScene : Scene
    {
        public TestScene()
        {
            Add(new PlayerEntity());
        }

        public override void Begin()
        {
            base.Begin();
            FeGame.Instance.Logger.Log("Test scene entered!");
        }
    }
}
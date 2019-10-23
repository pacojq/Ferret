using FerretEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Sandbox
{
	public class SandboxGame : FeGame
	{
		public new const int Width = 320;
		public new const int Height = 240;
		public const int WindowWidth = Width  *2;
		public const int WindowHeight = Height * 2;


		public static Effect TestEffect;
		
		
		public SandboxGame() : base(Width, Height, WindowWidth, WindowHeight, 
				"Ferret Sandbox", false)
		{
            
		}

		protected override void Initialize()
		{
			base.Initialize();
			SetScene(new TestScene());
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			TestEffect = Content.Load<Effect>("Ferret/Effects/test");
		}
	}
}
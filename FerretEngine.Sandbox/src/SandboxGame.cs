using FerretEngine.Content;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using Microsoft.Xna.Framework;
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
            ClearColor = Color.Black;
		}

		protected override void Initialize()
		{
			base.Initialize();
			SetScene(new TestScene());
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			TestEffect = FeContent.LoadEffect("Ferret/Effects/test.fxb");
			//TestEffect = FeContent.LoadEffect("Ferret/Effects/colorPalette.fxb");
		
			Material postPro = new Material("Ferret/Effects/Surface/distortion.fxb");
			postPro.SetTexture("_MaskTexture", FeContent.LoadTexture("Ferret/Effects/Res/scanline.png"));
			FeGraphics.PostProcessing.PushLayer(postPro);
		}
	}
}
namespace FerretEngine.Sandbox
{
	public class SandboxGame : FeGame
	{
		public new const int Width = 320;
		public new const int Height = 240;
		public const int WindowWidth = Width  *2;
		public const int WindowHeight = Height * 2;
        
		public SandboxGame() : base(Width, Height, WindowWidth, WindowHeight, 
				"Ferret Sandbox", false)
		{
            
		}
		
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			base.Initialize();

			this.SetScene(new TestScene());
		}
	}
}
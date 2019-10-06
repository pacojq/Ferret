namespace FerretEngine.Sandbox
{
	public class SandboxGame : FeGame
	{
		public const int ScreenWidth = 480;
		public const int ScreenHeight = 320;
        
		public SandboxGame() : base(ScreenWidth, ScreenHeight, ScreenWidth, ScreenHeight, 
				"Ferret Sandbox", false, @"Content")
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
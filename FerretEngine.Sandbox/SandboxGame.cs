namespace FerretEngine.Sandbox
{
	public class SandboxGame : FerretGame
	{
		public const int ScreenWidth = 480;
		public const int ScreenHeight = 320;
        
		public SandboxGame() : base(ScreenWidth, ScreenHeight, ScreenWidth, ScreenHeight, 
				"Ferret Sandbox", false, @"Content")
		{
            
		}
	}
}
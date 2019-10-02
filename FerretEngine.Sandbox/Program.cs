namespace FerretEngine.Sandbox
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			using (SandboxGame game = new SandboxGame())
			{
				game.Run();
			}
		}
	}
}
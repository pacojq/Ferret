using System;

namespace FerretEngine.Input
{
	public static class FerretInput
	{

		public static KeyboardInput Keyboard { get; private set; }
		
		internal static void Initialize()
		{
			Keyboard = new KeyboardInput();
			
			Console.WriteLine("MnkInput initialized!");
		}
		
		
		
		public static void Update()
		{
			Keyboard.Update();
		}
		
		
	}
}
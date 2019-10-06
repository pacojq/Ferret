using System;
using FerretEngine.Logging;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
	public static class FeInput
	{

		public static KeyboardInput Keyboard { get; private set; }
		
		internal static void Initialize()
		{
			Keyboard = new KeyboardInput();
			
			FeLog.Info("FerretInput initialized!");
		}
		
		
		
		public static void Update()
		{
			Keyboard.Update();
		}
		
		public static bool IsKeyPressed(Keys key)
		{
			return Keyboard.IsKeyPressed(key);
		}
		
	}
}
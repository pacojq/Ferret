using System;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
	public static class FeInput
	{

		public static KeyboardInput Keyboard { get; private set; }
		
		public static GamepadInput Gamepad { get; private set; }
		
		internal static void Initialize()
		{
			Keyboard = new KeyboardInput();
			Gamepad = new GamepadInput(PlayerIndex.One);
			
			FeLog.FerretInfo("FerretInput initialized!");
		}
		
		
		
		public static void Update()
		{
			Keyboard.Update();
			Gamepad.Update();
		}
		
		public static bool IsKeyPressed(Keys key)
		{
			return Keyboard.IsKeyPressed(key);
		}
		
	}
}
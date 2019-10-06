using System;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
	public static class FeInput
	{

		public static KeyboardInput Keyboard { get; private set; }
		
		internal static void Initialize()
		{
			Keyboard = new KeyboardInput();
			
			FeGame.Instance.Logger.Log("FerretInput initialized!");
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
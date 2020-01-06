using System;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
	public static class FeInput
	{

		public static KeyboardInput Keyboard { get; private set; }
		
		public static GamepadInput[] Gamepads { get; private set; }

		private static bool[] _gamepadConneced;


		public delegate void GamepadEvent(int index);

		public static GamepadEvent OnGamepadConnected { get; set; }
		public static GamepadEvent OnGamepadDisconnected { get; set; }
		
		
		internal static void Initialize()
		{
			Keyboard = new KeyboardInput();
			Gamepads = new[]
			{
				new GamepadInput(PlayerIndex.One),
				new GamepadInput(PlayerIndex.Two),
				new GamepadInput(PlayerIndex.Three),
				new GamepadInput(PlayerIndex.Four)
			};
			_gamepadConneced = new bool[4];

			OnGamepadConnected += index => FeLog.FerretInfo("Gamepad connected: " + index);
			OnGamepadDisconnected += index => FeLog.FerretInfo("Gamepad disconnected: " + index);
			
			FeLog.FerretInfo("FerretInput initialized!");
		}
		
		
		
		public static void Update()
		{
			Keyboard.Update();

			for (int i = 0; i < 4; i++)
			{
				GamepadInput gp = Gamepads[i];
				gp.Update();
				bool connected = gp.IsConnected;
				
				if (!_gamepadConneced[i] && connected)
				{
					OnGamepadConnected(i);
				}
				else if (!connected && _gamepadConneced[i])
				{
					OnGamepadDisconnected(i);
				}

				_gamepadConneced[i] = connected;
			}
			
		}
		
		public static bool IsKeyPressed(Keys key)
		{
			return Keyboard.IsKeyPressed(key);
		}
		
	}
}
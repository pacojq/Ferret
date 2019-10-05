using FerretEngine.Utils;
using Microsoft.Xna.Framework;

namespace FerretEngine.Graphics
{
	public static class Screen
	{
		private static GraphicsDeviceManager _graphicsManager;


		public static void Initialize(GraphicsDeviceManager graphicsManager)
		{
			Assert.IsNull(_graphicsManager);
			_graphicsManager = graphicsManager;
			
		}
	}
}
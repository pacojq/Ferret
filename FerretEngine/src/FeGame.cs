using System;
using System.IO;
using System.Reflection;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Input;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine
{
	public class FeGame : Game
	{

		public static FeGame Instance { get; private set; }
		
		
		
		
		public static bool ExitOnEscapeKeypress = true;

		public static bool PauseOnFocusLost = true;

		public static bool DebugRenderEnabled = false;

		
		
		
		
		/// <summary>
		/// The time (in seconds) since the last update tick.
		/// </summary>
		public static float DeltaTime { get; private set; }
		
		
		
		public static string ContentDirectory
		{
#if PS4
            get { return Path.Combine("/app0/", Instance.Content.RootDirectory); }
#elif NSWITCH
            get { return Path.Combine("rom:/", Instance.Content.RootDirectory); }
#elif XBOXONE
            get { return Instance.Content.RootDirectory; }
#else
			get { return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Content"/*Instance.Content.RootDirectory*/); }
#endif
		}
		
		
		
		
		public string Title { get; }
		
		public static int Width { get; private set; }
		public static int Height { get; private set; }

		
		
		public ILogger Logger { get; }
		
		
		public Scene Scene { get; private set; }
		
		
		
		public FeGraphics Graphics { get; }

		
		
		// Variables used for FPS count //
		public int FPS { get; private set; }
		private int _fpsCounter = 0;
		private TimeSpan _counterElapsed = TimeSpan.Zero;





	
		
		
		

		public FeGame(int width, int height, int windowWidth, int windowHeight, string windowTitle, 
				bool fullscreen, string contentRootDirectory)
		{
			Instance = this;
			
			Title = Window.Title = windowTitle;
			Width = width;
			Height = height;
            
			Logger = new ConsoleLogger();

			Graphics = new FeGraphics(this, width, height, windowWidth, windowHeight, fullscreen);

			Content.RootDirectory = contentRootDirectory;
			
			IsMouseVisible = true;
			IsFixedTimeStep = false;
			
			
		}



		public void SetScene(Scene scene)
		{
			if (Scene != null)
				Scene.End();
			
			Scene = scene;
			scene.Begin();
		}
		
		
		
		
		
		/// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
	        base.Initialize();
	        
	        FeInput.Initialize();
	        
	        // TODO load default font
        }

		
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
	        Graphics.LoadContent();
	        
            base.LoadContent();
            
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            Logger.Log("Content: " + this.Content.RootDirectory);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
	        FeInput.Update();
	        
	        if (ExitOnEscapeKeypress && FeInput.IsKeyPressed(Keys.Escape))
	        {
		        Exit();
		        return;
	        }
	        
	        DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

	        
	        // TODO UPDATE
	        if (Scene != null)
	        {
		        Scene.Update(DeltaTime);
	        }

	        // MonoGame update
            base.Update(gameTime);
        }

        
        
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
	        Graphics.Render(gameTime);
            
            base.Draw(gameTime);
            
            //Frame counter
            _fpsCounter++;
            _counterElapsed += gameTime.ElapsedGameTime;
            if (_counterElapsed >= TimeSpan.FromSeconds(1))
            {
#if DEBUG
                Window.Title = $"{Title} [{_fpsCounter} fps -  {(GC.GetTotalMemory(false) / 1048576f):F} MB]";
#endif
                FPS = _fpsCounter;
                _fpsCounter = 0;
                _counterElapsed -= TimeSpan.FromSeconds(1);
            }
            
        }
        
        
        protected override void OnExiting(object sender, EventArgs args)
        {
	        base.OnExiting(sender, args);
	        Logger.Log("Exiting game");
        }
        
        
        
	}
}
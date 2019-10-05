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
	public class FerretGame : Game
	{

		public static FerretGame Instance { get; private set; }
		
		
		
		
		public static bool ExitOnEscapeKeypress = true;

		public static bool PauseOnFocusLost = true;

		public static bool DebugRenderEnabled = false;

		
		
		
		
		
		public static float DeltaTime { get; private set; }
		public static float RawDeltaTime { get; private set; }
		
		
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
		
		
		protected GraphicsDeviceManager GraphicsManager { get; }
		protected SpriteBatch SpriteBatch { get; private set; }

		
		
		// Variables used for FPS count //
		public int FPS { get; private set; }
		private int _fpsCounter = 0;
		private TimeSpan _counterElapsed = TimeSpan.Zero;





	
		
		
		

		public FerretGame(int width, int height, int windowWidth, int windowHeight, string windowTitle, 
				bool fullscreen, string contentRootDirectory)
		{
			Instance = this;
			
			Title = Window.Title = windowTitle;
			Width = width;
			Height = height;
            
			Logger = new ConsoleLogger();

			GraphicsManager = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = windowWidth,
				PreferredBackBufferHeight = windowHeight,
				IsFullScreen = fullscreen,
				SynchronizeWithVerticalRetrace = true
			};
			//GraphicsManager.DeviceReset += OnGraphicsReset;
			//GraphicsManager.DeviceCreated += OnGraphicsCreate;
            
			Window.AllowUserResizing = false;
			//Window.ClientSizeChanged += OnClientSizeChanged;

			Screen.Initialize(GraphicsManager);
			/*
			if (fullscreen)
			{
				GraphicsManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				GraphicsManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				GraphicsManager.IsFullScreen = true;
			}
			else
			{
				GraphicsManager.PreferredBackBufferWidth = windowWidth;
				GraphicsManager.PreferredBackBufferHeight = windowHeight;
				GraphicsManager.IsFullScreen = false;
			}
			GraphicsManager.ApplyChanges();
            */
            
			Content.RootDirectory = contentRootDirectory;
			
			IsMouseVisible = true;
			IsFixedTimeStep = false;
			
			
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
	        
	        FerretInput.Initialize();
	        
	        // TODO load default font
        }

		
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
	        // TODO
	        
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
	        FerretInput.Update();
	        
	        if (ExitOnEscapeKeypress && FerretInput.IsKeyPressed(Keys.Escape))
	        {
		        Exit();
		        return;
	        }
	        
	        // TODO UPDATE
	        if (Scene != null)
	        {
		        Scene.Update();
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
	        GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO Render();
            
            if (Scene != null)
            {
	            Scene.Render();
            }
            
            
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
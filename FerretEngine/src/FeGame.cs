using System;
using System.IO;
using System.Reflection;
using FerretEngine.Core;
using FerretEngine.Coroutines;
using FerretEngine.Graphics;
using FerretEngine.GUI;
using FerretEngine.Input;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
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
		
		
		public static Random Random { get; private set; }
		
		
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

		public Color ClearColor { get; set; }
		
		public Scene Scene { get; private set; }
		
		
		
		
		// Variables used for FPS count //
		public int FPS { get; private set; }

		private int _fpsCounter = 0;
		private TimeSpan _counterElapsed = TimeSpan.Zero;




		private FeGUI _gui;
		
		
		

		public FeGame(int width, int height, int windowWidth, int windowHeight, string windowTitle, bool fullscreen)
		{
			Instance = this;
			
			Title = Window.Title = windowTitle;
			Width = width;
			Height = height;

			FeLog.Initialize();
			FeGraphics.Initialize(this, width, height, windowWidth, windowHeight, fullscreen);

			Content.RootDirectory = @"Content";
			
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			ClearColor = Color.CornflowerBlue;
			
			Random = new Random();
			
			_gui = new FeGUI();
		}



		public void SetScene(Scene scene)
		{
			if (Scene != null)
				Scene.End();
			
			FeGraphics.OnSceneChange(scene);
			OnSceneChange(scene);
			
			Scene = scene;
			scene.Begin();
		}

		protected virtual void OnSceneChange(Scene scene)
		{
			// To be implemented by child classes
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

	        FeCoroutines.Initialize();
	        FeInput.Initialize();
	        
	        // TODO load default font
        }

		
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
	        FeGraphics.LoadContent();
	        
	        _gui.LoadContent();
	        
            base.LoadContent();
            
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            FeLog.Info("Content: " + this.Content.RootDirectory);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
	        FeLog.Warning("Ferret unloading content");
            // TODO: Unload any non ContentManager content here
        }

        
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
	        DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
	        
	        // Early Update
	        FeInput.Update();
	        
	        if (ExitOnEscapeKeypress && FeInput.IsKeyPressed(Keys.Escape))
	        {
		        Exit();
		        return;
	        }
	        
	        

	        // TODO Game Update
	        if (Scene != null)
	        {
		        Scene.Update(DeltaTime);
	        }
	        
	        
	        // Late update
	        FeCoroutines.Update(DeltaTime);

	        
	        // MonoGame update
            base.Update(gameTime);
        }

        
        
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
	        FeGraphics.Render(gameTime);
            
	        _gui.Draw();
	        
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
	        FeLog.Info("Exiting game");
        }
        
        
        protected override void OnActivated(object sender, EventArgs args)
        {
	        base.OnActivated(sender, args);

	        if (Scene != null)
		        Scene.OnFocusGained();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
	        base.OnDeactivated(sender, args);

	        if (Scene != null)
		        Scene.OnFocusLost();
        }
	}
}
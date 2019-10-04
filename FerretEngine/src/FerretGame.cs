using System;
using System.IO;
using System.Reflection;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;

namespace FerretEngine
{
	public class FerretGame : Game
	{
		
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
		
		
		protected GraphicsDeviceManager Graphics { get; }
		protected SpriteBatch SpriteBatch { get; private set; }

		
		
		// Variables used for FPS count //
		public int FPS { get; private set; }
		private int _fpsCounter = 0;
		private TimeSpan _counterElapsed = TimeSpan.Zero;

		
		
		
		private Desktop _desktop;
		
		
		

		public FerretGame(int width, int height, int windowWidth, int windowHeight, string windowTitle, bool fullscreen)
		{
			Title = Window.Title = windowTitle;
			Width = width;
			Height = height;
            
			Logger = new ConsoleLogger();
            
			Graphics = new GraphicsDeviceManager(this);
			//_graphics.DeviceReset += OnGraphicsReset;
			//_graphics.DeviceCreated += OnGraphicsCreate;
            
			Window.AllowUserResizing = true;
			//Window.ClientSizeChanged += OnClientSizeChanged;

			if (fullscreen)
			{
				Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				Graphics.IsFullScreen = true;
			}
			else
			{
				Graphics.PreferredBackBufferWidth = windowWidth;
				Graphics.PreferredBackBufferHeight = windowHeight;
				Graphics.IsFullScreen = false;
			}
			Graphics.ApplyChanges();
            
            
			Content.RootDirectory = @"Content";
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
        }

		
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
	        MyraEnvironment.Game = this;

	        var grid = new Grid
	        {
		        RowSpacing = 8,
		        ColumnSpacing = 8
	        };

	        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
	        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
	        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
	        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

// TextBlock
	        var helloWorld = new TextBlock
	        {
		        Id = "label",
		        Text = "Hello, World!"
	        };
	        grid.Widgets.Add(helloWorld);

// ComboBox
	        var combo = new ComboBox
	        {
		        GridColumn = 1,
		        GridRow = 0
	        };

	        combo.Items.Add(new ListItem("Red", Color.Red));
	        combo.Items.Add(new ListItem("Green", Color.Green));
	        combo.Items.Add(new ListItem("Blue", Color.Blue));
	        grid.Widgets.Add(combo);

// Button
	        var button = new TextButton
	        {
		        GridColumn = 0,
		        GridRow = 1,
		        Text = "Show"
	        };

	        button.Click += (s, a) =>
	        {
		        var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
		        messageBox.ShowModal(_desktop);
	        };

	        grid.Widgets.Add(button);

// Spin button
	        var spinButton = new SpinButton
	        {
		        GridColumn = 1,
		        GridRow = 1,
		        Width = 100,
		        Nullable = true
	        };
	        grid.Widgets.Add(spinButton);

// Add it to the desktop
	        _desktop = new Desktop();
	        _desktop.Widgets.Add(grid);
	        
	        
	        
	        
	        
	        
	        
	        
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
	        // TODO UPDATE
	        
	        
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

	        _desktop.Render();
	        
            // TODO Render();
            
            
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
        
        
        
        
        
	}
}
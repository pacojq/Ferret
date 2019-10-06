using System;
using System.IO;
using FerretEngine.Core;
using FerretEngine.Graphics.Renderers;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public class FeGraphics
    {
        
        /// <summary>
        /// A texture which is just a white pixel.
        /// </summary>
        public static Sprite Pixel { get; private set; }
        
        
        
        
        
        
        
        public Renderer Renderer
        {
            get => _renderer;
            set
            {
                Assert.IsNotNull(value);
                _renderer = value;
            }
        }
        private Renderer _renderer;
        

        public GraphicsDevice GraphicsDevice => _game.GraphicsDevice;
        public GraphicsDeviceManager GraphicsManager { get; }


        internal SpriteBatch SpriteBatch => _spriteBatch;
        private SpriteBatch _spriteBatch;
        
        
        
        
        private readonly FeGame _game;
        
        /// <summary>
        /// Whether the <see cref="_spriteBatch"/> has already called Begin()
        /// </summary>
        private bool _isOpen;
        
        private readonly RenderTarget2D _renderTarget;

        
        
        public FeGraphics(FeGame game, int width, int height, int windowWidth, int windowHeight, bool fullscreen)
        {
            _game = game;
            
            GraphicsManager = new GraphicsDeviceManager(game)
            {
                PreferredBackBufferWidth = windowWidth,
                PreferredBackBufferHeight = windowHeight,
                IsFullScreen = fullscreen,
                SynchronizeWithVerticalRetrace = true
            };
            //GraphicsManager.DeviceReset += OnGraphicsReset;
            //GraphicsManager.DeviceCreated += OnGraphicsCreate;
            
            GraphicsManager.ApplyChanges();
            
            game.Window.AllowUserResizing = false;
            //game.Window.ClientSizeChanged += OnClientSizeChanged;

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
            
            _renderTarget = new RenderTarget2D(GraphicsDevice, width, height);
            Renderer = new BaseRenderer();
            
            FeDraw.Initialize(this);
        }

        internal void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Pixel = Sprite.PlainColor(1, 1, Color.White);
        }

        
        private void Begin(SpriteSortMode sortMode)
        {
            Assert.IsFalse(_isOpen);
            _isOpen = true;
            
            _spriteBatch.Begin(
                sortMode, 
                BlendState.AlphaBlend, 
                SamplerState.PointClamp, 
                DepthStencilState.None, 
                RasterizerState.CullNone
            );
        }

        private void End()
        {
            Assert.IsTrue(_isOpen);
            _isOpen = false;
            
            _spriteBatch.End();
        }
        
        
        
        
        
        
        internal void Render(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Begin( Renderer.SortMode );

            Scene scene = _game.Scene;
            if (scene != null)
            {
                scene.BeforeRender();
                Renderer.BeforeRender(scene);

                Renderer.Camera = scene.MainCamera;
                
                Renderer.Render(scene, (float) gameTime.ElapsedGameTime.TotalSeconds);

                Renderer.Camera = null;
                
                Renderer.AfterRender(scene);
                scene.AfterRender();
            }
            
            End();
            
            
            
            
            GraphicsDevice.SetRenderTarget(null);
            Begin(SpriteSortMode.Texture);

            Rectangle rect = new Rectangle(0, 0, 
                    GraphicsManager.PreferredBackBufferWidth,
                    GraphicsManager.PreferredBackBufferHeight
                );
            _spriteBatch.Draw(_renderTarget, rect, Color.White);
            
            End();
        }
        
        
        
        
        
        
        /// <summary>
        /// Loads a raw PNG image from the Content directory as a Sprite.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string path)
        {
            string texPath = Path.Combine(FeGame.ContentDirectory, path) + ".png";
            var fileStream = new FileStream(texPath, FileMode.Open, FileAccess.Read);
            Texture2D texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
            fileStream.Close();
            return new Sprite(texture);
        }
        
        
        
        
    }
}
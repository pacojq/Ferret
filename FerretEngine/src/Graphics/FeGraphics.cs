using System;
using System.Collections.Generic;
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
        
        /// <summary>
        /// Default font for the Ferret Engine.
        /// </summary>
        public static SpriteFont DefaultFont { get; private set; }

        
        
        
        public GraphicsDevice GraphicsDevice => _graphicsDevice;
        private GraphicsDevice _graphicsDevice;
        
        public GraphicsDeviceManager GraphicsManager { get; }


        internal SpriteBatch SpriteBatch => _spriteBatch;
        private SpriteBatch _spriteBatch;


        public bool IsRendering => CurrentRenderer != null;
        
        internal Renderer CurrentRenderer { get; private set; }
        
        private readonly FeGame _game;
        
        
        private readonly List<Renderer> _allRenderers;
        private readonly List<Renderer> _defaultRenderers;
        private readonly List<Renderer> _guiRenderers;
        
        private readonly RenderTarget2D _renderTarget;


        /// <summary>
        /// Whether the <see cref="_spriteBatch"/> has already called Begin()
        /// </summary>
        private bool _isOpen;
        
        

        
        
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
            
            
            
            
            _renderTarget = new RenderTarget2D(game.GraphicsDevice, width, height);

            
            _allRenderers = new List<Renderer>();
            _defaultRenderers = new List<Renderer>();
            _guiRenderers = new List<Renderer>();

            AddRenderer(new DefaultRenderer());
            AddRenderer(new DebugRenderer());
            AddRenderer(new GuiRenderer());
            

            FeDraw.Initialize(this);
        }

        internal void LoadContent()
        {
            _graphicsDevice = _game.GraphicsDevice;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            Pixel = Sprite.PlainColor(1, 1, Color.White);
            DefaultFont = _game.Content.Load<SpriteFont>(@"Ferret\FerretDefault");

            FeDraw.Font = DefaultFont;
            FeDraw.Color = Color.White;
        }



        internal void OnSceneChange(Scene scene)
        {
            Camera sceneCamera = null;
            if (_game.Scene != null)
                sceneCamera = _game.Scene.MainCamera;
                
            foreach (Renderer renderer in _allRenderers)
            {
                if (renderer.Camera == sceneCamera)
                    renderer.Camera = scene.MainCamera;
            }
        }

        public void AddRenderer(Renderer renderer)
        {
            if (renderer.Camera == null)
                if (_game.Scene != null)
                    renderer.Camera = _game.Scene.MainCamera;

            switch (renderer.Surface)
            {
                case RenderSurface.Default:
                    _defaultRenderers.Add(renderer);
                    break;
                case RenderSurface.Gui:
                    _guiRenderers.Add(renderer);
                    break;
                default:
                    throw new Exception($"RendererSurface {renderer.Surface} is not supported");
            }
            
            _allRenderers.Add(renderer);
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
            Scene scene = _game.Scene;
            Color clearColor = Color.CornflowerBlue;
            if (scene != null)
                clearColor = scene.BackgroundColor;

            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(clearColor);
            
            // Make each Renderer render the scene in the corresponding order
            
            if (scene != null)
            {
                scene.BeforeRender();
                
                RenderList(scene, deltaTime, _defaultRenderers);
                RenderList(scene, deltaTime, _guiRenderers);
                
                scene.AfterRender();
            }
            
            
            // Now let's go to the default RenderTarget and render the target we've painted before
            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(clearColor);
            Begin(SpriteSortMode.Texture);

            Rectangle rect = new Rectangle(0, 0, 
                    GraphicsManager.PreferredBackBufferWidth,
                    GraphicsManager.PreferredBackBufferHeight
                );
            _spriteBatch.Draw(_renderTarget, rect, Color.White);
            
            End();
        }



        private void RenderList(Scene scene, float deltaTime, List<Renderer> renderers)
        {
            foreach (Renderer renderer in renderers)
            {
                CurrentRenderer = renderer;
                
                Begin(renderer.SortMode);
                renderer.BeforeRender(scene);
                
                renderer.Render(scene, deltaTime);

                renderer.AfterRender(scene);
                End();
            }

            CurrentRenderer = null;
        }

        
        
        
        
        
        
        /// <summary>
        /// Loads a raw PNG image from the Content directory as a <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string path)
        {
            string texPath = Path.Combine(FeGame.ContentDirectory, path) + ".png";
            var fileStream = new FileStream(texPath, FileMode.Open, FileAccess.Read);
            Texture2D texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
            fileStream.Close();
            return texture;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FerretEngine.Core;
using FerretEngine.Graphics.Renderers;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;

namespace FerretEngine.Graphics
{
    public static class FeGraphics
    {
        
        /// <summary>
        /// A texture which is just a white pixel.
        /// </summary>
        public static Sprite Pixel { get; private set; }
        
        /// <summary>
        /// Default font for the Ferret Engine.
        /// </summary>
        public static SpriteFont DefaultFont { get; private set; }

        
        
        
        public static GraphicsDevice GraphicsDevice => _graphicsDevice;
        private static GraphicsDevice _graphicsDevice;
        
        public static GraphicsDeviceManager GraphicsManager { get; private set; }


        internal static SpriteBatch SpriteBatch => _spriteBatch;
        private static SpriteBatch _spriteBatch;


        public static bool IsRendering => CurrentRenderer != null;
        
        internal static Renderer CurrentRenderer { get; private set; }

        
        internal static ResolutionManager Resolution { get; private set; }

        private static FeGame _game;
        
        
        private static List<Renderer> _allRenderers;
        private static List<Renderer> _defaultRenderers;
        private static List<Renderer> _guiRenderers;
        
        private static RenderTarget2D _renderTarget;


        /// <summary>
        /// Whether the <see cref="_spriteBatch"/> has already called Begin()
        /// </summary>
        private static bool _isOpen;
        
        
        private static Material _currentMaterial;

        
        
        internal static void Initialize(FeGame game, int width, int height, int windowWidth, int windowHeight, bool fullscreen)
        {
            _game = game;

            GraphicsManager = new GraphicsDeviceManager(game);
            
            Resolution = new ResolutionManager(GraphicsManager, width, height);
            
            // Game Resolution
            Resolution.SetVirtualResolution(width, height);
            
            // Window resolution
            Resolution.SetResolution(windowWidth, windowHeight, fullscreen);
            
            
            // TODO allow changing borderless and resizing
            game.Window.IsBorderlessEXT = false;
            game.Window.AllowUserResizing = false;
            
            _renderTarget = new RenderTarget2D(game.GraphicsDevice, width, height);

            _allRenderers = new List<Renderer>();
            _defaultRenderers = new List<Renderer>();
            _guiRenderers = new List<Renderer>();

            AddRenderer(new DefaultRenderer());
            AddRenderer(new DebugRenderer());
            AddRenderer(new GuiRenderer());
        }

        internal static void LoadContent()
        {
            _graphicsDevice = _game.GraphicsDevice;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            _currentMaterial = Material.Default;
            
            Pixel = Sprite.PlainColor(1, 1, Color.White);
            DefaultFont = _game.Content.Load<SpriteFont>(@"Ferret\FerretDefault");

            FeDraw.Font = DefaultFont;
            FeDraw.Color = Color.White;
        }



        internal static void OnSceneChange(Scene scene)
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

        public static void AddRenderer(Renderer renderer)
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
        
        
        
        
        
        
        
        
        
        
        private static void SpriteBatchBegin(Matrix transformMatrix, SpriteSortMode sortMode, Effect effect)
        {
            _spriteBatch.Begin(
                sortMode, 
                BlendState.AlphaBlend, 
                SamplerState.PointClamp, 
                DepthStencilState.Default, 
                RasterizerState.CullNone,
                effect,
                transformMatrix
            );
        }

        private static void SpriteBatchEnd()
        {
            _spriteBatch.End();
        }
        
        
        
        
        
        
        
        internal static void SetMaterial(Material material)
        {
            Assert.IsNotNull(CurrentRenderer);
            if (material != _currentMaterial)
            {
                SpriteBatchEnd();
                _currentMaterial = material;
                SpriteBatchBegin(CurrentRenderer.Camera.TransformMatrix, CurrentRenderer.SortMode, material.Effect);
            }
        }
        
        
        
        
        
        internal static void Render(GameTime gameTime)
        {
            Scene scene = _game.Scene;
            Color clearColor = _game.ClearColor;
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
            
            Resolution.BeginDraw();
           
            
            SpriteBatchBegin(Resolution.TransformationMatrix, SpriteSortMode.Texture, null);
            
            Rectangle rect = new Rectangle(0, 0, GraphicsManager.PreferredBackBufferWidth, GraphicsManager.PreferredBackBufferHeight);
            _spriteBatch.Draw(_renderTarget, rect, Color.White);
            
            SpriteBatchEnd();
        }



        private static void RenderList(Scene scene, float deltaTime, List<Renderer> renderers)
        {
            foreach (Renderer renderer in renderers)
            {
                CurrentRenderer = renderer;
                renderer.Camera.Update();

                _isOpen = true;
                //SpriteBatchBegin(CurrentRenderer.Camera.TransformMatrix, CurrentRenderer.SortMode, _currentMaterial?.Effect);
                SpriteBatchBegin(Resolution.TransformationMatrix, CurrentRenderer.SortMode, _currentMaterial?.Effect);
                
                renderer.BeforeRender(scene);
                renderer.Render(scene, deltaTime);
                renderer.AfterRender(scene);
                
                SpriteBatchEnd();
                _isOpen = false;
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




        public static Effect LoadEffect(string path)
        {
            string fxPath = Path.Combine(FeGame.ContentDirectory, path) + ".fxb";
            byte[] bytes = GetFileResourceBytes(fxPath);
            return new Effect(FeGame.Instance.GraphicsDevice, bytes);
        }
        
        
        private static byte[] GetFileResourceBytes(string path)
        {
            byte[] bytes;
            try
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    if (stream.CanSeek)
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var txt = string.Format(
                    "OpenStream failed to find file at path: {0}. Did you add it to the Content folder and set its properties to copy to output directory?",
                    path);
                throw new Exception(txt, e);
            }

            return bytes;
        }

        
    }
}
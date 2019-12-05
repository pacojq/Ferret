using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FerretEngine.Content;
using FerretEngine.Core;
using FerretEngine.Graphics.Effects;
using FerretEngine.Graphics.Fonts;
using FerretEngine.Graphics.PostProcessing;
using FerretEngine.Graphics.Renderers;
using FerretEngine.Logging;
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
        public static Font DefaultFont { get; private set; }
        
        
        public static MaterialLibrary Materials { get; private set; }
        
        
        
        
        
        public static GraphicsDevice GraphicsDevice => _graphicsDevice;
        private static GraphicsDevice _graphicsDevice;
        
        public static GraphicsDeviceManager GraphicsManager { get; private set; }


        internal static SpriteBatch SpriteBatch => _spriteBatch;
        private static SpriteBatch _spriteBatch;


        public static bool IsRendering => CurrentRenderer != null;
        
        internal static Renderer CurrentRenderer { get; private set; }

        
        internal static ResolutionManager Resolution { get; private set; }
        
        /// <summary>
        /// The width of the game window.
        /// </summary>
        public static int WindowWidth => Resolution.WindowWidth;
        
        /// <summary>
        /// The height of the game window.
        /// </summary>
        public static int WindowHeight => Resolution.WindowHeight;
        
        /// <summary>
        /// The width of the monitor display the game is running on.
        /// </summary>
        public static int DisplayWidth => Resolution.DisplayWidth;
        
        /// <summary>
        /// The height of the monitor display the game is running on.
        /// </summary>
        public static int DisplayHeight => Resolution.DisplayHeight;

        
        

        public static PostProcessingStack PostProcessing { get; private set; }
        
        private static FeGame _game;
        
        
        private static List<Renderer> _allRenderers;
        private static List<Renderer> _defaultRenderers;
        private static List<Renderer> _guiRenderers;
        


        /// <summary>
        /// Whether the <see cref="_spriteBatch"/> has already called Begin()
        /// </summary>
        private static bool _isOpen;

        private static Material _currentMaterial;

        private static RenderTarget2D _renderTarget;

        
        
        internal static void Initialize(FeGame game, int width, int height, int windowWidth, int windowHeight, bool fullscreen)
        {
            _game = game;

            GraphicsManager = new GraphicsDeviceManager(game);
            Resolution = new ResolutionManager(GraphicsManager, width, height, windowWidth, windowHeight, fullscreen);
            PostProcessing = new PostProcessingStack();
            
            // TODO allow changing borderless and resizing
            game.Window.IsBorderlessEXT = false;
            game.Window.AllowUserResizing = false;

            
            
            _allRenderers = new List<Renderer>();
            _defaultRenderers = new List<Renderer>();
            _guiRenderers = new List<Renderer>();

            
            AddRenderer(new DefaultRenderer());
#if DEBUG
            AddRenderer(new DebugRenderer());
#endif
            AddRenderer(new GuiRenderer());
        }


        internal static void LoadContent()
        {
            _graphicsDevice = _game.GraphicsDevice;
            
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            FeLog.FerretDebug($"Creating Render Target. Size = {Resolution.WindowWidth}x{Resolution.WindowHeight}");
            _renderTarget = new RenderTarget2D(GraphicsDevice, Resolution.WindowWidth, Resolution.WindowHeight);
            
            _currentMaterial = Material.Default;
            Materials = new MaterialLibrary();
            
            Pixel = Sprite.PlainColor(1, 1, Color.White);
            DefaultFont = FeContent.LoadFont("Ferret/Fonts/MatchupPro.ttf", 12);

            FeDraw.Font = DefaultFont;
            FeDraw.Color = Color.White;
        }


        internal static void OnWindowResize()
        {
            // TODO
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
        
        
        
        
        public static void SetResolution(int width, int height, bool fullScreen)
        { 
            Resolution.SetResolution(width, height, fullScreen);
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
                BlendState.NonPremultiplied,//BlendState.AlphaBlend, 
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
            if (!material.AreEqual(_currentMaterial))
            {
                SpriteBatchEnd();
                _currentMaterial = material;
                SpriteBatchBegin(CurrentRenderer.Camera.TransformMatrix, CurrentRenderer.SortMode, material.Effect);
            }
        }

        internal static void BindMaterial()
        {
            _currentMaterial.Bind();
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
            
            
            // Once we got the whole game rendered, let's add postprocessing

            RenderTarget2D target = PostProcessing.Render(_renderTarget, _spriteBatch);
            
            
            
            
            // Now let's go to the default RenderTarget and render the target we've painted before
            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(clearColor);


            Resolution.BeginDraw();


            _spriteBatch.Begin(
                SpriteSortMode.Texture, 
                BlendState.NonPremultiplied,//BlendState.AlphaBlend, 
                SamplerState.PointClamp, 
                DepthStencilState.Default,
                RasterizerState.CullNone
            );
            
            
            Rectangle rect = new Rectangle(0, 0, Resolution.WindowWidth, Resolution.WindowHeight);
            _spriteBatch.Draw(target, rect, Color.White);
            
            _spriteBatch.End();
            
        }



        private static void RenderList(Scene scene, float deltaTime, List<Renderer> renderers)
        {
            foreach (Renderer renderer in renderers)
            {
                CurrentRenderer = renderer;

                _isOpen = true;
                SpriteBatchBegin(CurrentRenderer.Camera.TransformMatrix, CurrentRenderer.SortMode, _currentMaterial?.Effect);
                //SpriteBatchBegin(Resolution.TransformationMatrix, CurrentRenderer.SortMode, _currentMaterial?.Effect);
                
                renderer.BeforeRender(scene);
                renderer.Render(scene, deltaTime);
                renderer.AfterRender(scene);
                
                SpriteBatchEnd();
                _isOpen = false;
            }

            CurrentRenderer = null;
        }

        
    }
}
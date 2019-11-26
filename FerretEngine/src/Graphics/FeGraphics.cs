using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FerretEngine.Content;
using FerretEngine.Core;
using FerretEngine.Graphics.Effects;
using FerretEngine.Graphics.Fonts;
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
        public static Font DefaultFont { get; private set; }
        
        
        
        
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
        


        /// <summary>
        /// Whether the <see cref="_spriteBatch"/> has already called Begin()
        /// </summary>
        private static bool _isOpen;
        
        
        private static Material _currentMaterial;

        private static RenderTarget2D _renderTarget;


        // TODO remove this
        private static Effect _testPostPro;
        
        
        internal static void Initialize(FeGame game, int width, int height, int windowWidth, int windowHeight, bool fullscreen)
        {
            _game = game;

            GraphicsManager = new GraphicsDeviceManager(game);
            Resolution = new ResolutionManager(GraphicsManager, width, height);
            
            Resolution.SetVirtualResolution(width, height); // Game Resolution
            Resolution.SetResolution(windowWidth, windowHeight, fullscreen); // Window resolution
            
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
            _renderTarget = new RenderTarget2D(GraphicsDevice, Resolution.WindowWidth, Resolution.WindowHeight);
            
            _currentMaterial = Material.Default;
            
            Pixel = Sprite.PlainColor(1, 1, Color.White);
            DefaultFont = FeContent.LoadFont("Ferret/Fonts/MatchupPro.ttf", 12);

            FeDraw.Font = DefaultFont;
            FeDraw.Color = Color.White;

            //_testPostPro = FeContent.LoadEffect("Ferret/Effects/Surface/plain.fxb");
            _testPostPro = FeContent.LoadEffect("Ferret/Effects/Surface/colored.fxb");
            _testPostPro.Parameters["Color"].SetValue(new Vector4(1, 0, 0, 1));
            //_testPostPro.Parameters["Color"].SetValue(new Vector4(1, 0, 0, 1));
        }


        internal static void OnWindowResize()
        {
            
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
           
            
            
            
            _spriteBatch.Begin(
                SpriteSortMode.Texture, 
                BlendState.NonPremultiplied,//BlendState.AlphaBlend, 
                SamplerState.PointClamp, 
                DepthStencilState.Default,
                RasterizerState.CullNone,
                _testPostPro
            );
            
            Rectangle rect = new Rectangle(0, 0, Resolution.WindowWidth, Resolution.WindowHeight);
            _spriteBatch.Draw(_renderTarget, rect, Color.White);
            
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
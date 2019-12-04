using System;
using System.Text;
using FerretEngine.Logging;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    internal class ResolutionManager
    {
        public int VirtualViewportX => virtualViewportX;
        public int VirtualViewportY => virtualViewportY;

        public int VirtualWidth => _VWidth;
        public int VirtualHeight => _VHeight;

        
        public int WindowWidth => _graphicsDevice.PreferredBackBufferWidth;
        public int WindowHeight => _graphicsDevice.PreferredBackBufferHeight;
        
        
        public int DisplayWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public int DisplayHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        
        
        
        public Matrix TransformationMatrix
        {
            get
            {
                if (_dirtyMatrix) 
                    RecreateScaleMatrix();
                return _transformMatrix;
            }
        }
        
        
        private readonly GraphicsDeviceManager _graphicsDevice;
        
        private int _width;
        private int _height;
        private int _VWidth;
        private int _VHeight;
        
        private bool _fullscreen = false;
        
        private Matrix _transformMatrix;
        
        private bool _dirtyMatrix = true;
        
        private int virtualViewportX;
    	private int virtualViewportY;

        
        public ResolutionManager(GraphicsDeviceManager graphicsDevice, int vWidth, int vHeight, int windowWidth, int windowHeight, bool fullscreen)
        {
            FeLog.FerretInfo("Initializing Resolution Manager.");
            
            StringBuilder str = new StringBuilder();
            str.Append("Available resolutions: ");
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                str.Append($"{dm.Width}x{dm.Height} ");
            
            FeLog.FerretInfo(str.ToString());
            
            _graphicsDevice = graphicsDevice;
            SetVirtualResolution(vWidth, vHeight);
            SetResolution(windowWidth, windowHeight, fullscreen);
        }




        public void SetResolution(int width, int height, bool fullScreen)
        {
            FeLog.FerretWarning($"Setting resolution: {width}x{height}. Fullscreen: {fullScreen}.");
            SetResolutionImpl(width, height, fullScreen);
        }
        
        
        private void SetResolutionImpl(int width, int height, bool fullScreen)
        {
            _width = width;
            _height = height;
            _fullscreen = fullScreen;

            if (_width > DisplayWidth || _height > DisplayHeight)
            {
                if (_width > DisplayWidth)
                {
                    float scale = (float) DisplayWidth / (float) _width;
                    SetResolutionImpl(DisplayWidth, FeMath.FloorToInt(scale * _height), _fullscreen);
                    return;
                }
                if (_height > DisplayHeight)
                {
                    float scale = (float) DisplayHeight / (float) _height;
                    SetResolutionImpl(FeMath.FloorToInt(scale * _width), DisplayHeight, _fullscreen);
                    return;
                }
            }

            ApplyResolutionSettings();
        }

        public void SetVirtualResolution(int vWidth, int vHeight)
        {
            FeLog.FerretWarning($"Setting virtual resolution: {vWidth}x{vHeight}");
            
            _VWidth = vWidth;
            _VHeight = vHeight;

            _dirtyMatrix = true;
        }



        private void ApplyResolutionToGraphicsDevice()
        {
            FeLog.FerretInfo($"Applying resolution settings. Preferred Back Buffer Size: {_width}x{_height}. Fullscreen: {_fullscreen}.");
            
            _graphicsDevice.PreferredBackBufferWidth = _width;
            _graphicsDevice.PreferredBackBufferHeight = _height;
            _graphicsDevice.IsFullScreen = _fullscreen;
            _graphicsDevice.PreferMultiSampling = true;
            _graphicsDevice.ApplyChanges();
        }
        
        
        private void ApplyResolutionSettings()
        {

#if XBOX360
            _fullscreen = true;
#endif

            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (!_fullscreen)
            {
                if (_width > DisplayWidth || _height > DisplayHeight)
                    Assert.Fail("We should not reach this point.");
               
                ApplyResolutionToGraphicsDevice();
                
                _dirtyMatrix = true;
                _width =  _graphicsDevice.PreferredBackBufferWidth;
                _height = _graphicsDevice.PreferredBackBufferHeight;
                
                return;
            }
            
            
            // Fullscreen
            
            float ratio = _width / (float) _height;
            DisplayMode fitDisplayMode = ChooseBestFitDisplayMode();

            if (fitDisplayMode != null)
            {
                FeLog.Debug($"Found fitting display mode: {fitDisplayMode}");
                
                _width = fitDisplayMode.Width;
                _height = fitDisplayMode.Height;
            }
            else
            {
                _width = DisplayWidth;
                _height = DisplayHeight;
            }
            ApplyResolutionToGraphicsDevice();
            
            _dirtyMatrix = true;
            _width =  _graphicsDevice.PreferredBackBufferWidth;
            _height = _graphicsDevice.PreferredBackBufferHeight;
        }


        private DisplayMode ChooseBestFitDisplayMode()
        {
            DisplayMode fitDisplayMode = null;
            float scale = 0f;
            
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (_width > dm.Width || _height > dm.Height)
                    continue;

                int wScale = FeMath.FloorToInt(dm.Width / (float) _width);
                int hScale = FeMath.FloorToInt(dm.Width / (float) _height);
                   
                // Check the width and height of each mode against the passed values
                if (dm.Width == _width * wScale && dm.Height >= _height * wScale)
                {
                    if (wScale > scale)
                    {
                        scale = wScale;
                        fitDisplayMode = dm;
                    }
                }
                else if (dm.Width >= _width * hScale && dm.Height == _height * hScale)
                {
                    if (hScale > scale)
                    {
                        scale = hScale;
                        fitDisplayMode = dm;
                    }
                }
            }

            return fitDisplayMode;
        }
        
        
        
        

        /// <summary>
        /// Sets the device to use the draw pump
        /// Sets correct aspect ratio
        /// </summary>
        public void BeginDraw()
        {
            // Start by resetting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            _graphicsDevice.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            _graphicsDevice.GraphicsDevice.Clear(Color.Black);
        }

        private void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            float wScale = (float) _graphicsDevice.GraphicsDevice.Viewport.Width / _VWidth;
            
            _transformMatrix = Matrix.CreateScale(wScale, wScale, 1f);
        }


        public void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _width;
            vp.Height = _height;
            _graphicsDevice.GraphicsDevice.Viewport = vp;
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public float getVirtualAspectRatio()
        {
            return (float)_VWidth / (float)_VHeight;
        }

        public void ResetViewport()
        {
            float targetAspectRatio = getVirtualAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            int width = _graphicsDevice.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + .5f);
            bool changed = false;
            
            if (height > _graphicsDevice.PreferredBackBufferHeight)
            {
                height = _graphicsDevice.PreferredBackBufferHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport viewport = new Viewport();

            viewport.X = (_graphicsDevice.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (_graphicsDevice.PreferredBackBufferHeight / 2) - (height / 2);
            virtualViewportX = viewport.X;
	        virtualViewportY = viewport.Y;
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
            {
                _dirtyMatrix = true;
            }

            _graphicsDevice.GraphicsDevice.Viewport = viewport;
        }
    }
}
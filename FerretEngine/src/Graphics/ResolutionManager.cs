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
        private int _VWidth = 1024;
        private int _VHeight = 768;
        
        private bool _fullscreen = false;
        
        private Matrix _transformMatrix;
        
        private bool _dirtyMatrix = true;
        
        private int virtualViewportX;
    	private int virtualViewportY;

        
        public ResolutionManager(GraphicsDeviceManager graphicsDevice, int width, int height)
        {
            FeLog.FerretInfo("Initializing Resolution Manager.");
            
            StringBuilder str = new StringBuilder();
            str.AppendLine("Available resolutions:");
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                str.AppendLine($"{dm.Width}x{dm.Height}");
            
            FeLog.FerretInfo(str.ToString());
            
            _graphicsDevice = graphicsDevice;
            _width = _graphicsDevice.PreferredBackBufferWidth;
            _height = _graphicsDevice.PreferredBackBufferHeight;
            
            _dirtyMatrix = true;
            
            ApplyResolutionSettings();
        }

        

        public void SetResolution(int Width, int Height, bool FullScreen)
        {
            _width = Width;
            _height = Height;

            _fullscreen = FullScreen;

           ApplyResolutionSettings();
        }

        public void SetVirtualResolution(int Width, int Height)
        {
            _VWidth = Width;
            _VHeight = Height;

            _dirtyMatrix = true;
        }

        private void ApplyResolutionSettings()
       {

#if XBOX360
           _fullscreen = true;
#endif

           // If we aren't using a full screen mode, the height and width of the window can
           // be set to anything equal to or smaller than the actual screen size.
           if (_fullscreen == false)
           {
               if (_width > DisplayWidth || _height > DisplayHeight)
               {
                   float scaling = Math.Min(DisplayWidth / (float)_width, DisplayHeight / (float)_height);

                   _width = FeMath.FloorToInt(_width * scaling);
                   _height = FeMath.FloorToInt(_height * scaling);
               }
               
               _graphicsDevice.PreferredBackBufferWidth = _width;
               _graphicsDevice.PreferredBackBufferHeight = _height;
               _graphicsDevice.IsFullScreen = _fullscreen;
               _graphicsDevice.PreferMultiSampling = true;
               _graphicsDevice.ApplyChanges();
           }
           else
           {
               float ratio = _width / (float) _height;
               
               float scale = 1f; // The scale we're going to set.
               float closerDif = float.MaxValue;
               
               // If we are using full screen mode, we should check to make sure that the display
               // adapter can handle the video mode we are trying to set.  To do this, we will
               // iterate through the display modes supported by the adapter and check them against
               // the mode we want to set.
               bool success = false;
               foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
               {
                   if (success)
                       break;
                   
                   // Check the width and height of each mode against the passed values
                   if ((dm.Width == _width) && (dm.Height == _height))
                   {
                       // The mode is supported, so set the buffer formats, apply changes and return
                       _graphicsDevice.PreferredBackBufferWidth = _width;
                       _graphicsDevice.PreferredBackBufferHeight = _height;
                       _graphicsDevice.IsFullScreen = _fullscreen;
                       _graphicsDevice.PreferMultiSampling = true;
                       _graphicsDevice.ApplyChanges();

                       success = true;
                   }
                   else
                   {
                       float adjustedWidth = _width;
                       float adjustedHeight = _height;
                       
                       if (adjustedWidth > dm.Width || adjustedHeight > DisplayHeight)
                       {
                           float scaling = FeMath.FloorToInt(Math.Min(DisplayWidth / (float)adjustedWidth, DisplayHeight / (float)adjustedHeight));

                           adjustedWidth = FeMath.FloorToInt(adjustedWidth * scaling);
                           adjustedHeight = FeMath.FloorToInt(adjustedHeight * scaling);
                       }
                       
                       float displayRatio = dm.Width / (float) dm.Height;
                       float dif = Math.Abs(ratio - displayRatio);

                       if (dif < closerDif)
                       {
                           closerDif = dif;
                           scale = Math.Min(dm.Width / (float)adjustedWidth, dm.Height / (float)_height);
                       }
                   }
               }

               if (!success)
               {
                   // Adjust to closest ratio
                   //scale = FeMath.FloorToInt(scale);
                       
                   _width = FeMath.FloorToInt(_width * scale);
                   _height = FeMath.FloorToInt(_height * scale);
                   
                   _graphicsDevice.PreferredBackBufferWidth = _width;
                   _graphicsDevice.PreferredBackBufferHeight = _height;
                   _graphicsDevice.IsFullScreen = _fullscreen;
                   _graphicsDevice.PreferMultiSampling = true;
                   _graphicsDevice.ApplyChanges();
               }
           }

           _dirtyMatrix = true;

           _width =   _graphicsDevice.PreferredBackBufferWidth;
           _height = _graphicsDevice.PreferredBackBufferHeight;
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
            _transformMatrix = Matrix.CreateScale(
                           (float)_graphicsDevice.GraphicsDevice.Viewport.Width / _VWidth,
                           (float)_graphicsDevice.GraphicsDevice.Viewport.Width / _VWidth,
                           1f);
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
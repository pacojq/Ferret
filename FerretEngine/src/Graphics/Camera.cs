using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace FerretEngine.Graphics
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Camera
    {
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }
        private Vector2 _position;
        
        public float Rotation { get; set; }
        
        public bool Active { get; set; }

        public float Zoom
        {
            get => _zoom;
            set => _zoom = Math.Max(0.1f, value);
        }
        private float _zoom;

        public Matrix TransformMatrix => _transformMatrix;
        private Matrix _transformMatrix;
        
        private Rectangle screenRect;
        

        public Camera(Vector2 position)
        {
            Active = true;
            _position = position;
            Zoom = 1;
        }
        
        public Camera() :
            this(new Vector2(FeGraphics.Resolution.VirtualWidth/2f, FeGraphics.Resolution.VirtualHeight/2f))
        {
        }


        public void Update()
        {
            CalculateMatrixAndRectangle();
        }
        
        private void CalculateMatrixAndRectangle()
        {
            //The math involved with calculated our transformMatrix and screenRect is a little intense, so instead of calling the math whenever we need these variables,
            //we'll calculate them once each frame and store them... when someone needs these variables we will simply return the stored variable instead of re cauclating them every time.

            Vector3 middleScreen = new Vector3(
                FeGraphics.Resolution.VirtualWidth * 0.5f,
                FeGraphics.Resolution.VirtualHeight * 0.5f, 0);
            
            //Calculate the camera transform matrix:
            _transformMatrix = Matrix.CreateTranslation(new Vector3(-_position, 0)) 
                    * Matrix.CreateRotationZ(Rotation) 
                    * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) 
                    * Matrix.CreateTranslation(middleScreen);

            //Now combine the camera's matrix with the Resolution Manager's transform matrix to get our final working matrix:
            _transformMatrix = _transformMatrix * FeGraphics.Resolution.TransformationMatrix;

            //Round the X and Y translation so the camera doesn't jerk as it moves:
            _transformMatrix.M41 = (float)Math.Round(_transformMatrix.M41, 0);
            _transformMatrix.M42 = (float)Math.Round(_transformMatrix.M42, 0);

            //Calculate the rectangle that represents where our camera is at in the world:
            screenRect = VisibleArea();
        }

        /// <summary>
        /// Calculates the screenRect based on where the camera currently is.
        /// </summary>
        private Rectangle VisibleArea()
        {
            Matrix inverseViewMatrix = Matrix.Invert(TransformMatrix);
            Vector2 tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            Vector2 tr = Vector2.Transform(new Vector2(FeGraphics.Resolution.VirtualWidth, 0), inverseViewMatrix);
            Vector2 bl = Vector2.Transform(new Vector2(0, FeGraphics.Resolution.VirtualHeight), inverseViewMatrix);
            Vector2 br = Vector2.Transform(new Vector2(FeGraphics.Resolution.VirtualWidth, FeGraphics.Resolution.VirtualHeight), inverseViewMatrix);
            Vector2 min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            Vector2 max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            return new Rectangle((int)min.X, (int)min.Y, (int)(FeGraphics.Resolution.VirtualWidth / Zoom), (int)(FeGraphics.Resolution.VirtualHeight / Zoom));
        }



        /*
        public Vector2 WorldToScreen(Vector2 entityPosition)
        {
            return Vector2.Transform(entityPosition, this.TransformMatrix);
        }
        */
    }
}
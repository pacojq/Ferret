using System.Collections.Generic;
using System.Linq;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components
{
    public abstract class Collider : Component
    {
        
        public virtual float Width { get; protected set; }
        public virtual float Height { get; protected set; }
		
        public bool IsTrigger { get; set; }
		
        public Vector2 Center { get; set; }


        public virtual float Top { get; }
        public virtual float Bottom { get; }
        public virtual float Left { get; }
        public virtual float Right { get; }
        
        
        
        
        public delegate void CollisionEvent(Collider other);

        public CollisionEvent OnCollisionEnter;
        public CollisionEvent OnCollisionStay;
        public CollisionEvent OnCollisionExit;
        
        private HashSet<Collider> _collisionCache;
        private HashSet<Collider> _newCollisions;

        public Collider()
        {
            _collisionCache = new HashSet<Collider>();
            _newCollisions = new HashSet<Collider>();
        }
        
        internal void Collide(Collider other)
        {
            if (!_collisionCache.Contains(other))
            {
                if (OnCollisionEnter != null)
                    OnCollisionEnter(other);
            }
            else
            {
                if (OnCollisionStay != null)
                    OnCollisionStay(other);   
            }
            _newCollisions.Add(other);
        }

        internal void AfterPhysicsUpdate()
        {
            IEnumerable<Collider> notCollided = _collisionCache.Where(c => !_newCollisions.Contains(c));
            foreach (Collider col in notCollided)
            {
                if (OnCollisionExit != null)
                    OnCollisionExit(col);
            }
            _collisionCache.Clear();
            
            _collisionCache = _newCollisions;
            _newCollisions = new HashSet<Collider>();
        }


        public override void Draw(float deltaTime)
        {
            //DebugDraw(deltaTime);
        }

        internal abstract void DebugDraw(float deltaTime);



        // = = = = = = = = = = = VISITOR PATTERN = = = = = = = = = = = //
        
        internal abstract bool Accept(Collider other);
        
        internal abstract bool CollidesWith(BoxCollider other);
        internal abstract bool CollidesWith(CircleCollider other);
        internal abstract bool CollidesWith(PointCollider other);
        
    }
}
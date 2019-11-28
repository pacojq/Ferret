using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FerretEngine.Components;
using FerretEngine.Graphics;
using FerretEngine.Logging;
using FerretEngine.Physics;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Core
{
	public class Entity : IEnumerable<Component>
    {
        private static uint _idCount;
        public readonly uint ID;
        
        
        #region // - - - - - Properties - - - - - //

        /// <summary>
        /// The <see cref="Core.Scene"/> this Entity is in.
        /// </summary>
        public Scene Scene { get; internal set; }

        
        /// <summary>
        /// The <see cref="Core.Layer"/> where the Entity is
        /// stored inside the <see cref="Core.Scene"/>.
        /// </summary>
        public Layer Layer { get; internal set; }
        
        
        
        /// <summary>
        /// A tag that identifies the entity.
        /// If not set, it will return null.
        /// </summary>
        public string Tag { get; protected set; }
        
        
        public IEnumerable<Component> Components => _components;
        private readonly List<Component> _components;
        
        
        public IEnumerable<Collider> Colliders => _colliders;
        private readonly List<Collider> _colliders;

        
        
        public Vector2 Position { get; set; }
        
        
        public float Rotation { get; set; }


        // Left as a property just in case we want to
        // do something with the scene on depth set
        public int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        /// <summary>
        /// Shortcut to get and set <see cref="Position"/>.X
        /// </summary>
        public float X
        {
            get => Position.X;
            set => Position += new Vector2(value, 0);
        }

        /// <summary>
        /// Shortcut to get and set <see cref="Position"/>.Y
        /// </summary>
        public float Y
        {
            get => Position.Y;
            set => Position += new Vector2(0, value);
        }


        #endregion



        

        #region // - - - - - Fields - - - - - //

        
        
        public bool IsActive = true;
        public bool IsVisible = true;
        public bool IsCollidable = true;
        
        internal int _depth = 0;
        
        
        #endregion
        
        
        

        public Entity(string tag, Vector2 position)
        {
            ID = _idCount++;

            Tag = tag;
            Position = position;
            _components = new List<Component>();
            _colliders = new List<Collider>();
        }

        
        // Util constructors //

        public Entity(string tag) : this(tag, Vector2.Zero)
        {
        }

        public Entity() : this(null, Vector2.Zero)
        {
        }
        
        
        
        
        

        /// <summary>
        /// Update game logic.
        /// This method will be skipped if the entity is not <see cref="IsActive"/>.
        /// 
        /// Rendering calls should never be called from this method.
        /// <param name="deltaTime">The time in seconds since the last update</param>
        /// </summary>
        internal void Update(float deltaTime)
        {
            foreach (Component c in Components)
            {
                c.Update(deltaTime);
            }

            OnUpdate(deltaTime);
        }
        
        /// <summary>
        /// Update game logic.
        /// This method will be skipped if the entity is not <see cref="IsActive"/>.
        /// 
        /// Rendering calls should never be called from this method.
        /// <param name="deltaTime">The time in seconds since the last update</param>
        /// </summary>
        protected virtual void OnUpdate(float deltaTime)
        {
            
        }
        
        
        internal virtual void Draw(float deltaTime)
        {
            foreach (Component c in Components)
            {
                c.Draw(deltaTime);
            }
        }
        
        
        internal void DrawGUI(float deltaTime)
        {
            foreach (Component c in Components)
            {
                c.DrawGUI(deltaTime);
            }
        }


        public void Destroy()
        {
            this.Scene.Destroy(this);
        }
        
        
        
        
        
        #region // - - - - - Component Management - - - - - //

        public void Bind(Component component)
        {
            if (component.Entity != null)
                throw new InvalidOperationException("Cannot add a component that is already bound to an entity.");
            
            _components.Add(component);
            component.Bind(this);

            if (component is Collider)
            {
                Collider col = (Collider) component;
                _colliders.Add(col);
                if (this.Scene != null)
                    this.Scene.Space.Add(col);
            }
        }
        

        public void Unbind(Component component)
        {
            if (component.Entity != this)
                throw new ArgumentException("An Entity can only unbind components which are bound to it.");
            
            component.Unbind();
            _components.Remove(component);
            
            if (component is Collider)
            {
                Collider col = (Collider) component;
                _colliders.Remove(col);
                if (this.Scene != null)
                    this.Scene.Space.Remove(col);
            }
        }

        public void Bind(params Component[] components)
        {
            foreach (var c in components)
                Bind(c);
        }

        public void Unbind(params Component[] components)
        {
            foreach (var c in components)
                Unbind(c);
        }

        public T Get<T>() where T : Component
        {
            foreach (var c in Components)
                if (c is T)
                    return c as T;
            return null;
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion



        
        
        public virtual void OnSceneBegin(Scene scene)
        {
            // TODO
        }
        
        
        public virtual void OnSceneEnd(Scene scene)
        {
            // TODO
        }



        public override string ToString()
        {
            string tag = string.IsNullOrEmpty(Tag) ? "-" : Tag;
            return $"[{this.GetType().Name} | id:{ID} | tag:{tag}]";
        }
    }
}
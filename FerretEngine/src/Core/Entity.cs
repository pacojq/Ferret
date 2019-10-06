using System;
using System.Collections;
using System.Collections.Generic;
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
        /// The Scene this Entity is in.
        /// </summary>
        public Scene Scene { get; private set; }

        
        /// <summary>
        /// A tag that identifies the entity.
        /// If not set, it will return null.
        /// </summary>
        public string Tag { get; }
        
        
        public IEnumerable<Component> Components => _components;
        private readonly List<Component> _components;
        
        
        
        public Vector2 Position { get; set; }


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

        
        
        public bool Active = true;
        public bool Visible = true;
        public bool Collidable = true;
        
        internal int _depth = 0;
        
        
        #endregion
        
        
        

        public Entity(string tag, Vector2 position)
        {
            ID = _idCount++;

            Tag = tag;
            Position = position;
            _components = new List<Component>();
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
        /// Don't perform any rendering calls here.
        /// This method will be skipped if the entity is not <see cref="Active"/>
        /// <param name="deltaTime">The time in seconds since the last update</param>
        /// </summary>
        public void Update(float deltaTime)
        {
            foreach (Component c in Components)
            {
                c.Update(deltaTime);
            }
        }
        
        
        public void Draw(float deltaTime)
        {
            foreach (Component c in Components)
            {
                c.Draw(deltaTime);
            }
        }
        
        
        
        
        
        
        #region // - - - - - Component Management - - - - - //

        public void Bind(Component component)
        {
            if (component.Entity != null)
                throw new InvalidOperationException("Cannot add a component that is already bound to an entity.");
            
            _components.Add(component);
            component.Bind(this);
        }
        

        public void Unbind(Component component)
        {
            if (component.Entity != this)
                throw new ArgumentException("An Entity can only unbind components which are bound to it.");
            
            component.Unbind();
            _components.Remove(component);
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
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FerretEngine.Components;
using FerretEngine.Graphics;
using FerretEngine.Physics;
using Microsoft.Xna.Framework;

namespace FerretEngine.Core
{
	public abstract class Scene : IEnumerable<Entity>, IEnumerable
    {

        public float TimeActive{ get; private set; }


        public Color BackgroundColor { get; set; }

        public IEnumerable<Entity> Entities => _entities;
        private readonly List<Entity> _entities;
        
        public Camera MainCamera { get; set; }
        
        
        /// <summary>
        /// The physics space of the scene
        /// </summary>
        internal Space Space { get; }

        public Vector2 Gravity
        {
            get => this.Space.Gravity;
            set => this.Space.Gravity = value;
        }
        
        
        public bool Paused;
        


        public Scene()
        {
            _entities = new List<Entity>();
            Space = new Space();

            BackgroundColor = Color.CornflowerBlue;
            MainCamera = new Camera(Vector2.Zero);
        }




        public virtual void Begin()
        {
            foreach (var entity in Entities)
                entity.OnSceneBegin(this);
        }

        public virtual void End()
        {
            foreach (var entity in Entities)
                entity.OnSceneEnd(this);
        }

        public virtual void BeforeUpdate()
        {
            if (!Paused)
                TimeActive += FeGame.DeltaTime;
        }
        
        
        

        public virtual void Update(float deltaTime)
        {
            if (Paused)
                return;
            
            foreach (Entity e in Entities)
            {
                if (!e.IsActive)
                    continue;
                e.Update(deltaTime);
            }

            MainCamera.Position += Vector2.One;
            MainCamera.Update();
            Space.Update();
        }

        public virtual void AfterUpdate()
        {
            
        }

        
        
        
        
        public virtual void BeforeRender()
        {
            // To be implemented by child classes
        }


        public virtual void AfterRender()
        {
            // To be implemented by child classes
        }



        public virtual void OnFocusGained()
        {
            // To be implemented by child classes
        }
        
        public virtual void OnFocusLost()
        {
            // To be implemented by child classes
        }
        
        
        
        

        #region // - - - - - Entity Management - - - - - //


        public void AddEntity(Entity entity)
        {
            entity.Scene = this;
            _entities.Add(entity);

            foreach (Collider col in entity.Colliders)
                Space.Add(col);
        }
        

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Remove(entity))
            {
                entity.Scene = null;
                foreach (Collider col in entity.Colliders)
                    Space.Add(col);
            }
        }

        public void AddEntities(params Entity[] entities)
        {
            foreach (var e in entities)
                AddEntity(e);
        }

        public void RemoveEntities(params Entity[] entities)
        {
            foreach (var e in entities)
                RemoveEntity(e);   
        }
        
        
        
        
        
        public T FindEntity<T>() where T : Entity
        {
            return (T) _entities.FirstOrDefault(e => e is T);
        }
        
        public Entity FindEntity(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            
            return _entities.FirstOrDefault(e => tag.Equals(e.Tag));
        }
        
        public T FindEntity<T>(string tag) where T : Entity
        {
            return (T) _entities.FirstOrDefault(e => tag.Equals(e.Tag) && e is T);
        }
        
        public T[] FindEntities<T>() where T : Entity
        {
            return (T[]) _entities
                .Where(e => e is T)
                .ToArray();
        }
        
        public Entity[] FindEntities(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            
            return _entities
                .Where(e => tag.Equals(e.Tag))
                .ToArray();
        }
        
        public T[] FindEntities<T>(string tag) where T : Entity
        {
            return (T[]) _entities
                .Where(e => tag.Equals(e.Tag) && e is T)
                .ToArray();
        }

        
        
        
        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        
    }
}
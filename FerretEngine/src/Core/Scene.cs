using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FerretEngine.Graphics;
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
        
        
        public bool Paused;
        


        public Scene()
        {
            _entities = new List<Entity>();

            BackgroundColor = Color.CornflowerBlue;
            MainCamera = new Camera(FeGame.Width, FeGame.Height);
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
                e.Update(deltaTime);
            }
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
        }
        

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Remove(entity))
                entity.Scene = null;
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
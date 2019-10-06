using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Core
{
	public abstract class Scene : IEnumerable<Entity>, IEnumerable
    {

        
        public float TimeActive{ get; private set; }


        public IEnumerable<Entity> Entities => _entities;
        private readonly List<Entity> _entities;
        
        
        
        public bool Paused;
        


        public Scene()
        {
            _entities = new List<Entity>();
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

        
        
        
        
        
        

        #region // - - - - - Entity Management - - - - - //


        public void Add(Entity entity)
        {
            _entities.Add(entity);
        }
        

        public void Remove(Entity entity)
        {
            _entities.Remove(entity);
        }

        public void Add(params Entity[] entities)
        {
            foreach (var e in entities)
            {
                _entities.Add(e);   
            }
        }

        public void Remove(params Entity[] entities)
        {
            foreach (var e in entities)
            {
                _entities.Remove(e);   
            }
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
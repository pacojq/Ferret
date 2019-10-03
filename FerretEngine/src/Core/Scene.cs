using System.Collections;
using System.Collections.Generic;

namespace FerretEngine.Core
{
	public abstract class Scene : IEnumerable<Entity>, IEnumerable
    {

        
        public float TimeActive{ get; private set; }
        public float RawTimeActive { get; private set; }
        public List<Entity> Entities { get; }
        
        
        
        public bool Paused;
        


        public Scene()
        {
            Entities = new List<Entity>();
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
                TimeActive += FerretGame.DeltaTime;
            RawTimeActive += FerretGame.RawDeltaTime;
        }
        
        
        

        public virtual void Update()
        {
            if (Paused)
                return;
        
            foreach (Entity e in Entities)
            {
                e.Update();
            }
        }

        public virtual void AfterUpdate()
        {
            
        }

        public virtual void BeforeRender()
        {
            
        }

        public virtual void Render()
        {
            
        }

        public virtual void AfterRender()
        {
            
        }

        
        
        

        #region // - - - - - Entity Management - - - - - //


        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }
        

        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void Add(params Entity[] entities)
        {
            foreach (var e in entities)
            {
                Entities.Add(e);   
            }
        }

        public void Remove(params Entity[] entities)
        {
            foreach (var e in entities)
            {
                Entities.Remove(e);   
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
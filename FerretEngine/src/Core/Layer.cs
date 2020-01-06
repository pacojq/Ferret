using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FerretEngine.Components;
using FerretEngine.Logging;
using FerretEngine.Physics;

namespace FerretEngine.Core
{
    public class Layer : IEnumerable<Entity>, IEnumerable
    {
        internal static int CreationIdCount = 0;

        public string Id { get; }

        public Scene Scene { get; }

        /// <summary>
        /// Used to check if a Layer was created before another.
        /// </summary>
        internal int CreationId { get; }



        /// <summary>
        /// Whether the Layer will update.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Whether the Layer will render.
        /// </summary>
        public bool IsVisible { get; set; }


        public int Depth
        {
            get => _depth;
            set
            {
                _depth = value;
                Scene.SortLayers();
            }
        }

        private int _depth;


        public IEnumerable<Entity> Entities => _entities;
        private readonly List<Entity> _entities;
        
        private readonly Queue<Entity> _createQueue;
        private readonly Queue<Entity> _destroyQueue;

        internal Layer(string id, Scene scene)
        {
            Id = id;
            CreationId = CreationIdCount++;
            Scene = scene;

            IsEnabled = true;
            IsVisible = true;
            
            _entities = new List<Entity>();
            
            _createQueue= new Queue<Entity>();
            _destroyQueue = new Queue<Entity>();
        }

        private void UpdateQueues()
        {
            while (_createQueue.Count > 0)
                AddEntity(_createQueue.Dequeue());
            
            while (_destroyQueue.Count > 0)
                RemoveEntity(_destroyQueue.Dequeue());
        }
        
        
        
        /// <summary>
        /// Called when the Scene starts after calling <see cref="FeGame.SetScene"/>.
        /// </summary>
        internal virtual void Begin()
        {
            foreach (var entity in Entities)
                entity.OnSceneBegin(Scene);
        }

        /// <summary>
        /// Called when the Scene ends and a new Scene begins.
        /// </summary>
        internal virtual void End()
        {
            foreach (var entity in Entities)
                entity.OnSceneEnd(Scene);
        }
        
        
        
        
        internal virtual void BeforeUpdate()
        {
            if (!IsEnabled)
                return;
            
            UpdateQueues();
        }
        

        internal virtual void Update(float deltaTime)
        {
            if (!IsEnabled)
                return;
            
            foreach (Entity e in Entities)
            {
                if (!e.IsActive)
                    continue;
                e.Update(deltaTime);
            }
        }
        
        
        internal void AfterUpdate()
        {
            if (!IsEnabled)
                return;
            
            UpdateQueues();
        }
        
        
        
        
        
        
        
        internal void Create(Entity entity)
        {
            _createQueue.Enqueue(entity);
        }
        
        private void AddEntity(Entity entity)
        {
            entity.Scene = Scene;
            entity.Layer = this;
            
            _entities.Add(entity);

            foreach (Collider col in entity.Colliders)
                Scene.Space.Add(col);
        }
        
        
        
        internal void Destroy(Entity entity)
        {
            _destroyQueue.Enqueue(entity);
        }
        
        private void RemoveEntity(Entity entity)
        {
            if (_entities.Remove(entity))
            {
                entity.Scene = null;
                foreach (Collider col in entity.Colliders)
                    Scene.Space.Remove(col);
                
                FeLog.FerretDebug("Entity destroyed");
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
    }
}
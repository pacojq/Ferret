using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FerretEngine.Components;
using FerretEngine.Graphics;
using FerretEngine.Logging;
using FerretEngine.Physics;
using Microsoft.Xna.Framework;
using NLog.Fluent;

namespace FerretEngine.Core
{
	public abstract class Scene : IEnumerable<Entity>, IEnumerable
    {

        public float TimeActive{ get; private set; }


        public Color BackgroundColor { get; set; }


        
        public Camera MainCamera { get; set; }
        
        
        
        public Layer DefaultLayer { get; }


        public IEnumerable<Layer> Layers => _layers;
        private readonly List<Layer> _layers;

        private readonly Queue<Layer> _createQueue;

        public IEnumerable<Entity> Entities
        {
            get
            {
                foreach (Layer layer in _layers)
                    foreach (Entity e in layer.Entities)
                        yield return e;
            }
        }
        
        
        /// <summary>
        /// The physics space of the scene
        /// </summary>
        internal Space Space { get; }

        public Vector2 Gravity
        {
            get => this.Space.Gravity;
            set => this.Space.Gravity = value;
        }
        
        
        public bool Paused { get; set; }
        


        public Scene()
        {
            Space = new Space();
            
            _createQueue = new Queue<Layer>();
            
            _layers = new List<Layer>();
            DefaultLayer = CreateLayer("default");

            
            BackgroundColor = Color.CornflowerBlue;
            MainCamera = new Camera(Vector2.Zero);
        }


        public Layer CreateLayer(string id)
        {
            Layer layer = new Layer(id, this);
            _createQueue.Enqueue(layer);
            return layer;
        }

        /// <summary>
        /// Actually add a Layer dequeued from <see cref="_createQueue"/>.
        /// </summary>
        /// <param name="layer"></param>
        private void AddLayer(Layer layer)
        {
            _layers.Add(layer);
        }


        
        internal void SortLayers()
        {
            _layers.Sort((l1, l2) =>
            {
                if (l1.Depth == l2.Depth)
                    return l1.CreationId.CompareTo(l2.CreationId);
                return l1.Depth.CompareTo(l2.Depth);
            });
        }
        
        
        
        

        /// <summary>
        /// Called when the Scene starts after calling <see cref="FeGame.SetScene"/>.
        /// </summary>
        public virtual void Begin()
        {
            foreach (Layer layer in _layers)
                layer.Begin();
        }

        /// <summary>
        /// Called when the Scene ends and a new Scene begins.
        /// </summary>
        public virtual void End()
        {
            foreach (Layer layer in _layers)
                layer.End();
        }

        
        
        
        
        
        
        
        protected internal virtual void BeforeUpdate()
        {
            if (!Paused)
                TimeActive += FeGame.DeltaTime;
            
            while (_createQueue.Count > 0)
                AddLayer(_createQueue.Dequeue());
            
            foreach (Layer layer in _layers)
                layer.BeforeUpdate();
        }
        

        protected internal virtual void Update(float deltaTime)
        {
            if (Paused)
                return;
            
            foreach (Layer layer in _layers)
                layer.Update(deltaTime);

            MainCamera.Update();
            Space.Update();
        }

        protected internal virtual void AfterUpdate()
        {
            foreach (Layer layer in _layers)
                layer.AfterUpdate();
        }

        
        
        
        
        protected internal virtual void BeforeRender()
        {
            // To be implemented by child classes
        }


        protected internal virtual void AfterRender()
        {
            // To be implemented by child classes
        }



        protected internal virtual void OnFocusGained()
        {
            // To be implemented by child classes
        }
        
        protected internal virtual void OnFocusLost()
        {
            // To be implemented by child classes
        }
        
        
        
        

        #region // - - - - - Entity Management - - - - - //

        public void Create(Entity entity, string layerId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            Layer layer = _layers.FirstOrDefault(l => l.Id.Equals(layerId));
            if (layer == null)
            {
                FeLog.Warning($"Could not find layer with id '{layerId}'. Creating entity in default layer instead.");
                layer = DefaultLayer;
            }
            Create(entity, layer);
        }
        
        public void Create(Entity entity, Layer layer)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            if (layer == null)
                throw new ArgumentNullException(nameof(layer));
            
            layer.Create(entity);
        }

        public void Create(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            DefaultLayer.Create(entity);
        }
        
        public void Destroy(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.Layer.Destroy(entity);
        }

        
        
        
        
        
        
        
        
        public T FindEntity<T>() where T : Entity
        {
            return (T) Entities.FirstOrDefault(e => e is T);
        }
        
        public Entity FindEntity(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            
            return Entities.FirstOrDefault(e => tag.Equals(e.Tag));
        }
        
        public T FindEntity<T>(string tag) where T : Entity
        {
            return (T) Entities.FirstOrDefault(e => tag.Equals(e.Tag) && e is T);
        }
        
        public T[] FindEntities<T>() where T : Entity
        {
            return Entities
                .OfType<T>()
                .ToArray();
        }
        
        public Entity[] FindEntities(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            
            return Entities
                .Where(e => tag.Equals(e.Tag))
                .ToArray();
        }
        
        public T[] FindEntities<T>(string tag) where T : Entity
        {
            return (T[]) Entities
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
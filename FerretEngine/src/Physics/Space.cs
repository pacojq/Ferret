using System;
using System.Collections.Generic;
using FerretEngine.Components;
using FerretEngine.Core;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;

namespace FerretEngine.Physics
{
    internal class Space
    {

        public Vector2 Gravity
        {
            get => _gravity;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _gravity = value;
            }
        }
        private Vector2 _gravity;
        
        
        
        private readonly List<Entity> _entities;
        private readonly List<int> _colliderCount;
        
        
        public Space()
        {
            _gravity = Vector2.Zero;
            _entities = new List<Entity>();
            _colliderCount = new List<int>();
        }


        public void Update()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity entity = _entities[i];
                if (!entity.IsActive || !entity.IsCollidable)
                    continue;
                
                // Diagonal check
                for (int j = i + 1; j < _entities.Count; j++)
                {
                    Entity other = _entities[j];
                    if (!other.IsActive || !other.IsCollidable)
                        continue;
                    
                    UpdateEntities(entity, other);
                }
            }

            // After update
            foreach (Entity entity in _entities)
            {
                if (!entity.IsActive || !entity.IsCollidable)
                    continue;

                foreach (Collider collider in entity.Colliders)
                    collider.AfterPhysicsUpdate();
            }
        }

        private void UpdateEntities(Entity entity, Entity other)
        {
            foreach (Collider a in entity.Colliders)
            {
                foreach (Collider b in other.Colliders)
                {
                    if (a.Accept(b))
                    {
                        a.Collide(b);
                        b.Collide(a);
                    }
                }
            }
        }



        public void DebugDraw(float deltaTime)
        {
            foreach (Entity entity in _entities)
            {
                if (!entity.IsActive || !entity.IsCollidable)
                    continue;

                foreach (Collider collider in entity.Colliders)
                    collider.DebugDraw(deltaTime);
            }
        }
        
        


        public void Add(Collider collider)
        {
            FeLog.Debug($"Adding collider to space: {collider}");
            int index = _entities.IndexOf(collider.Entity);
            if (index < 0)
            {
                index = _entities.Count;
                _entities.Add(collider.Entity);
                _colliderCount.Add(0);
            }

            _colliderCount[index] = _colliderCount[index] + 1;
        }

        public void Remove(Collider collider)
        {
            int index = _entities.IndexOf(collider.Entity);
            if (index < 0)
                return;
            
            _colliderCount[index] = _colliderCount[index] - 1;
            if (_colliderCount[index] <= 0)
            {
                _colliderCount.RemoveAt(index);
                _entities.RemoveAt(index);
            }
        }
    }
}
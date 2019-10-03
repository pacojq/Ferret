using System;
using Microsoft.Xna.Framework;

namespace FerretEngine.Core
{
	public abstract class Component
	{

		private Entity _entity;
		public Entity Entity
		{
			get => _entity;
			set
			{
				if (_entity != null)
					throw new InvalidOperationException("Cannot set a new entity to a bound component.");
				_entity = value;
			}
		}


		public Vector2 LocalPosition = Vector2.Zero;
		
		public Vector2 Position
		{
			get
			{
				if (Entity != null)
					return Entity.Position + LocalPosition;
				return LocalPosition;
			}
		}



		public virtual void OnBinding(Entity entity)
		{
			// To be implemented by each individual component
		}
		
		public virtual void OnUnbinding(Entity entity)
		{
			// To be implemented by each individual component
		}


		public virtual void Update()
		{
			// To be implemented by each individual component
		}
	}
}
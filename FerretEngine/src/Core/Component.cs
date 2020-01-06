using System;
using FerretEngine.Graphics;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Core
{
	public abstract class Component
	{

		private Entity _entity;
		public Entity Entity => _entity;

		/// <summary>
		/// A tag that identifies the componen.
		/// If not set, it will return null.
		/// </summary>
		public string Tag { get; set; }
		

		/// <summary>
		/// Position of the component relative to its <see cref="Entity"/>.
		/// </summary>
		public Vector2 LocalPosition = Vector2.Zero;
		
		
		/// <summary>
		/// World position of the component.
		/// </summary>
		public Vector2 Position
		{
			get
			{
				Assert.IsNotNull(Entity, "Cannot calculate a Component position when its Entity is null.");
				if (Entity != null)
				{
					Vector2 offset = Vector2.Zero;
					
					offset.X = FeMath.Cos(Entity.Rotation) * LocalPosition.X +
					           FeMath.Sin(Entity.Rotation) * LocalPosition.Y;
					offset.Y = FeMath.Cos(Entity.Rotation) * LocalPosition.Y +
					           FeMath.Sin(Entity.Rotation) * LocalPosition.X;
					
					return Entity.Position + offset;
				}
				return LocalPosition;
			}
		}



		internal void Bind(Entity entity)
		{
			Assert.IsNotNull(entity);
			Assert.IsNull(_entity);

			OnBinding(entity);
			_entity = entity;
		}

		internal void Unbind()
		{
			OnUnbinding(_entity);
			_entity = null;
		}
		
		
		protected virtual void OnBinding(Entity entity)
		{
			// To be implemented by each individual component
		}
		
		protected virtual void OnUnbinding(Entity entity)
		{
			// To be implemented by each individual component
		}


		
		/// <summary>
		/// Update game logic.
		/// Don't perform any rendering calls here.
		/// This method will be skipped if the <see cref="Entity"/> is not active.
		/// </summary>
		/// <param name="deltaTime">The time in seconds since the last update</param>
		public virtual void Update(float deltaTime)
		{
			// To be implemented by each individual component
		}

		public virtual void Draw(float deltaTime)
		{
			// To be implemented by each individual component
		}
		
		public virtual void DrawGUI(float deltaTime)
		{
			// To be implemented by each individual component
		}
	}
}
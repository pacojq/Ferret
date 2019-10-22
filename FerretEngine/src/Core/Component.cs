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


		public Vector2 LocalPosition = Vector2.Zero;
		
		public Vector2 Position
		{
			get
			{
				Assert.IsNotNull(Entity, "La chingamos");
				if (Entity != null)
					return Entity.Position + LocalPosition;
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
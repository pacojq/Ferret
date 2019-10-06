using FerretEngine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public abstract class Renderer
    {
        public bool Visible = true;
        public virtual SpriteSortMode SortMode => SpriteSortMode.Deferred;


        public Camera Camera { get; internal set; }
        
        
        public abstract void Render(Scene scene, float deltaTime);

        public virtual void BeforeRender(Scene scene)
        {
            
        }

        public virtual void AfterRender(Scene scene)
        {
            
        }
    }
}
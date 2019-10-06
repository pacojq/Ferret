using FerretEngine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Renderers
{
    public abstract class Renderer
    {
        public bool Visible = true;
        
        public SpriteSortMode SortMode {get; set; }
        public RenderSurface Surface {get; }

        
        
        public Renderer(RenderSurface surface, SpriteSortMode sortMode)
        {
            SortMode = sortMode;
            Surface = surface;
        }
        


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
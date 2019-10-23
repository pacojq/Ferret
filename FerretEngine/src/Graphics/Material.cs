using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public class Material
    {
        
        public static readonly Material Default = new Material();
        
        
        
        public Effect Effect { get; set; }
        
        
        public Material()
        {
        }

        public Material(Effect effect)
        {
            Effect = effect;
        }

        public Material(string effect)
        {
            Effect = FeGame.Instance.Content.Load<Effect>(effect);
        }
    }
}
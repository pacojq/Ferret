using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Effects
{
    public class Shader
    {
        public string Id { get; }
        internal Effect Effect { get; }

        internal Shader(string id, Effect effect)
        {
            Id = id;
            Effect = effect;
        }
    }
}
using FerretEngine.Components;
using FerretEngine.Core;
using FerretEngine.Graphics;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            Sprite sprite = FeGraphics.LoadSprite("char01");
            Bind(new SpriteRenderer(sprite));
            
            Bind(new PlayerComponent());
        }
    }
}
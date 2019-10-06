using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public class SpriteSheet
    {
        public Texture2D Texture { get; }
        
        
        public int SpriteWidth { get; }
        public int SpriteHeight { get; }
        
        
        /// <summary>
        /// The amount of pixels between two sprites
        /// in the same row.
        /// </summary>
        public int OffsetX { get; }
        
        /// <summary>
        /// The amount of pixels between two sprites
        /// in the same column.
        /// </summary>
        public int OffsetY { get; }
        
        /// <summary>
        /// The number of pixels between the left border
        /// of the texture and the first sprite.
        /// </summary>
        public int BeginX { get; }
        
        /// <summary>
        /// The number of pixels between the top border
        /// of the texture and the first sprite.
        /// </summary>
        public int BeginY { get; }


        /// <summary>
        /// The number of rows in the SpriteSheet
        /// </summary>
        public int Rows => (_textureHeight - BeginY + OffsetY) / (SpriteHeight + OffsetY);
        
        /// <summary>
        /// The number of columns in the SpriteSheet
        /// </summary>
        public int Columns => (_textureWidth - BeginX + OffsetX) / (SpriteHeight + OffsetX);


        private readonly int _textureWidth;
        private readonly int _textureHeight;

        
        
        
        public SpriteSheet(Texture2D texture, int spriteWidth, int spriteHeight, 
                int offsetX, int offsetY, int beginX, int beginY)
        {
            Texture = texture;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            OffsetX = offsetX;
            OffsetY = offsetY;
            BeginX = beginX;
            BeginY = beginY;
            
            _textureWidth = texture.Width;
            _textureHeight = texture.Height;
        }
        
        
        // = = = = = = = Util Constructors = = = = = = = //

        
        public SpriteSheet(Texture2D texture, int spriteWidth, int spriteHeight)
            : this(texture, spriteWidth, spriteHeight, 0, 0, 0, 0)
        {
        }
        
        public SpriteSheet(Texture2D texture, int spriteWidth, int spriteHeight, int offsetX, int offsetY)
            : this(texture, spriteWidth, spriteHeight, offsetX, offsetY, 0, 0)
        {
        }





        /// <summary>
        /// Returns the sprite in the [i, j] position in the
        /// SpriteSheet rows and columns.
        /// </summary>
        /// <param name="i">Row index</param>
        /// <param name="j">Column index</param>
        /// <returns></returns>
        public Sprite GetSprite(int i, int j)
        {
            int x = BeginX + (SpriteWidth + OffsetX) * i;
            int y = BeginY + (SpriteHeight + OffsetY) * j;
            
            return new Sprite(Texture, new Rectangle(x, y, SpriteWidth, SpriteHeight));
        }
    }
}
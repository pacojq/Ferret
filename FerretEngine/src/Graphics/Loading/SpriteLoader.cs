using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace FerretEngine.Graphics.Loading
{
    public static class SpriteLoader
    {
        /*
         *
         * 
         *
         * 
         */
        private struct SpriteSheetDto
        {
            public SpriteDto[] Sprites;
        }
        private struct SpriteDto
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public int OriginX;
            public int OriginY;
            public string FileName;
        }
        
        public static Sprite[] LoadSprites(string path)
        {
            path = Path.Combine(FeGame.ContentDirectory, path);
            
            if (! ".feAsset".Equals(Path.GetExtension(path)))
                throw new ArgumentException("Only .feAsset files can be loaded.");
            
            string file = File.ReadAllText(path);
            SpriteSheetDto spriteSheet = JsonConvert.DeserializeObject<SpriteSheetDto>(file);

            return spriteSheet.Sprites
                .Select(s =>
                {
                    string sprPath = Path.Combine( Path.GetDirectoryName(path), s.FileName);
                    var fileStream = new FileStream(sprPath, FileMode.Open, FileAccess.Read);
                    Texture2D texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
                    
                    Rectangle clip = new Rectangle(s.X, s.Y, s.Width, s.Height);
                    Vector2 origin = new Vector2(s.OriginX, s.OriginY);
                    
                    return new Sprite(texture, clip, origin);
                })
                .ToArray();
        }
    }
}
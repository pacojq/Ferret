using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FerretEngine.Content.Dto;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Fonts;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace FerretEngine.Content
{
    public static class FeContent
    {

        private static string FileNotFound(string path)
        {
            return $"Failed to find file at: {path}. Did you add it to the Content folder and copied it to the output directory?";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetPathInContent(string path)
        {
            return Path.Combine(FeGame.ContentDirectory, path);
        }
        
        
        internal static string CheckFilename(string path, string extension)
        {
            path = Path.Combine(FeGame.ContentDirectory, path);
            string found = Path.GetExtension(path);
            
            if (!extension.Equals(found))
                throw new ArgumentException($"Invalid file extension. Expected: {extension} Found: {found}");

            if (!File.Exists(path))
                throw new FileNotFoundException(FileNotFound(path));
            
            return path;
        }
        
        private static byte[] GetFileResourceBytes(string path)
        {
            byte[] bytes;
            try
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    if (stream.CanSeek)
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(FileNotFound(path), e);
            }
            return bytes;
        }






        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to a TTF font</param>
        /// <param name="size">Size of the font</param>
        public static Font LoadFont(string path, int size, char defaultChar = '_')
        {
            // TODO try/catch FileNotFoundException and look for the font in the system
            
            path = CheckFilename(path, ".ttf");
            FontLibrary lib = new FontLibrary(File.OpenRead(path), FeGame.Instance.GraphicsDevice);
            Font fnt = lib.CreateFont(size);
            fnt.DefaultCharacter = defaultChar;
            fnt.PreheatCache("asdfghjklñqwertyuiopzxcvbnm");
            return fnt;
        }
        
        
        /// <summary>
        /// Loads a raw PNG image from the Content directory as a <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string path)
        {
            Texture2D texture;
            path = CheckFilename(path, ".png");
            
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
            }
            return texture;
        }
        
        /// <summary>
        /// Loads a raw PNG image from the Content directory as a Sprite.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string path)
        {
            Texture2D texture;
            path = CheckFilename(path, ".png");
            
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
            }
            return new Sprite(texture);
        }


        public static Effect LoadEffect(string path)
        {
            try
            {
                path = CheckFilename(path, ".fxb");
            }
            catch (Exception e)
            {
                FeLog.FerretError($"Loading effect '{path}' resulted on an exception:\n{e}");
                path = CheckFilename("Ferret/Effects/error.fxb", ".fxb");
            }

            byte[] bytes = GetFileResourceBytes(path);
            return new Effect(FeGame.Instance.GraphicsDevice, bytes);
        }
        
        public static Sprite[] LoadSpriteSheet(string path)
        {
            path = CheckFilename(path, ".feAsset");
            
            string file = File.ReadAllText(path);
            SpriteSheetDto spriteSheet = JsonConvert.DeserializeObject<SpriteSheetDto>(file);

            return spriteSheet.Sprites
                .Select(s =>
                {
                    string sprPath = Path.Combine( Path.GetDirectoryName(path), s.FileName);
                    var fileStream = new FileStream(sprPath, FileMode.Open, FileAccess.Read);
                    Texture2D texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
                    fileStream.Close();
                    
                    Rectangle clip = new Rectangle(s.X, s.Y, s.Width, s.Height);
                    Vector2 origin = new Vector2(s.OriginX, s.OriginY);
                    
                    return new Sprite(texture, clip, origin);
                })
                .ToArray();
        }
        
        
        
    }
}
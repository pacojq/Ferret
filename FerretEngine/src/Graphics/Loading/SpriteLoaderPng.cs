using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Loading
{
    internal static class SpriteLoaderPng
    {
        public static Sprite LoadSprite(string path)
        {
            string texPath = Path.Combine(FeGame.ContentDirectory, path) + ".png";
            var fileStream = new FileStream(texPath, FileMode.Open, FileAccess.Read);
            Texture2D texture = Texture2D.FromStream(FeGame.Instance.GraphicsDevice, fileStream);
            fileStream.Close();
            return new Sprite(texture);
        }
    }
}
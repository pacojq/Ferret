using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace FerretEngine.Graphics.Fonts
{
    internal class FontLibrary
    {
        public static Library Lib = new Library();
        
        private readonly Dictionary<int, Font> fonts = new Dictionary<int, Font>();
        private readonly GraphicsDevice GraphicsDevice;
        private readonly byte[] fontBytes;
        
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fontStream">A stream to the TTF/OTF file that will be used. The stream will be copied internally but will NOT be disposed.</param>
        /// <param name="graphicsDevice">XNA GraphicsDevice which will be used to create textures.</param>
        public FontLibrary(Stream fontStream, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            using (var ms = new MemoryStream())
            {
                fontStream.CopyTo(ms);
                fontBytes = ms.ToArray();
            }
        }
        
        
        /// <summary>
        /// Create an instance of a Font with the given size.
        /// If called multiple times with the same size, the same Font instance will always be returned.
        /// </summary>
        /// <param name="size">Font size</param>
        /// <returns>The created Font</returns>
        public Font CreateFont(int size)
        {
            if (!fonts.TryGetValue(size, out var font))
                font = new Font(size, fontBytes, GraphicsDevice);

            return font;
        }

        /// <summary>Disposes all Fonts tracked by this FontLibrary.</summary>
        public void Dispose()
        {
            foreach (var font in fonts.Values)
                font.Dispose();

            // TODO fix possible memory leaks due to not calling Lib.Dispose in FeContent
            Lib.Dispose();
        }
    }
}
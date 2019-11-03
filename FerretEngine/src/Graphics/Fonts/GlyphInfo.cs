using SharpFont;

namespace FerretEngine.Graphics.Fonts
{
    internal struct GlyphInfo
    {
        public uint Index;
        public int Width;
        public int Height;
        public int Advance;
        public int BearingX;
        public int BearingY;
        public char Character;
        public byte[] BufferData;
        public int BitmapLeft;

        public GlyphInfo(char c, uint index, int bitmapLeft, GlyphMetrics metrics, byte[] bufferData)
        {
            BitmapLeft = bitmapLeft;
            Index = index;
            Character = c;
            Width = (int)metrics.Width;
            Height = (int)metrics.Height;
            Advance = (int)metrics.HorizontalAdvance;
            BearingX = (int)metrics.HorizontalBearingX;
            BearingY = (int)metrics.HorizontalBearingY;
            BufferData = bufferData;
        }
    }
}
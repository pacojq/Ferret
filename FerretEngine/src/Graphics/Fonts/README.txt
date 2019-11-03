Huge shout out to Jake Albano https://bitbucket.org/jacobalbano/fnt/src/default/

Usage:

var font = new FontLibrary(File.OpenRead("path/to/font.ttf"), GraphicsDevice);
var fontFace = font.CreateFont(64);
var text = font.MakeText("Hello, world!");
var bounds = new Rectangle(0, 0, text.Width, text.Height);

sb.RenderString(text, Vector2.Zero, Color.White);
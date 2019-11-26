using System.Text;
using FerretEngine.Logging;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Effects
{
    // Super helpful website: http://rbwhitaker.wikidot.com/shaders-in-xna
    
    /// <summary>
    /// Our shader library.
    /// </summary>
    public static class FeShaders
    {
        internal static void LoadContent()
        {
            // TODO load default and error shaders
        }

        public static void SetGlobalInt(string id, int value)
        {
            
        }








        /// <summary>
        /// Prepare a Shader to submit for rendering, setting all
        /// needed properties in its Effect.
        /// </summary>
        /// <param name="shader"></param>
        internal static void PrepareShader(Shader shader)
        {
            foreach (EffectParameter p in shader.Effect.Parameters)
            {
                switch (p.ParameterType)
                {
                    case EffectParameterType.Texture2D:
                        break;
                    
                    case EffectParameterType.Bool:
                        break;
                    
                    case EffectParameterType.Int32:
                        break;

                    case EffectParameterType.Single:
                        {
                            if (p.ColumnCount == 2)
                            {
                                // TODO
                            }
                        }
                        break;
                }
                StringBuilder str = new StringBuilder();
                str.Append($"Name = {p.Name} ");
                str.Append($"Type = {p.ParameterType} ");
                str.Append($"Class = {p.ParameterClass} ");
                str.Append($"ColumnCount = {p.ColumnCount} ");
                FeLog.Warning(str.ToString());
            }
        }

        private static void PrepareInt(EffectParameter p)
        {
            
        }
        
        
        
    }
}
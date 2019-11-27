using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        
        
        
        
        
        
        
        // =========================== STATIC INTERFACE =========================== //
        
        private static readonly Dictionary<string, int> _gInt = new Dictionary<string, int>();
        private static readonly Dictionary<string, bool> _gBool = new Dictionary<string, bool>();
        private static readonly Dictionary<string, float> _gFloat = new Dictionary<string, float>();
        private static readonly Dictionary<string, Vector2> _gVec2 = new Dictionary<string, Vector2>();
        private static readonly Dictionary<string, Vector3> _gVec3 = new Dictionary<string, Vector3>();
        private static readonly Dictionary<string, Vector4> _gVec4 = new Dictionary<string, Vector4>();
        private static readonly Dictionary<string, Texture2D> _gTex = new Dictionary<string, Texture2D>();
        
        public static void SetGlobalInt(string name, int value)
        {
            if (!_gInt.ContainsKey(name))
                _gInt.Add(name, 0);
            _gInt[name] = value;
        }
        
        public static bool GetGlobalInt(string name, ref int value)
        {
            if (_gInt.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public static void SetGlobalBool(string name, bool value)
        {
            if (!_gBool.ContainsKey(name))
                _gBool.Add(name, false);
            _gBool[name] = value;
        }
        
        public static bool GetGlobalBool(string name, ref bool value)
        {
            if (_gBool.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        public static void SetGlobalFloat(string name, float value)
        {
            if (!_gFloat.ContainsKey(name))
                _gFloat.Add(name, 0);
            _gFloat[name] = value;
        }
        
        public static bool GetGlobalFloat(string name, ref float value)
        {
            if (_gFloat.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public static void SetGlobalVector2(string name, Vector2 value)
        {
            if (!_gVec2.ContainsKey(name))
                _gVec2.Add(name, Vector2.Zero);
            _gVec2[name] = value;
        }
        
        public static bool GetGlobalVector2(string name, ref Vector2 value)
        {
            if (_gVec2.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public static void SetGlobalVector3(string name, Vector3 value)
        {
            if (!_gVec3.ContainsKey(name))
                _gVec3.Add(name, Vector3.Zero);
            _gVec3[name] = value;
        }
        
        public static bool GetGlobalVector3(string name, ref Vector3 value)
        {
            if (_gVec3.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public static void SetGlobalVector4(string name, Vector4 value)
        {
            if (!_gVec4.ContainsKey(name))
                _gVec4.Add(name, Vector4.Zero);
            _gVec4[name] = value;
        }
        
        public static bool GetGlobalVector4(string name, ref Vector4 value)
        {
            if (_gVec4.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }

        public static void SetGlobalColor(string name, Color value)
        {
            if (!_gVec4.ContainsKey(name))
                _gVec4.Add(name, Vector4.Zero);
            _gVec4[name] = value.ToVector4();
        }
        
        public static bool GetGlobalColor(string name, ref Color value)
        {
            if (_gVec4.TryGetValue(name, out var result))
            {
                value = new Color(result);
                return true;
            }
            return false;
        }

        public static void SetGlobalTexture(string name, Texture2D value)
        {
            if (!_gTex.ContainsKey(name))
                _gTex.Add(name, null);
            _gTex[name] = value;
        }
        
        public static bool GetGlobalTexture(string name, ref Texture2D value)
        {
            if (_gTex.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
    }
}
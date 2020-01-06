using System;
using System.Collections.Generic;
using System.Text;
using FerretEngine.Content;
using FerretEngine.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Effects
{
    public class Material
    {
        public static readonly Material Default = new Material();
        
        
        // TODO change to Shader
        public Effect Effect { get; set; }

        private readonly Dictionary<string, int> _intProps;
        private readonly Dictionary<string, bool> _boolProps;
        private readonly Dictionary<string, float> _floatProps;
        private readonly Dictionary<string, Vector2> _vec2Props;
        private readonly Dictionary<string, Vector3> _vec3Props;
        private readonly Dictionary<string, Vector4> _vec4Props;
        private readonly Dictionary<string, Texture2D> _texProps;

        private bool _isDirty = false;
        
        public Material()
        {
            _intProps = new Dictionary<string, int>();
            _boolProps = new Dictionary<string, bool>();
            _floatProps = new Dictionary<string, float>();
            _vec2Props = new Dictionary<string, Vector2>();
            _vec3Props = new Dictionary<string, Vector3>();
            _vec4Props = new Dictionary<string, Vector4>();
            _texProps = new Dictionary<string, Texture2D>();
        }

        public Material(Effect effect) : this()
        {
            Effect = effect;
        }

        public Material(string effectPath) : this()
        {
            Effect = FeContent.LoadEffect(effectPath);
        }


        public bool AreEqual(Material other)
        {
            if (other == null)
                return false;

            if (this.Effect == null)
                return other.Effect == null;
            
            return Effect.Equals(other.Effect);
        }


        /// <summary>
        /// Used before each render call.
        /// </summary>
        internal void Bind()
        {
            if (Effect == null || !_isDirty)
                return;
            
            _isDirty = false;
            
            foreach (EffectParameter p in Effect.Parameters)
            {
                switch (p.ParameterType)
                {
                    case EffectParameterType.Texture:
                    case EffectParameterType.Texture2D:
                        BindTexture(p);
                        break;
                    
                    case EffectParameterType.Bool:
                        BindBool(p);
                        break;
                    
                    case EffectParameterType.Int32:
                        BindInt(p);
                        break;

                    case EffectParameterType.Single:
                        if (p.ParameterClass == EffectParameterClass.Vector)
                        {
                            if (p.ColumnCount == 2)
                                BindVector2(p);
                            else if (p.ColumnCount == 3)
                                BindVector3(p);
                            else if (p.ColumnCount == 4)
                                BindVector4(p);
                            else goto default;
                        }
                        else if (p.ParameterClass == EffectParameterClass.Scalar)
                            BindFloat(p);
                        
                        // TODO matrices, etc
                        // else UnknownEffectParameter(p);
                        break;
                    
                    default:
                        UnknownEffectParameter(p);
                        break;
                }
            }
        }

        private void UnknownEffectParameter(EffectParameter p)
        {
            StringBuilder str = new StringBuilder();
            str.Append("UNKNOWN EFFECT PARAMETER: ");
            str.Append($"Name={p.Name} | ");
            str.Append($"Type={p.ParameterType} | ");
            str.Append($"Class={p.ParameterClass} | ");
            str.Append($"ColumnCount={p.ColumnCount}");
            
            throw new Exception(str.ToString());
        }

        
        
        private void BindInt(EffectParameter p)
        {
            int value = 0;
            if(GetInt(p.Name, ref value) || Shader.GetGlobalInt(p.Name, ref value))
                p.SetValue(value);
        }

        private void BindBool(EffectParameter p)
        {
            bool value = false;
            if(GetBool(p.Name, ref value) || Shader.GetGlobalBool(p.Name, ref value))
                p.SetValue(value);
        }
        
        private void BindFloat(EffectParameter p)
        {
            float value = 0;
            if(GetFloat(p.Name, ref value) || Shader.GetGlobalFloat(p.Name, ref value))
                p.SetValue(value);
        }
        
        private void BindVector2(EffectParameter p)
        {
            Vector2 value = Vector2.Zero;
            if(GetVector2(p.Name, ref value) || Shader.GetGlobalVector2(p.Name, ref value))
                p.SetValue(value);
        }
        
        private void BindVector3(EffectParameter p)
        {
            Vector3 value = Vector3.Zero;
            if(GetVector3(p.Name, ref value) || Shader.GetGlobalVector3(p.Name, ref value))
                p.SetValue(value);
        }
        
        private void BindVector4(EffectParameter p)
        {
            Vector4 value = Vector4.Zero;
            if(GetVector4(p.Name, ref value) || Shader.GetGlobalVector4(p.Name, ref value))
                p.SetValue(value);
        }
        
        private void BindTexture(EffectParameter p)
        {
            Texture2D value = null;
            if(GetTexture(p.Name, ref value) || Shader.GetGlobalTexture(p.Name, ref value))
                p.SetValue(value);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        public void SetInt(string name, int value)
        {
            if (!_intProps.ContainsKey(name))
                _intProps.Add(name, 0);
            _intProps[name] = value;
            _isDirty = true;
        }
        
        public bool GetInt(string name, ref int value)
        {
            if (_intProps.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public void SetBool(string name, bool value)
        {
            if (!_boolProps.ContainsKey(name))
                _boolProps.Add(name, false);
            _boolProps[name] = value;
            _isDirty = true;
        }
        
        public bool GetBool(string name, ref bool value)
        {
            if (_boolProps.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public void SetFloat(string name, float value)
        {
            if (!_floatProps.ContainsKey(name))
                _floatProps.Add(name, 0);
            _floatProps[name] = value;
            _isDirty = true;
        }
        
        public bool GetFloat(string name, ref float value)
        {
            if (_floatProps.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public void SetVector2(string name, Vector2 value)
        {
            if (!_vec2Props.ContainsKey(name))
                _vec2Props.Add(name, Vector2.Zero);
            _vec2Props[name] = value;
            _isDirty = true;
        }
        
        public bool GetVector2(string name, ref Vector2 value)
        {
            if (_vec2Props.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public void SetVector3(string name, Vector3 value)
        {
            if (!_vec3Props.ContainsKey(name))
                _vec3Props.Add(name, Vector3.Zero);
            _vec3Props[name] = value;
            _isDirty = true;
        }
        
        public bool GetVector3(string name, ref Vector3 value)
        {
            if (_vec3Props.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
        public void SetVector4(string name, Vector4 value)
        {
            if (!_vec4Props.ContainsKey(name))
                _vec4Props.Add(name, Vector4.Zero);
            _vec4Props[name] = value;
            _isDirty = true;
        }
        
        public bool GetVector4(string name, ref Vector4 value)
        {
            if (_vec4Props.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }

        public void SetColor(string name, Color value)
        {
            if (!_vec4Props.ContainsKey(name))
                _vec4Props.Add(name, Vector4.Zero);
            _vec4Props[name] = value.ToVector4();
            _isDirty = true;
        }
        
        public bool GetColor(string name, ref Color value)
        {
            if (_vec4Props.TryGetValue(name, out var result))
            {
                value = new Color(result);
                return true;
            }
            return false;
        }

        public void SetTexture(string name, Texture2D value)
        {
            if (!_texProps.ContainsKey(name))
                _texProps.Add(name, null);
            _texProps[name] = value;
            _isDirty = true;
        }
        
        public bool GetTexture(string name, ref Texture2D value)
        {
            if (_texProps.TryGetValue(name, out var result))
            {
                value = result;
                return true;
            }
            return false;
        }
        
    }
}
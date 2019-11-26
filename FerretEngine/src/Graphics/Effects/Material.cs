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
        public Effect Effect
        {
            get => _effect;
            set
            {
                _effect = value;
                UpdateEffect();
            }
        }
        private Effect _effect;

        private bool _dirtyProperties;

        private List<PropertyInt> _intProps;
        private List<PropertyFloat> _floatProps;
        private List<PropertyVector2> _vec2Props;
        private List<PropertyVector3> _vec3Props;
        private List<PropertyVector4> _vec4Props;
        
        
        public Material()
        {
            _intProps = new List<PropertyInt>();
            _floatProps = new List<PropertyFloat>();
            _vec2Props = new List<PropertyVector2>();
            _vec3Props = new List<PropertyVector3>();
            _vec4Props = new List<PropertyVector4>();
        }

        public Material(Effect effect) : this()
        {
            Effect = effect;

            UpdateEffect();
        }

        public Material(string effectPath)
        {
            Effect = FeContent.LoadEffect(effectPath);
        }


        internal void CheckDirty()
        {
            if (_dirtyProperties)
            {
                UpdateEffect();
                _dirtyProperties = false;
            }
        }


        private void UpdateEffect()
        {
            if (_effect == null)
                return;
            
            // TODO apply global shader values

            foreach (EffectParameter p in _effect.Parameters)
            {
                StringBuilder str = new StringBuilder();
                str.Append($"Name = {p.Name} ");
                str.Append($"Type = {p.ParameterType} ");
                str.Append($"Class = {p.ParameterClass} ");
                str.Append($"ColumnCount = {p.ColumnCount} ");
                FeLog.Warning(str.ToString());
            }

        }
        
        
        
        
        
        
        public void SetInt(string id, int value)
        {
            
        }
        
        public void SetFloat(float id, int value)
        {
            
        }
        
        public void SetVector2(string id, Vector2 value)
        {
            
        }
        
        public void SetVector3(string id, Vector3 value)
        {
            
        }
        
        public void SetVector4(string id, Vector4 value)
        {
            
        }

        public void SetColor(string id, Color value)
        {
            
        }
    }
}
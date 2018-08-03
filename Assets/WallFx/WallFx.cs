using UnityEngine;
using Klak.Math;

namespace Flipper
{
    public class WallFx : MonoBehaviour
    {
        #region Editable variables

        public enum EffectType
        {
            SimpleFill, Flasher,
            VerticalBar, HorizontalBar,
            VerticalSlits, HorizontalSlits, DiagonalSlits,
            StreamLines
        }

        [SerializeField] EffectType _effect;

        public EffectType Effect {
            get { return _effect; }
            set { _effect = value; }
        }

        #endregion

        #region Runtime properties

        public float Level { get; set; }

        #endregion

        #region Shader resources

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            _material = new Material(_shader);
        }

        void OnDestroy()
        {
            Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var lv = Level;

            if (_effect == EffectType.Flasher)
            {
                var offs = Mathf.Clamp(Perlin.Noise(Time.time * 80) * 10, -1, 1);
                lv = offs + Level - 1;
            }

            _material.SetColor("_Color", GlobalConfig.Instance.BaseColor);
            _material.SetFloat("_Intensity", Mathf.Clamp01(lv));
            Graphics.Blit(null, destination, _material, (int)_effect);
        }

        #endregion
    }
}

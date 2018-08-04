using UnityEngine;

namespace Flipper
{
    [ExecuteInEditMode]
    public class SimpleBlit : MonoBehaviour
    {
        [SerializeField] Texture _source;
        [SerializeField, Range(0, 1)] float _opacity = 1;
        [SerializeField, Range(0, 1)] float _glitch = 0;

        public float Opacity {
            get { return _opacity; }
            set { _opacity = value; }
        }

        public float Glitch {
            get { return _glitch; }
            set { _glitch = value; }
        }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        void OnDestroy()
        {
            if (_material != null)
            {
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            var fcount = Application.isPlaying ? Time.frameCount : 0;

            _material.SetFloat("_Opacity", _opacity);
            _material.SetFloat("_Glitch", _glitch * 0.4f);
            _material.SetInt("_FrameCount", fcount);

            Graphics.Blit(_source == null ? source : _source, destination, _material, 0);
        }
    }
}

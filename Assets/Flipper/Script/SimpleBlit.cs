using UnityEngine;

namespace Flipper
{
    [ExecuteInEditMode]
    public class SimpleBlit : MonoBehaviour
    {
        [SerializeField] Texture _source;
        [SerializeField, Range(0, 1)] float _sourceOpacity = 1;

        [SerializeField] Texture _overlay;
        [SerializeField, Range(0, 1)] float _overlayOpacity = 1;

        [SerializeField, Range(0, 1)] float _glitch = 0;

        public float SourceOpacity {
            get { return _sourceOpacity; }
            set { _sourceOpacity = value; }
        }

        public float OverlayOpacity {
            get { return _overlayOpacity; }
            set { _overlayOpacity = value; }
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

        void OnRenderImage(RenderTexture filterSource, RenderTexture destination)
        {
            var source = _source != null ? _source : filterSource;

            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _material.SetVector("_Opacity", new Vector2(_sourceOpacity, _overlayOpacity));
            _material.SetFloat("_Glitch", _glitch * 0.4f);
            _material.SetInt("_FrameCount", Application.isPlaying ? Time.frameCount : 0);

            if (_overlay == null || _overlayOpacity == 0)
            {
                Graphics.Blit(source, destination, _material, 0);
            }
            else
            {
                _material.SetTexture("_Overlay", _overlay);
                Graphics.Blit(source, destination, _material, 1);
            }
        }
    }
}

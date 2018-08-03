using UnityEngine;

namespace Flipper
{
    [ExecuteInEditMode]
    public class SimpleBlit : MonoBehaviour
    {
        [SerializeField] Texture _source;
        [SerializeField, Range(0, 1)] float _opacity = 1;

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

            _material.SetFloat("_Opacity", _opacity);

            Graphics.Blit(_source == null ? source : _source, destination, _material, 0);
        }
    }
}

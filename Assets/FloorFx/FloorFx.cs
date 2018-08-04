using UnityEngine;

namespace Flipper
{
    [ExecuteInEditMode]
    class FloorFx : MonoBehaviour
    {
        #region Editable variable

        [SerializeField, Range(0, 4)] int _effectType;

        public int EffectType {
            get { return _effectType; }
            set { _effectType = value; }
        }

        #endregion

        #region Internal resources

        [SerializeField, HideInInspector] Mesh _mesh;
        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        readonly string[][] _shaderKeywords = {
            new [] { "FLOORFX0" },
            new [] { "FLOORFX1" },
            new [] { "FLOORFX2" },
            new [] { "FLOORFX3" },
            new [] { "FLOORFX4" }
        };

        #endregion

        #region MonoBehaviour implementation

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

        void LateUpdate()
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            var time = (Application.isPlaying ? Time.time : 0) + 10;

            _material.SetFloat("_Cutoff", 0.3f);
            _material.SetFloat("_LocalTime", time);
            _material.shaderKeywords = _shaderKeywords[_effectType];

            Graphics.DrawMesh(
                _mesh, transform.localToWorldMatrix,
                _material, gameObject.layer
            );
        }

        #endregion
    }
}

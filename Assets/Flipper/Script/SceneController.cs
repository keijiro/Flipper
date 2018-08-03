using UnityEngine;
using Klak.Motion;
using UnityEngine.Rendering.PostProcessing;
using Flipbook;

namespace Flipper
{
    public class SceneController : MonoBehaviour
    {
        #region Editable variables

        [SerializeField] SmoothFollow _cameraMotion;
        [SerializeField] BrownianMotion[] _cameraPivots;
        [SerializeField] PostProcessVolume _flashVolume;
        [SerializeField] Puppet.Dancer _dancer;
        [SerializeField] WallFx _wallFx;
        [SerializeField] FlipbookRenderer _flipbook;

        #endregion

        #region Private members

        float _flash;

        static float SmoothStep(float edge0, float edge1, float x)
        {
            x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
            return x * x * (3 - 2 * x);
        }

        #endregion
        
        #region Public members

        public void RandomizeBaseColor()
        {
            GlobalConfig.Instance.BaseColor = Color.HSVToRGB(Random.value, 1, 1);
        }

        public void ResetBaseColor()
        {
            GlobalConfig.Instance.BaseColor = Color.white;
        }

        public void HitEffects()
        {
            _cameraMotion.JumpRandomly();
            foreach (var piv in _cameraPivots) piv.Rehash();
            _dancer.Rehash();
            _flash = 1;
        }

        #endregion

        #region MonoBehaviour implementation

        void Update()
        {
            _flash = Mathf.Clamp01(_flash - Time.deltaTime);
        }

        void LateUpdate()
        {
            _flashVolume.weight = SmoothStep(0, 1, _flash);
            _wallFx.Flash = SmoothStep(0.8f, 1, _flash);
        }

        #endregion
    }
}

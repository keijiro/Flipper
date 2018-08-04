using UnityEngine;
using Klak.Motion;
using UnityEngine.Rendering.PostProcessing;

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
        [SerializeField] SimpleBlit[] _blitters;

        #endregion

        #region Private members

        float _flash;
        float _glitch;

        static float SmoothStep(float edge0, float edge1, float x)
        {
            x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
            return x * x * (3 - 2 * x);
        }

        void UpdatePuppetParameters(float param)
        {
            _dancer.noiseFrequency = Mathf.Lerp(0.1f, 0.33f, param);

            _dancer.footDistance  = Mathf.Lerp(0.4f, 0.6f, param);
            _dancer.stepFrequency = Mathf.Lerp(0.1f, 0.4f, param);
            _dancer.stepHeight    = Mathf.Lerp(0, 0.1f, param);
            _dancer.stepAngle     = Mathf.Lerp(0, 90, param);

            _dancer.hipHeight        = Mathf.Lerp(0.95f, 0.8f, param);
            _dancer.hipPositionNoise = Mathf.Lerp(0.05f, 0.3f, param);
            _dancer.hipRotationNoise = Mathf.Lerp(4, 50, param);

            _dancer.spineRotationNoise = Vector3.Lerp(
                new Vector3(15, 6, 6),
                new Vector3(50, 30, 30),
                param
            );

            _dancer.handPosition = Vector3.Lerp(
                new Vector3(0.3f, -0.03f, 0.19f),
                new Vector3(0.3f, 0.4f, -0.4f),
                param
            );

            _dancer.handPositionNoise = Vector3.Lerp(
                new Vector3(0.1f, 0.1f, 0.1f),
                new Vector3(0.4f, 0.4f, 0.6f),
                param
            );

            _dancer.headMove = Mathf.Lerp(3, 10, param);
        }

        #endregion

        #region Public members

        public float PuppetParameter { get; set; }
        public float BlitterGlitch { get; set; }

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

        public void HitGlitch()
        {
            _glitch = 1;
        }

        #endregion

        #region MonoBehaviour implementation

        void Update()
        {
            var dt = Time.deltaTime;
            _flash = Mathf.Clamp01(_flash - dt * 2);
            _glitch = Mathf.Clamp01(_glitch - dt * 5);
            UpdatePuppetParameters(PuppetParameter);
        }

        void LateUpdate()
        {
            _flashVolume.weight = SmoothStep(0, 1, _flash);
            _wallFx.Flash = SmoothStep(0.8f, 1, _flash);

            foreach (var blitter in _blitters)
                blitter.Glitch = BlitterGlitch + SmoothStep(0, 1, _glitch);
        }

        #endregion
    }
}

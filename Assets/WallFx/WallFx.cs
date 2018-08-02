using UnityEngine;

namespace Flipper
{
    public class WallFx : MonoBehaviour
    {
        [SerializeField] Shader _shader;

        Material _material;

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
            Graphics.Blit(null, destination, _material, 0);
        }
    }
}

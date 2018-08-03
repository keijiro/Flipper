using UnityEngine;

namespace Flipper
{
    class SprayController : MonoBehaviour
    {
        [SerializeField] Transform _snapTarget;

        Kvant.SprayMV _spray;
        float _offset;

        void Start()
        {
            _spray = GetComponent<Kvant.SprayMV>();
            _offset = _spray.emitterSize.y / 2;
        }

        void Update()
        {
            var pos = _snapTarget.position;
            pos.y = _offset;
            _spray.emitterCenter = pos;
        }
    }
}

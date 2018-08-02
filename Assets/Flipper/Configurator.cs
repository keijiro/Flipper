using UnityEngine;

namespace Flipper
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] int _displayCount = 1;

        void Start()
        {
        #if UNITY_STANDALONE
            var displays = Display.displays;
            var count = Mathf.Min(_displayCount, displays.Length);
            for (var i = 1; i < count; i++) displays[i].Activate();
        #endif
        }
    }
}

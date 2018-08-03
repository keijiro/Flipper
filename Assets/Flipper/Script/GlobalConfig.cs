using UnityEngine;

namespace Flipper
{
    [ExecuteInEditMode]
    public class GlobalConfig : MonoBehaviour
    {
        #region Singleton-like class interface

        static GlobalConfig _instance;

        public static GlobalConfig Instance { get { return _instance; } }

        #endregion

        #region Editable variables

        [SerializeField, ColorUsage(false)] Color _baseColor = Color.white;
        [SerializeField] int _displayCount = 1;

        public Color BaseColor {
            get { return _baseColor; }
            set { _baseColor = value; }
        }

        #endregion

        #region MonoBehaviour interface

        void Start()
        {
        #if UNITY_STANDALONE
            var displays = Display.displays;
            var count = Mathf.Min(_displayCount, displays.Length);
            for (var i = 1; i < count; i++) displays[i].Activate();
        #endif
        }

        void OnEnable()
        {
            Debug.Assert(_instance == null);
            _instance = this;
        }

        void OnDisable()
        {
            _instance = null;
        }

        #endregion
    }
}

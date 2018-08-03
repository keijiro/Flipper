using UnityEngine;

namespace Flipper
{
    public class SceneController : MonoBehaviour
    {
        public void RandomizeBaseColor()
        {
            GlobalConfig.Instance.BaseColor = Color.HSVToRGB(Random.value, 1, 1);
        }

        public void ResetBaseColor()
        {
            GlobalConfig.Instance.BaseColor = Color.white;
        }
    }
}

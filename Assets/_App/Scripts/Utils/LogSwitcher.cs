using UnityEngine;

namespace MobaVR
{
    public class LogSwitcher : MonoBehaviour
    {
        public bool UsePlayerSettings = false;
        public bool UseLogs = true;

        private void Awake()
        {
            if (UsePlayerSettings)
            {
                Debug.unityLogger.logEnabled = Debug.isDebugBuild;
            }
            else
            {
                Debug.unityLogger.logEnabled = UseLogs;
            }
        }
    }
}
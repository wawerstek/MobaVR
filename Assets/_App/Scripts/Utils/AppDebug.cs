using UnityEngine;

namespace MobaVR
{
    public class AppDebug
    {
        //[System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_NO_LOGGER")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
using UnityEngine;

namespace MobaVR
{
    public abstract class BaseTimeView : MonoBehaviour, IViewVisibility
    {
        public abstract void UpdateTime(float time);
        public abstract void Show();
        public abstract void Hide();
    }
}
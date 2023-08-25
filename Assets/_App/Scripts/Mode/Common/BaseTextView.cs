using UnityEngine;

namespace MobaVR
{
    public abstract class BaseTextView : MonoBehaviour, IViewVisibility
    {
        public abstract void UpdateText(string message);
        public abstract void Show();
        public abstract void Hide();
    }
}
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MobaVR
{
    public abstract class BaseVictoryView : MonoBehaviour, IViewVisibility
    {
        public abstract void Show();
        public abstract void Hide();
    }
}
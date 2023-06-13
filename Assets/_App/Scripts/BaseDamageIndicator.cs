using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobaVR
{
    public abstract class BaseDamageIndicator : MonoBehaviour, IDamageIndicator
    {
        public abstract void Show();
        public abstract void Hide();
    }
}
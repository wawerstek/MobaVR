using UnityEngine;
using UnityEngine.UI;

namespace MobaVR
{
    public class SpellImage:MonoBehaviour
    {
        [SerializeField] private Image m_OffImage;
        [SerializeField] private Image m_OnImage;

        public Image OffImage => m_OffImage;
        public Image OnImage => m_OnImage;
    }
}
using UnityEngine;

namespace MobaVR
{
    public class LoseView : BaseVictoryView
    {
        [SerializeField] private GameObject m_LoseFfx;

        public override void Show()
        {
            m_LoseFfx.SetActive(true);
        }

        public override void Hide()
        {
            m_LoseFfx.SetActive(false);
        }
    }
}
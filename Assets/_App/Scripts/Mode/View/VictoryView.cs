using UnityEngine;

namespace MobaVR
{
    public class VictoryView : BaseVictoryView
    {
        [SerializeField] private GameObject m_VictoryFfx;

        public override void Show()
        {
            m_VictoryFfx.SetActive(true);
        }

        public override void Hide()
        {
            m_VictoryFfx.SetActive(false);
        }
    }
}
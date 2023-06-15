using Michsky.MUIP;
using UnityEngine;

namespace MobaVR
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private GameObject m_ProgressGroup;
        [SerializeField] private ProgressBar m_HealthProgressBar;
        [SerializeField] private ProgressBar m_ManaProgressBar;
        [SerializeField] private ProgressBar m_DefenceProgressBar;

        public ProgressBar HealthProgressBar => m_HealthProgressBar;
        public ProgressBar ManaProgressBar => m_ManaProgressBar;
        public ProgressBar DefenceProgressBar => m_DefenceProgressBar;

        public void UpdateHealth(float healthPoint)
        {
            if (HealthProgressBar != null)
            {
                HealthProgressBar.currentPercent = healthPoint;
                HealthProgressBar.UpdateUI();
            }
        }

        public void UpdateMana(float manaPoint)
        {
            if (ManaProgressBar != null)
            {
                m_ManaProgressBar.currentPercent = manaPoint;
                m_ManaProgressBar.UpdateUI();
            }
        }

        public void UpdateDefence(float defencePoint)
        {
            if (DefenceProgressBar != null)
            {
                m_DefenceProgressBar.currentPercent = defencePoint;
                m_DefenceProgressBar.UpdateUI();
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            m_ProgressGroup.SetActive(isEnabled);
        }
    }
}
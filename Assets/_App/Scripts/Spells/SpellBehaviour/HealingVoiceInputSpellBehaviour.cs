using UnityEngine;

namespace MobaVR
{
    public class HealingVoiceInputSpellBehaviour : VoiceInputSpellBehaviour
    {
        [Header("Healing")]
        [SerializeField] private float m_HealLocalPlayer = 20f;
        [SerializeField] private float m_HealTeammate = 10f;
        [SerializeField] private float m_HealDelay = 1f;
        [SerializeField] private bool m_IsHealTeammates = true;

        private bool m_CanHealing = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_CanHealing = true;
        }

        protected override void ExecuteVoice()
        {
            base.ExecuteVoice();
            if (!m_CanHealing)
            {
                return;
            }

            if (m_PlayerVR == null)
            {
                return;
            }

            m_CanHealing = false;
            if (!m_IsHealTeammates)
            {
                m_PlayerVR.WizardPlayer.Heal(m_HealLocalPlayer);
            }
            else
            {
                Team team = m_PlayerVR.Team;
                foreach (PlayerVR playerVR in team.Players)
                {
                    if (playerVR == m_PlayerVR)
                    {
                        playerVR.WizardPlayer.Heal(m_HealLocalPlayer);
                    }
                    else
                    {
                        playerVR.WizardPlayer.Heal(m_HealTeammate);
                    }
                }
            }

            Invoke(nameof(EnableHealing), m_HealDelay);
        }

        protected void EnableHealing()
        {
            m_CanHealing = true;
        }
    }
}
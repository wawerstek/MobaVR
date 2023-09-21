using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class FogSpell : Spell
    {
        [SerializeField] private ParticleSystem m_ParticleSystem;
        [SerializeField] private Color m_TeamColor = Color.black;
        [SerializeField] private Color m_EnemyColor = Color.black;
        [SerializeField] private float m_Duration;

        [PunRPC]
        public override void RpcInit(TeamType teamType, int idOwner)
        {
            base.RpcInit(teamType, idOwner);

            ClassicGameSession gameSession = FindObjectOfType<ClassicGameSession>();
            if (gameSession == null || gameSession.LocalPlayer == null)
            {
                return;
            }

            Color fogColor = gameSession.LocalPlayer.TeamType == teamType ? m_TeamColor : m_EnemyColor;
            ParticleSystem.MainModule particleSystemMain = m_ParticleSystem.main;
            particleSystemMain.startColor = fogColor;
            particleSystemMain.duration = m_Duration;
            
            m_ParticleSystem.Play();
            
            Invoke(nameof(RpcDestroyThrowable), m_DestroyLifeTime);
        }
    }
}
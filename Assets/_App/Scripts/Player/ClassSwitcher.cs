using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ClassSwitcher : MonoBehaviourPun
    {
        [SerializeField] private SpellsHandler m_WizardSpellsHandler;
        [SerializeField] private SpellsHandler m_ArcherSpellsHandler;
        [SerializeField] private SpellsHandler m_DefenderSpellsHandler;

        private void Clear()
        {
            m_WizardSpellsHandler.gameObject.SetActive(false);
            m_ArcherSpellsHandler.gameObject.SetActive(false);
            m_DefenderSpellsHandler.gameObject.SetActive(false);
        }

        public void SelectWizard()
        {
            photonView.RPC(nameof(RpcSelectWizard), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void RpcSelectWizard()
        {
            Clear();
            m_WizardSpellsHandler.gameObject.SetActive(true);
        }
        
        public void SelectArcher()
        {
            photonView.RPC(nameof(RpcSelectArcher), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void RpcSelectArcher()
        {
            Clear();
            m_ArcherSpellsHandler.gameObject.SetActive(true);
        }
        
        public void SelectDefender()
        {
            photonView.RPC(nameof(RpcSelectDefender), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void RpcSelectDefender()
        {
            Clear();
            m_DefenderSpellsHandler.gameObject.SetActive(true);
        }
    }
}
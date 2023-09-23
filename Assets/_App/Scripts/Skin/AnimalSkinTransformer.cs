using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class AnimalSkinTransformer : MonoBehaviourPun
    {
        [SerializeField] private float m_Duration = 5f;
        [SerializeField] private ParticleSystem m_ParticleSystem;
        [SerializeField] private PlayerStateSO m_PlayerStateSO;

        private AnimalSkin m_AnimalSkin;
        private SkinCollection m_SkinCollection;
        private PlayerVR m_PlayerVR;

        private PlayerStateSO m_CurrentPlayerStateSO;
        private PlayerStateSO m_PrevPlayerStateSO;

        private bool m_IsAnimalSkin = true;

        private void OnDestroy()
        {
            m_AnimalSkin.OnActivated.RemoveListener(OnSkinActivated);
            m_AnimalSkin.OnDeactivated.RemoveListener(OnSkinDeactivated);

            m_PlayerVR.WizardPlayer.OnDie -= OnPlayerDie;
            m_PlayerVR.WizardPlayer.OnReborn -= OnPlayerReborn;
        }

        // TODO: подписываемся тут
        private void Awake()
        {
            m_AnimalSkin = GetComponent<AnimalSkin>();
            m_SkinCollection = GetComponentInParent<SkinCollection>();
            m_PlayerVR = GetComponentInParent<PlayerVR>();

            m_AnimalSkin.OnActivated.AddListener(OnSkinActivated);
            m_AnimalSkin.OnDeactivated.AddListener(OnSkinDeactivated);

            m_PlayerVR.WizardPlayer.OnDie += OnPlayerDie;
            m_PlayerVR.WizardPlayer.OnReborn += OnPlayerReborn;
        }

        private void OnPlayerDie()
        {
            if (!m_IsAnimalSkin)
            {
                return;
            }

            m_IsAnimalSkin = false;
            
            CancelInvoke(nameof(HideSkin));
            ShowParticle();
        }

        private void OnPlayerReborn()
        {
            if (!m_IsAnimalSkin)
            {
                return;
            }

            m_IsAnimalSkin = false;
            
            CancelInvoke(nameof(HideSkin));
            ShowParticle();
        }

        private void OnSkinActivated()
        {
            CancelInvoke(nameof(HideSkin));

            m_IsAnimalSkin = true;
            
            ShowParticle();
            Invoke(nameof(HideSkin), m_Duration);
        }

        private void OnSkinDeactivated()
        {
            m_IsAnimalSkin = false;
            
            CancelInvoke(nameof(HideSkin));
            ShowParticle();
        }

        private void HideSkin()
        {
            /*
            if (photonView.IsMine)
            {
                m_SkinCollection.RestoreSkin();
            }
            */

            CancelInvoke(nameof(HideSkin));
            m_IsAnimalSkin = false;
            
            ShowParticle();
            m_SkinCollection.RpcRestoreSkin();
        }

        private void ShowParticle()
        {
            m_ParticleSystem.transform.position = m_AnimalSkin.Armature.position;
            m_ParticleSystem.Play();
        }

        #region Debug

        [ContextMenu("Set Animal Skin")]
        private void SetAnimalSkin()
        {
            m_SkinCollection.SetAnimalDefaultSkin();
        }

        #endregion
    }
}
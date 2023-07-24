using System;
using DG.Tweening;
using Michsky.MUIP;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class BigShield : Spell
    {
        [SerializeField] private Renderer m_Renderer;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private SliderManager m_Slider;

        private void Awake()
        {
            m_Renderer.enabled = false;
            m_Collider.enabled = false;
            m_Slider.gameObject.SetActive(false);
        }

        public virtual void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            OnInitSpell?.Invoke();

            m_Owner = wizardPlayer;
            m_TeamType = teamType;
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }

        [PunRPC]
        public virtual void RpcInit(TeamType teamType)
        {
            m_TeamItem.SetTeam(teamType);
        }

        public void Prepare()
        {
            m_Slider.gameObject.SetActive(false);
            
            m_Renderer.enabled = true;
            m_Collider.enabled = false;

            Color color = m_Renderer.material.color;
            color.a = 0.05f;
            m_Renderer.material.color = color;
        }

        public void Place()
        {
            photonView.RPC(nameof(RpcPlace), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void RpcPlace()
        {
            transform.parent = null;

            m_Renderer.enabled = true;
            m_Collider.enabled = true;

            Color color = m_Renderer.material.color;
            color.a = 0.6f;
            m_Renderer.material.color = color;

            m_Slider.gameObject.SetActive(true);
            float sliderValue = 1f;

            DOTween
                .To(() => sliderValue, x => sliderValue = x, 0, m_DestroyLifeTime)
                .OnUpdate(() => { m_Slider.mainSlider.value = sliderValue; })
                .OnComplete(() => { m_Slider.gameObject.SetActive(false); });

            if (photonView.IsMine)
            {
                Invoke(nameof(DestroySpell), m_DestroyLifeTime);
            }
        }

        private void DestroySpell()
        {
            if (gameObject.activeSelf)
            {
                photonView.RPC(nameof(RpcDestroyShield), RpcTarget.All);
                StopAllCoroutines();
            }
        }

        [PunRPC]
        private void RpcDestroyShield()
        {
            OnDestroySpell?.Invoke();

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }
    }
}
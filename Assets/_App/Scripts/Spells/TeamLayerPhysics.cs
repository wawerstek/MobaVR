using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class IgnoreOwnerPhysics : MonoBehaviourPun
    {
        private Spell m_Spell;
        private Collider[] m_Colliders;

        private void OnEnable()
        {
            if (m_Spell != null)
            {
                //m_Spell.OnInitSpell += OnInitSpell;
                m_Spell.OnRpcInitSpell += OnInitSpell;
            }
        }

        private void OnDisable()
        {
            if (m_Spell != null)
            {
                //m_Spell.OnInitSpell -= OnInitSpell;
                m_Spell.OnRpcInitSpell -= OnInitSpell;
            }
        }

        private void Awake()
        {
            /*
            if (!photonView.IsMine)
            {
                return;
            }
            */
            
            m_Spell = GetComponent<Spell>();
            m_Colliders = GetComponents<Collider>();
        }

        private void OnInitSpell()
        {
            if (m_Spell.Owner != null)
            {
                SkinCollection skinCollection = m_Spell.Owner.PlayerVR.SkinCollection;
                Skin activeSkin = skinCollection.AliveActiveSkin;
                if (activeSkin != null)
                {
                    HitCollider[] hitColliders = activeSkin.GetComponentsInChildren<HitCollider>();
                    foreach (HitCollider hitCollider in hitColliders)
                    {
                        foreach (Collider spellCollider in m_Colliders)
                        {
                            Physics.IgnoreCollision(spellCollider, hitCollider.Collider, this);
                        }
                    }
                }
            }
        }
    }
}
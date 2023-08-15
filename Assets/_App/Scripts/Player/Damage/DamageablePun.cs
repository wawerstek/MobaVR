using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    [RequireComponent(typeof(PhotonView))]
    public class DamageablePun : Damageable
    {
        protected PhotonView m_PhotonView;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out m_PhotonView);
        }
    }
}
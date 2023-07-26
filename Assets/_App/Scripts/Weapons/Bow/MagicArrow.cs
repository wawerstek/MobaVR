using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.Weapons.Bow
{
    public class MagicArrow : ArrowSpell
    {
        [SerializeField] private float m_MaxScale = 5f;
        [SerializeField] private float m_DurationScale = 1f;

        [PunRPC]
        protected override void RpcReleaseArrow(Vector3 position, Quaternion rotation, Vector3 force)
        {
            foreach (Collider collisionCollider in m_CollisionColliders)
            {
                collisionCollider.enabled = false;
            }
            
            if (m_Arrow != null)
            {
                Vector3 scale = transform.lossyScale * m_MaxScale;
                transform.DOScale(scale, m_DurationScale);
            }


            base.RpcReleaseArrow(position, rotation, force);
        }

        protected override void HandleCollision(Transform interactable)
        {
            //base.HandleCollision(interactable);
        }
    }
}
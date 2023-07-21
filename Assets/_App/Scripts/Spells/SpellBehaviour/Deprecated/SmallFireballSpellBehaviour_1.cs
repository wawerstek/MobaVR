using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class SmallFireballSpellBehaviour_1 : InputSpellBehaviour
    {
        [SerializeField] private SmallFireBall m_SmallFireballPrefab;

        private SmallFireBall m_SmallFireBall;

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

           // m_SpellsHandler.SetCurrentSpell(this);
            
            CreateSmallFireBall(m_MainHandInputVR.FingerPoint);
            ThrowSmallFireBall(m_SmallFireBall, m_MainHandInputVR.Grabber.transform.forward);
        }

        private void CreateSmallFireBall(Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_SmallFireballPrefab.name}",
                                                                   point.transform.position,
                                                                   point.transform.rotation);
            
            if (networkFireball.TryGetComponent(out m_SmallFireBall))
            {
                m_SmallFireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);

                Transform fireBallTransform = m_SmallFireBall.transform;
                fireBallTransform.parent = null;
                fireBallTransform.position = point.transform.position;
                fireBallTransform.rotation = Quaternion.identity;
            }
        }

        private void ThrowSmallFireBall(SmallFireBall fireBall, Vector3 direction)
        {
            if (fireBall != null)
            {
                fireBall.Shoot(direction);
                fireBall = null;
            }
        }

     
    }
}
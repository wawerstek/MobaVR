using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    //[CreateAssetMenu(fileName = "SmallFireball", menuName = "MobaVR API/Spells/Create small fireball")]
    public class SmallFireballSpellBehaviour : InputSpellBehaviour
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

            m_SpellsHandler.SetCurrentSpell(this);
            CreateSmallFireBall(m_MainHandInputVR.FingerPoint);
            ThrowSmallFireBall(m_SmallFireBall, m_MainHandInputVR.Grabber.transform.forward);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            
            //m_SpellsHandler.DeactivateCurrentSpell(this);
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
                fireBall.ThrowByDirection(direction);
                fireBall = null;
            }
        }

        public override void SpellEnter()
        {
        }

        public override void SpellUpdate()
        {
        }

        public override void SpellExit()
        {
        }
    }
}
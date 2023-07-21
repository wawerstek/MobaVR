using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
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

            Shoot(m_MainHandInputVR.FingerPoint, m_MainHandInputVR.Grabber.transform.forward);
        }

        private void Shoot(Transform point, Vector3 direction)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_SmallFireballPrefab.name}",
                                                                   point.position,
                                                                   point.rotation);

            if (networkFireball.TryGetComponent(out m_SmallFireBall))
            {
                Transform fireBallTransform = m_SmallFireBall.transform;
                fireBallTransform.parent = null;
                fireBallTransform.position = point.transform.position;
                fireBallTransform.rotation = Quaternion.identity;

                m_SmallFireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                m_SmallFireBall.OnInitSpell = () => { m_IsPerformed = true; };
                m_SmallFireBall.OnDestroySpell = () =>
                {
                    m_IsPerformed = false;
                    m_SmallFireBall = null;
                    OnCompleted?.Invoke();
                };
                
                m_SmallFireBall.Shoot(direction);
            }
        }
    }
}
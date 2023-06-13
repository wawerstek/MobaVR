using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BigFireballBehaviourSO : SpellBehaviourSO
    {
        [SerializeField] private BigFireBall m_BigFireballPrefab;

        private BigFireBall m_FireBall;

        protected override void OnStartAction(InputAction.CallbackContext context)
        {
        }

        protected override void OnPerformedAction(InputAction.CallbackContext context)
        {
            if (!m_WizardPlayer.CurrentPlayerState.CanCast)
            {
                return;
            }

            if (m_WizardPlayer.IsLife)
            {
                Transform point;
                if (m_HandType == SpellHandType.LEFT_HAND)
                {
                    point = m_WizardPlayer.InputVR.LeftGrabber.transform;
                }
                else
                {
                    point = m_WizardPlayer.InputVR.RightGrabber.transform;
                }
                
                CreateFireBall(out m_FireBall, point);
            }
        }

        protected override void OnCanceledAction(InputAction.CallbackContext context)
        {
            if (!m_WizardPlayer.CurrentPlayerState.CanCast)
            {
                return;
            }

            if (m_WizardPlayer.IsLife)
            {
                ThrowFireBall(m_FireBall);
            }
        }

        private void CreateFireBall(out BigFireBall fireBall, Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_BigFireballPrefab.name}",
                                                                   point.transform.position,
                                                                   point.transform.rotation);
            if (networkFireball.TryGetComponent(out fireBall))
            {
                fireBall.Init(m_WizardPlayer, m_WizardPlayer.TeamType);

                Transform fireBallTransform = fireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;
                //fireBall.Owner = m_WizardPlayer;
            }
        }
        
        private void ThrowFireBall(BigFireBall fireBall)
        {
            if (fireBall != null)
            {
                fireBall.Throw();
            }
        }
    }
}
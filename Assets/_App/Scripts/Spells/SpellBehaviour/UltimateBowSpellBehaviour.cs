using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Bow = MobaVR.Weapons.Bow.Bow;

namespace MobaVR
{
    public class UltimateBowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private BowSpell m_Bow;
        [SerializeField] private ArrowSpell m_ArrowPrefab;

        private ArrowSpell m_CurrentArrow;
        private bool m_IsThrown = false;
        private int m_Number = 0;
        private bool m_IsAttach = false;

        #region Input Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Bow.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Bow.gameObject.SetActive(false);
        }

        protected override void OnStartCast(InputAction.CallbackContext context)
        {
            base.OnStartCast(context);
            OnStarted?.Invoke();
        }

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);
            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            OnPerformed?.Invoke();

            m_IsPerformed = true;
            m_IsThrown = false;
            m_IsAttach = false;

            CreateArrow(m_RightHand.Grabber.transform);

            if (!m_Bow.Grabbable.IsGrabbable())
            {
                m_Bow.Grabbable.DropItem(m_RightHand.Grabber);
            }

            m_Bow.Grabbable.GrabRemoteItem(m_LeftHand.Grabber);
            m_Bow.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (!CanCast())
            {
                return;
            }

            if (!m_IsPerformed)
            {
                return;
            }

            m_IsPerformed = false;

            if (m_IsAttach)
            {
                Release();
            }
            else
            {
                Interrupt();
            }
        }

        protected override void Interrupt()
        {
            base.Interrupt();

            if (m_CurrentArrow != null)
            {
                m_CurrentArrow.DestroySpell();
                m_CurrentArrow = null;

                m_IsAttach = false;
                m_IsThrown = false;
            }

            m_IsPerformed = false;
            m_Bow.Show(false);
            m_Bow.Grabbable.DropItem(m_LeftHand.Grabber);

            WaitCooldown();
            OnCompleted?.Invoke();
        }

        private void Release()
        {
            if (m_CurrentArrow != null)
            {
                //m_CurrentArrow.Release();
                m_CurrentArrow = null;

                m_IsThrown = true;
                m_IsAttach = false;
            }

            m_IsPerformed = false;
            m_Bow.Show(false);
            m_Bow.Grabbable.DropItem(m_LeftHand.Grabber);
            
            WaitCooldown();
            OnCompleted?.Invoke();
        }

        #endregion

        #region Fireball

        private void CreateArrow(Transform point)
        {
            GameObject networkArrow = PhotonNetwork.Instantiate($"Spells/{m_ArrowPrefab.name}",
                                                                point.position,
                                                                point.rotation);

            m_IsThrown = false;

            if (networkArrow.TryGetComponent(out ArrowSpell arrowSpell))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_ArrowPrefab.name}_{handName}_{m_Number}";
                networkArrow.name = fireballName;

                Transform fireBallTransform = networkArrow.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                arrowSpell.Arrow.OnAttach.AddListener(OnAttach);

                arrowSpell.OnInitSpell += () => OnInitSpell(arrowSpell);
                arrowSpell.OnDestroySpell += () => OnDestroySpell(arrowSpell);

                arrowSpell.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                m_CurrentArrow = arrowSpell;
            }
        }

        private void OnAttach(Bow bow)
        {
            m_IsAttach = true;
        }

        private void OnDestroySpell(ArrowSpell arrowSpell)
        {
            if (arrowSpell != null)
            {
                arrowSpell.OnInitSpell -= () => OnInitSpell(arrowSpell);
                arrowSpell.OnDestroySpell -= () => OnDestroySpell(arrowSpell);

                arrowSpell.Arrow.OnAttach.RemoveListener(OnAttach);
            }

            if (m_CurrentArrow == arrowSpell)
            {
                m_IsPerformed = false;
                OnCompleted?.Invoke();
            }
        }

        private void OnInitSpell(ArrowSpell arrowSpell)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        #endregion
    }
}
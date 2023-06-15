using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZBoom;
using MobaVR;

namespace ZBoom.Menu
{
    public class StaffController : MonoBehaviour
    {
        public enum MagicSpellType
        {
            FIREBALL = 0,
            ICEBALL = 1,
            DARKBALL = 2,
            ELECTRICBALL = 3
        }

        public enum StaffState
        {
            IDLE,
            ATTACK_START,
            ATTACK_END
        }

        public FpsWizardController HeroController;

        public MagicSpell FireBallMagicSpell;
        public MagicSpell IceBallMagicSpell;
        public MagicSpell DarkBallMagicSpell;
        public MagicSpell ElectricBallMagicSpell;

        public GameObject FireBallGameObject;
        public GameObject IceBallGameObject;
        public GameObject DarkBallGameObject;
        public GameObject ElectricGameObject;

        [SerializeField] private Animator m_RootAnimator;
        [SerializeField] private Animator m_ChildAnimator;

        private StaffState m_CurrentStaffState;
        [SerializeField, ReadOnly] private bool m_CanAttack = true;

        private MagicSpell m_CurrentMagicSlellPrefab;
        private GameObject m_CurrentMagicGameObject;
        private int m_CurrentPosition;

        public StaffState CurrentStaffState
        {
            get => m_CurrentStaffState;
            set
            {
                m_CurrentStaffState = value;
                switch (value)
                {
                    case StaffState.IDLE:
                        break;
                    case StaffState.ATTACK_START:
                        CanAttack = false;
                        m_CurrentMagicGameObject.SetActive(false);
                        CreateMagicSpell();
                        break;
                    case StaffState.ATTACK_END:
                        m_CurrentMagicGameObject.SetActive(true);
                        CanAttack = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public bool CanAttack
        {
            get => m_CanAttack;
            set => m_CanAttack = value;
        }

        public MagicSpell CurrentMagicSlellPrefab
        {
            get => m_CurrentMagicSlellPrefab;
            set => m_CurrentMagicSlellPrefab = value;
        }

        private void Start()
        {
            CanAttack = true;
            UpdateMagicSpell(0);
        }

        public void UpdateMagicSpell(int position)
        {
            Clear();
            m_CurrentPosition = position;
            if (m_CurrentPosition < 0)
            {
                m_CurrentPosition = 3;
            }

            if (m_CurrentPosition > 3)
            {
                m_CurrentPosition = 0;
            }

            switch (m_CurrentPosition)
            {
                case 0:
                    CurrentMagicSlellPrefab = FireBallMagicSpell;
                    m_CurrentMagicGameObject = FireBallGameObject;
                    break;
                case 1:
                    CurrentMagicSlellPrefab = IceBallMagicSpell;
                    m_CurrentMagicGameObject = IceBallGameObject;
                    break;
                case 2:
                    CurrentMagicSlellPrefab = DarkBallMagicSpell;
                    m_CurrentMagicGameObject = DarkBallGameObject;
                    break;
                case 3:
                    CurrentMagicSlellPrefab = ElectricBallMagicSpell;
                    m_CurrentMagicGameObject = ElectricGameObject;
                    break;
            }

            m_CurrentMagicGameObject.SetActive(true);
        }

        private void Clear()
        {
            FireBallGameObject.SetActive(false);
            IceBallGameObject.SetActive(false);
            DarkBallGameObject.SetActive(false);
            ElectricGameObject.SetActive(false);
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButton(0))
                {
                    //Attack();
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    m_CurrentPosition++;
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    m_CurrentPosition--;
                }

                UpdateMagicSpell(m_CurrentPosition);
            }
        }

        public void Attack()
        {
            if (CanAttack)
            {
                CanAttack = false;
                m_ChildAnimator.SetTrigger("Attack");
            }
        }

        private void CreateMagicSpell()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            //var launchPoint = m_Camera.transform;
            var magicSpell = Instantiate(CurrentMagicSlellPrefab);
            //m_CurrentMagicGameObject.transform.rotation);
            Vector3 position = m_CurrentMagicGameObject.transform.position;
            position += HeroController.transform.forward * 1.2f; //1.6
            Quaternion rotation = HeroController.transform.rotation;
            magicSpell.transform.position = position;
            magicSpell.transform.rotation = rotation;

            var rigid = magicSpell.GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            //rigid.AddForce(ray.direction * 15f + Vector3.up * 5f);
            rigid.AddForce(ray.direction * magicSpell.Speed);
            //rigid.AddRelativeForce(ray.direction * magicSpell.Speed);
        }
    }
}
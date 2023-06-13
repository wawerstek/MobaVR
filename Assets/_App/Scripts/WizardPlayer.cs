using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    /// <summary>
    /// Отвечает за существование игрока, ХП, каст заклинаний, обработку ввода контроллера
    /// </summary>
    public class WizardPlayer : MonoBehaviourPunCallbacks
    {
        protected const string TAG = nameof(WizardPlayer);

        [Header("Fireballs")]
        [SerializeField] private BigFireBall m_BigFireballPrefab;
        [SerializeField] private SmallFireBall m_SmallFireballPrefab;

        [Header("Shields")]
        [SerializeField] private Shield m_LeftShield;
        [SerializeField] private Shield m_RightShield;

        [Header("Player")]
        [SerializeField] private Teammate m_Teammate;
        [SerializeField] private PlayerView m_PlayerView;
        [SerializeField] private InputBridge inputBridge;

        /// <summary>
        /// TODO: input for Oculus and Pico
        /// </summary>
        [Header("Input Platform")]
        [SerializeField] private InputActionSO m_ActiveInput;
        [SerializeField] private bool m_IsAutoDetect = false;
        [SerializeField] private InputActionSO m_OculusInput;
        [SerializeField] private InputActionSO m_PicoInput;

        [Header("Input")]
        [SerializeField] private InputActionReference m_HealthInput;
        [SerializeField] private InputActionReference m_SwitchModeLeftHandInput;
        [SerializeField] private InputActionReference m_SwitchModeRightHandInput;

        /// <summary>
        /// TODO: make new class for left and right hand with this dependencies
        /// </summary>
        [Header("Right Hand")]
        [SerializeField] private Grabber m_RightGrabber;
        [SerializeField] private Transform m_RightBigFireballPoint;
        [SerializeField] private Transform m_RightSmallFireballPoint;
        [SerializeField] private InputActionReference m_RightGrabInput;
        [SerializeField] private InputActionReference m_RightActivateInput;

        [Header("Left Hand")]
        [SerializeField] private Grabber m_LeftGrabber;
        [SerializeField] private Transform m_LeftBigFireballPoint;
        [SerializeField] private Transform m_LeftSmallFireballPoint;
        [SerializeField] private InputActionReference m_LeftGrabInput;
        [SerializeField] private InputActionReference m_LeftActivateInput;

        private BigFireBall m_RightBigFireBall;
        private BigFireBall m_LeftBigFireBall;

        private SmallFireBall m_RightSmallFireBall;
        private SmallFireBall m_LeftSmallFireBall;

        private TeamType m_TeamType = TeamType.RED;
        public float m_Health = 100f;
        private float m_CurrentHealth = 100f;

        /// <summary>
        /// TODO: move this logic to new class
        /// </summary>
        private bool m_IsAttackLeftHand = false;
        private bool m_IsAttackRightHand = false;

        private bool m_UseLeftShield = false;
        private bool m_UseRightShield = false;

        public float CurrentHealth => m_CurrentHealth;
        public bool IsLife => m_CurrentHealth > 0;
        public TeamType TeamType
        {
            get => m_TeamType;
            set => m_TeamType = value;
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                enabled = false;
                return;
            }

            InputActionSO inputActionSO = m_ActiveInput;
           InputBridge inputBridge = FindObjectOfType<InputBridge>();
            if (inputBridge != null && m_IsAutoDetect)
            {
                inputActionSO = inputBridge.InputSource == XRInputSource.Pico ? m_PicoInput : m_OculusInput;
            }

            m_HealthInput = inputActionSO.HealthInput;
            m_SwitchModeLeftHandInput = inputActionSO.SwitchModeLeftHandInput;
            m_SwitchModeRightHandInput = inputActionSO.SwitchModeRightHandInput;
            m_LeftGrabInput = inputActionSO.LeftGrabInput;
            m_LeftActivateInput = inputActionSO.LeftActivateInput;
            m_RightGrabInput = inputActionSO.RightGrabInput;
            m_RightActivateInput = inputActionSO.RightActivateInput;

            ShowShield(m_LeftShield, false);
            ShowShield(m_RightShield, false);

            #region Switch Mode

            m_SwitchModeLeftHandInput.action.performed += context =>
            {
                if (m_IsAttackLeftHand)
                {
                    ThrowBigFireBall(m_LeftBigFireBall);
                }
                else
                {
                    m_LeftShield.Show(false);
                }

                m_IsAttackLeftHand = !m_IsAttackLeftHand;
            };

            m_SwitchModeRightHandInput.action.performed += context =>
            {
                if (m_IsAttackRightHand)
                {
                    ThrowBigFireBall(m_RightBigFireBall);
                }
                else
                {
                    m_RightShield.Show(false);
                }

                m_IsAttackRightHand = !m_IsAttackRightHand;
            };

            #endregion

            #region Attack big fireballs

            m_RightGrabInput.action.started += context => { Debug.Log($"{TAG}: RightGrab: started"); };
            m_LeftGrabInput.action.started += context => { Debug.Log($"{TAG}: LeftGrab: started"); };

            m_RightGrabInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: RightGrab: performed");
                if (IsLife)
                {
                    if (!m_UseRightShield)
                    {
                        CreateBigFireBall(out m_RightBigFireBall, m_RightGrabber);
                    }
                }
            };
            m_LeftGrabInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: LeftGrab: performed");
                if (IsLife)
                {
                    if (!m_UseLeftShield)
                    {
                        CreateBigFireBall(out m_LeftBigFireBall, m_LeftGrabber);
                    }
                }
            };

            m_RightGrabInput.action.canceled += context =>
            {
                Debug.Log($"{TAG}: RightGrab: canceled");
                if (IsLife)
                {
                    ThrowBigFireBall(m_RightBigFireBall);
                }
            };
            m_LeftGrabInput.action.canceled += context =>
            {
                Debug.Log($"{TAG}: LeftGrab: canceled");
                if (IsLife)
                {
                    ThrowBigFireBall(m_LeftBigFireBall);
                }
            };

            #endregion

            #region Attack small fireballs or use shield

            m_RightActivateInput.action.started += context => { Debug.Log($"{TAG}: RightActivate: started"); };
            m_LeftActivateInput.action.started += context => { Debug.Log($"{TAG}: LeftActivate: started"); };

            m_RightActivateInput.action.performed += context =>
            {
               // Debug.Log($"{TAG}: RightActivate: performed");
                if (IsLife)
                {
                    if (m_IsAttackRightHand)
                    {
                        ShootFireBall(m_RightBigFireBall,
                                      out m_RightSmallFireBall,
                                      m_RightGrabber,
                                      m_RightBigFireballPoint,
                                      m_RightSmallFireballPoint,
                                      -m_RightGrabber.transform.right);
                    }
                    else
                    {
                        ThrowBigFireBall(m_RightBigFireBall);

                        m_UseRightShield = true;
                        ShowShield(m_RightShield, true);
                    }
                }
            };
            m_LeftActivateInput.action.performed += context =>
            {
               // Debug.Log($"{TAG}: LeftActivate: performed");
                if (IsLife)
                {
                    if (m_IsAttackLeftHand)
                    {
                        if (m_IsAttackLeftHand)
                        {
                            ShootFireBall(m_LeftBigFireBall,
                                          out m_LeftSmallFireBall,
                                          m_LeftGrabber,
                                          m_LeftBigFireballPoint,
                                          m_LeftSmallFireballPoint,
                                          m_LeftGrabber.transform.right);
                        }
                    }
                    else
                    {
                        ThrowBigFireBall(m_LeftBigFireBall);

                        m_UseLeftShield = true;
                        ShowShield(m_LeftShield, true);
                    }
                }
            };

            m_RightActivateInput.action.canceled += context =>
            {
               // Debug.Log($"{TAG}: RightActivate: canceled");

                if (m_UseRightShield)
                {
                    m_UseRightShield = false;
                    ShowShield(m_RightShield, false);
                }
            };
            m_LeftActivateInput.action.canceled += context =>
            {
               // Debug.Log($"{TAG}: LeftActivate: canceled");

                if (m_UseLeftShield)
                {
                    m_UseLeftShield = false;
                    ShowShield(m_LeftShield, false);
                }
            };

            #endregion

            #region Other Inputs

            m_HealthInput.action.performed += context =>
            {
                // Debug.Log($"{TAG}: HealthButton: performed");
                RestoreHp();
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            };

            #endregion
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    RestoreHp();
            //}


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               
                CreateBigFireBall(out m_LeftBigFireBall, m_LeftGrabber);
                if (m_LeftBigFireBall != null)
                {
                    m_LeftBigFireBall.transform.parent = null;
                    m_LeftBigFireBall.ThrowByDirection(m_LeftGrabber.transform.forward);
                }
            }

           
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                
                CreateBigFireBall(out m_RightBigFireBall, m_RightGrabber);
                if (m_RightBigFireBall != null)
                {
                    m_RightBigFireBall.transform.parent = null;
                    m_RightBigFireBall.ThrowByDirection(m_RightGrabber.transform.forward);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                
                CreateSmallFireBall(out m_LeftSmallFireBall, m_LeftGrabber);
                if (m_LeftSmallFireBall != null)
                {
                    m_LeftSmallFireBall.transform.parent = null;
                    m_LeftSmallFireBall.ThrowByDirection(m_LeftGrabber.transform.forward);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
               
                CreateSmallFireBall(out m_RightSmallFireBall, m_RightGrabber);
                if (m_RightSmallFireBall != null)
                {
                    m_RightSmallFireBall.transform.parent = null;
                    m_RightSmallFireBall.ThrowByDirection(m_RightGrabber.transform.forward);
                }
            }

            //это со щитами
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ShowShield(m_LeftShield, true);
            }

            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                ShowShield(m_LeftShield, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ShowShield(m_RightShield, true);
            }

            if (Input.GetKeyUp(KeyCode.Alpha8))
            {
                ShowShield(m_RightShield, false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                CreateBigFireBall(out m_RightBigFireBall, m_RightGrabber);
                if (m_RightBigFireBall != null)
                {
                    m_RightBigFireBall.transform.parent = null;
                    //m_RightBigFireBall.ThrowForce(m_RightGrabber.transform.forward);
                    m_RightBigFireBall.ThrowByDirection(m_RightGrabber.transform.forward);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateBigFireBall(out m_LeftBigFireBall, m_LeftGrabber);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                ShootFireBall(m_RightBigFireBall,
                              out m_RightSmallFireBall,
                              m_RightGrabber,
                              m_RightBigFireballPoint,
                              m_RightSmallFireballPoint,
                              -m_RightGrabber.transform.right);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateBigFireBall(out m_RightBigFireBall, m_RightGrabber);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                ShootFireBall(m_RightBigFireBall,
                              out m_RightSmallFireBall,
                              m_RightGrabber,
                              m_RightBigFireballPoint,
                              m_RightSmallFireballPoint,
                              -m_RightGrabber.transform.right);
            }
        }

        #region Shields

        private void ShowShield(Shield shield, bool isShow)
        {
            //shield.SetActive(isShow);
            shield.Show(isShow);
        }

        #endregion

        #region Throw Fireballs

        private void ThrowBigFireBall(BigFireBall fireBall)
        {
            if (fireBall != null)
            {
                //fireBall.Throw();
                fireBall.Throw();
                fireBall = null;
            }
        }

        private void ThrowSmallFireBall(SmallFireBall fireBall, Grabber grabber, Vector3 direction)
        {
            if (fireBall != null)
            {
                //fireBall.ThrowForce(grabber.transform.forward);
                fireBall.ThrowByDirection(grabber.transform.forward);
                fireBall = null;
            }
        }

        /// <summary>
        /// TODO: need refactoring
        /// У большого шара возможно два варианта полета: от руки / от плеча и по нажатию курка.
        /// Этот метод активируется при нажатии курка и проверяет, есть ли у игрока сейчас шар,
        /// Если есть, то мы управляем шаром в воздухе = меняем направление его движения.
        /// Если нет, то мы стреляем маленькими снарядами. 
        private void ShootFireBall(BigFireBall bigFireBall,
                                   out SmallFireBall smallFireBall,
                                   Grabber grabber,
                                   Transform bigFireballPoint,
                                   Transform smallFireballPoint,
                                   Vector3 direction)
        {
            if (bigFireBall != null)
            {
                bigFireBall.ThrowByDirection(direction);
            }
            else
            {
                CreateSmallFireBall(out smallFireBall, smallFireballPoint);
                ThrowSmallFireBall(smallFireBall, grabber, direction);
            }

            bigFireBall = null;
            smallFireBall = null;
        }

        #endregion

        #region Create Big Fireball

        private void CreateBigFireBall(out BigFireBall fireBall, Grabber grabber)
        {
            CreateBigFireBall(out fireBall, grabber.transform);
        }

        private void CreateBigFireBall(out BigFireBall fireBall, Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_BigFireballPrefab.name}",
                                                                   point.transform.position,
                                                                   point.transform.rotation);
            if (networkFireball.TryGetComponent(out fireBall))
            {
               fireBall.Init(m_TeamType);

                Transform fireBallTransform = fireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;
               fireBall.Owner = this;
            }
        }

        #endregion

        #region Create Small Fireball

        private void CreateSmallFireBall(out SmallFireBall fireBall, Grabber grabber)
        {
            CreateSmallFireBall(out fireBall, grabber.transform);
        }

        private void CreateSmallFireBall(out SmallFireBall fireBall, Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_SmallFireballPrefab.name}",
                                                                   point.transform.position,
                                                                   point.transform.rotation);
            if (networkFireball.TryGetComponent(out fireBall))
            {
                fireBall.Init(m_TeamType);

                Transform fireBallTransform = fireBall.transform;
                fireBallTransform.parent = null;
                fireBallTransform.position = point.transform.position;
                fireBallTransform.rotation = Quaternion.identity;
                fireBall.Owner = this;
            }
        }

        #endregion

        //урон скорее всего одна из функций вызывается при ранении, скорее всего это Hit
        #region HP

        private void RestoreHp()
        {
            if (photonView.IsMine)
            {
                m_CurrentHealth = m_Health;
                m_PlayerView.RpcSetHealth(m_CurrentHealth);
            }
        }

        //вызвалась функция и получила значение файрбола и damage
        public void Hit(Fireball fireball, float damage)
        {
            //тут как раз проверка на командность, если файир бол прилетел из команды той же команды, что и игрок, то выходим
            if ((fireball.Team.IsRed && m_Teammate.IsRed)
                || (!fireball.Team.IsRed && !m_Teammate.IsRed))
            {
                return;
            }

            //если прилетел из другой команды, то запускаем у всех функцию
            photonView.RPC(nameof(RpcHit), RpcTarget.All, damage);
        }

        [PunRPC]
        public void RpcHit(float damage)
        {
            //если это я
            if (photonView.IsMine)
            {
                m_CurrentHealth -= damage;
                m_PlayerView.RpcSetHealth(m_CurrentHealth);
            }
        }



        
        public void Respown()
        {
            m_Health = 100f;
            if (photonView.IsMine)
            {
                m_CurrentHealth = m_Health;
                m_PlayerView.RpcSetHealth(m_CurrentHealth);
            }

            //  photonView.RPC(nameof(RpcRespown), RpcTarget.AllBuffered, damage);
        }

        //[PunRPC]
        //public void RpcRespown(float damage)
        //{
        //    //если это я
        //    if (photonView.IsMine)
        //    {
        //        m_CurrentHealth = damage;
        //        m_PlayerView.RpcSetHealth(m_CurrentHealth);
        //    }
        //}



        #endregion

        #region Init Transforms

        public void SetLeftGrabber(Grabber grabber, Transform smallFireballPoint, Transform bigFireballPoint)
        {
            m_LeftGrabber = grabber;
            m_LeftSmallFireballPoint = smallFireballPoint;
            m_LeftBigFireballPoint = bigFireballPoint;
        }

        public void SetRightGrabber(Grabber grabber, Transform smallFireballPoint, Transform bigFireballPoint)
        {
            m_RightGrabber = grabber;
            m_RightSmallFireballPoint = smallFireballPoint;
            m_RightBigFireballPoint = bigFireballPoint;
        }

        #endregion
    }
}
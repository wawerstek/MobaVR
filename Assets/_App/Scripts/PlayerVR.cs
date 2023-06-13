using System.Collections.Generic;
using BNG;
using Photon.Pun;
using UnityEngine;


namespace MobaVR
{
    /// <summary>
    /// Основной класс, который содержит в себе все зависимости игрока.
    /// Этот класс используется для инициализации игрока, передачи ему InputVR, выбор команды
    /// ИК: сейчас не работает.
    /// </summary>
    public class PlayerVR : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TeamType m_CurrentTeam = TeamType.RED;


        
        private GameObject _InputVR;
        private ChangeTeam _ChangeTeam;

        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private Teammate m_Teammate;


        [SerializeField] private CharacterIK m_CharacterIK;

        [Header("Hands")]
        [SerializeField] private HandController m_LeftHand;
        [SerializeField] private HandController m_RightHand;

        [Header("Renderers")]
        [SerializeField] private bool m_IsRender = false;
        [SerializeField] private List<Renderer> m_Renderers;

        [Header("DieSkin")]
        public SkinDieRespawn[] skinDieRespawnObjects;
        public Collider[] colliders; // Массив объектов с коллайдерами персонажа
        public PlayerView playerView;  // Переменная для хранения объекта с компонентом PlayerView


        private InputVR m_InputVR;
        private bool m_IsLocalPlayer = false;
        private bool m_IsInit = false;
        [SerializeField] private TeamType m_TeamType;

        public bool IsLocalPlayer => m_IsLocalPlayer;
        public bool IsInit => m_IsInit;
        public TeamType TeamType => m_TeamType;


        private void OnValidate()
        {
            if (m_WizardPlayer == null)
            {
                TryGetComponent(out m_WizardPlayer);
            }

            if (m_CharacterIK == null)
            {
                TryGetComponent(out m_CharacterIK);
            }

            if (m_Teammate == null)
            {
                TryGetComponent(out m_Teammate);
            }


        }


        private void Start()
        {
            // Находим объект InputVR
            _InputVR = GameObject.Find("InputVR");



            if (_InputVR != null)
            {
                // Получаем компонент ChangeTeam для смены цвета скина
                _ChangeTeam = _InputVR.GetComponent<ChangeTeam>();
            }

        }





        private void Update()
        {
            if (m_IsLocalPlayer && m_IsInit)
            {
                transform.position = m_InputVR.BngPlayerController.transform.position;
                transform.rotation = m_InputVR.BngPlayerController.transform.rotation;

                m_LeftHand.transform.position = m_InputVR.IKLeftHand.transform.position;
                m_LeftHand.transform.rotation = m_InputVR.IKLeftHand.transform.rotation;

                m_RightHand.transform.position = m_InputVR.IKRightHand.transform.position;
                m_RightHand.transform.rotation = m_InputVR.IKRightHand.transform.rotation;
            }
        }

        public void SetLocalPlayer()
        {
            photonView.RPC(nameof(SetLocalPlayer),RpcTarget.All);
        }





        //смена команды
        public void ChangeTeamOnClick()
        {
            // Переключаем команду на противоположную
            m_CurrentTeam = (m_CurrentTeam == TeamType.RED) ? TeamType.BLUE : TeamType.RED;

            // Применяем новую команду для всех объектов
            //эта функция меняет у игрока команду в WizardPlayer м Teammate
            SetTeam(m_CurrentTeam);

        }


        







        public void SetTeam(TeamType teamType)
        {
            m_TeamType = teamType;
            photonView.RPC(nameof(SetTeamRpc), RpcTarget.AllBuffered, teamType);
            //меняем цвет локального игрока

            // Находим объект InputVR
            _InputVR = GameObject.Find("InputVR");
            if (_InputVR != null)
            {
                // Получаем компонент ChangeTeam для смены цвета скина
                _ChangeTeam = _InputVR.GetComponent<ChangeTeam>();

                if (_ChangeTeam != null)
                {
                    // Выполняем функцию ChangeAllTeams
                    _ChangeTeam.ChangeAllTeams(teamType);
                }

            }
            
        }




        [PunRPC]
        public void SetTeamRpc(TeamType teamType)
        {
            m_TeamType = teamType;

            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.TeamType = m_TeamType;
            }

            if (m_Teammate != null)
            {
                m_Teammate.SetTeam(m_TeamType);
            }
        }



        public void SetLocalPlayer(InputVR inputVR)
        {
            m_IsLocalPlayer = true;
            m_InputVR = inputVR;
            
            if (m_InputVR == null)
            {
                m_InputVR = FindObjectOfType<InputVR>();
            }

            if (m_InputVR == null)
            {
                return;
            }

            if (m_CharacterIK != null)
            {
                m_CharacterIK.FollowHead = m_InputVR.IKHead;
                m_CharacterIK.FollowLeftController = m_InputVR.IKLeftHand;
                m_CharacterIK.FollowRightController = m_InputVR.IKRightHand;
                m_CharacterIK.FollowPlayer = m_InputVR.CharacterController;
                m_CharacterIK.IKActive = true;
            }

            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.SetLeftGrabber(m_InputVR.LeftGrabber,
                                              m_InputVR.LeftSmallFireballPoint,
                                              m_InputVR.LeftBigFireballPoint);
                
                m_WizardPlayer.SetRightGrabber(m_InputVR.RightGrabber,
                                               m_InputVR.RightSmallFireballPoint,
                                               m_InputVR.RightBigFireballPoint);
            }

            if (m_LeftHand != null)
            {
                m_LeftHand.handPoser = m_InputVR.LeftController.handPoser;
                m_LeftHand.grabber = m_InputVR.LeftController.grabber;
            }

            if (m_RightHand != null)
            {
                m_RightHand.handPoser = m_InputVR.RightController.handPoser;
                m_RightHand.grabber = m_InputVR.RightController.grabber;
            }

            if (m_Teammate != null)
            {
                TeamItem leftHandTheme = m_InputVR.LeftController.GetComponentInChildren<TeamItem>();
                if (leftHandTheme != null)
                {
                    leftHandTheme.SetTeam(m_TeamType);
                }
                
                TeamItem rightHandTheme = m_InputVR.RightController.GetComponentInChildren<TeamItem>();
                if (rightHandTheme != null)
                {
                    rightHandTheme.SetTeam(m_TeamType);
                }
            }

            


            if (!m_IsRender)
            {
                foreach (Renderer meshRenderer in m_Renderers)
                {
                    meshRenderer.enabled = false;
                }
            }

            m_IsInit = true;
        }


        //все игроки видят, как выполнилась эта функция
        public void DieRemote()
        {

            // Выполняем функцию Die() на каждом объекте в массиве
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //заменяем материало на прозрачный
                obj.Die();
            }

            //нужно отключить возможность стрельбы и коллайдер
           m_WizardPlayer.enabled = false;

            //foreach (Collider collider in colliders)
            //{
            //    collider.enabled = false; // Отключаем коллайдеры
            //}


            //нужно занести инфу о смерти игрока, а тому от кого прилетел последний шар внести инфу о убийстве

        }

        public void Respawn()
        {
            //будет возможность стрелять
            m_WizardPlayer.enabled = true;

            //Возвращаться видимость скина

            // Выполняем функцию Respawn() на каждом объекте в массиве
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //заменяем материало на прозрачный
                obj.Respawn();
            }

            //восстанавливаем цвет скин на локальной версии
            GameObject inputVRObject = GameObject.Find("InputVR");

            if (inputVRObject != null)
            {
                LocalVR localVR = inputVRObject.GetComponent<LocalVR>();

                if (localVR != null)
                {
                    //делаем прозрачный скин в локальной версии
                    localVR.RespawnLocal();
                }
            }



            //жизни 100 
            m_WizardPlayer.Respown();


            //if (playerView != null)
            //{


            //    // Вызываем функцию RpcSetHealth со значением 100
            //    playerView.RpcSetHealth(100f);
                
            //}

            
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ZonaRespown"))
            {
                Respawn(); // Вызываем функцию Respawn
            }
        }



    }
}
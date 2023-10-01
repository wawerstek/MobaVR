using System;
using Michsky.MUIP;
using MobaVR.Utils;
using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_NickNameInput;
        [SerializeField] private CustomDropdown m_TeamDropdown;
        [SerializeField] private CustomDropdown m_RoleDropdown;

        private ClassicGameSession m_GameSession;
        private PlayerVR m_PlayerVR;

        public PlayerVR PlayerVR
        {
            get => m_PlayerVR;
            set => m_PlayerVR = value;
        }

        private void OnDestroy()
        {
            if (m_PlayerVR != null)
            {
                m_PlayerVR.OnNickNameChange -= OnUpdateNickName;
                m_PlayerVR.OnRpcChangeTeam -= OnUpdateTeam;
                m_PlayerVR.ClassSwitcher.OnClassChange -= OnUpdateRole;
            }
        }

        private void Awake()
        {
            m_GameSession = FindObjectOfType<ClassicGameSession>();
        }

        private void Start()
        {
            if (m_PlayerVR != null)
            {
                SetPlayerData();
            }
        }

        private void SetPlayerData()
        {
            OnUpdateNickName(m_PlayerVR.PlayerData.NickName);
            m_PlayerVR.OnNickNameChange += OnUpdateNickName;

            OnUpdateTeam(m_PlayerVR.TeamType);
            m_PlayerVR.OnRpcChangeTeam += OnUpdateTeam;

            OnUpdateRole(m_PlayerVR.ClassSwitcher.CurrentIdClass);
            m_PlayerVR.ClassSwitcher.OnClassChange += OnUpdateRole;
        }

        public void ResetCalibration()
        {
            if (m_PlayerVR == null && m_PlayerVR.Calibration != null)
            {
                return;
            }

            m_PlayerVR.Calibration.ResetCalibration();
        }

        public void SetHeight()
        {
            if (m_PlayerVR == null && m_PlayerVR.Calibration != null)
            {
                return;
            }

            m_PlayerVR.Calibration.SetHeight();
        }

        public void UpdateNickName(string nickName)
        {
            if (m_PlayerVR == null)
            {
                return;
            }
            
            m_PlayerVR.SetNickName(nickName);
        }
        
        public void UpdateRole(string idRole)
        {
            if (m_PlayerVR == null)
            {
                return;
            }
            
            m_PlayerVR.ClassSwitcher.SetRole(idRole);
        }

        public void UpdateTeam(string teamName)
        {
            if (m_PlayerVR == null)
            {
                return;
            }

            TeamType teamType = teamName.Equals("red", StringComparison.OrdinalIgnoreCase)
                ? TeamType.RED
                : TeamType.BLUE;
            
            //m_PlayerVR.SetTeam(teamType);

            if (m_GameSession != null)
            {
                m_GameSession.SetTeam(teamType, m_PlayerVR);
            }
        }

        #region OnUpdate

        private void OnUpdateRole(string idRole)
        {
            int selectedItemIndex = 0;
            switch (idRole)
            {
                case (AppStrings.ID_ROLE_WIZARD):
                {
                    selectedItemIndex = 0;
                    break;
                }

                case (AppStrings.ID_ROLE_DEFENDER):
                {
                    selectedItemIndex = 1;
                    break;
                }

                case (AppStrings.ID_ROLE_ARCHER):
                {
                    selectedItemIndex = 2;
                    break;
                }
            }

            m_RoleDropdown.SetDropdownIndex(selectedItemIndex);
            m_RoleDropdown.UpdateItemLayout();
        }

        private void OnUpdateNickName(string nickName)
        {
            m_NickNameInput.text = m_PlayerVR.PlayerData.NickName;
        }

        private void OnUpdateTeam(TeamType teamType)
        {
            int selectedItemIndex = teamType == TeamType.BLUE ? 0 : 1;
            m_TeamDropdown.SetDropdownIndex(selectedItemIndex);
            m_TeamDropdown.UpdateItemLayout();
        }

        #endregion
    }
}
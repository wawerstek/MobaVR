using System.Collections;
using System.Collections.Generic;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobaVR
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private BNGPlayerController m_PlayerController;
        
        [SerializeField] private Material m_DayMaterial;
        [SerializeField] private Color m_DayColor;
        [SerializeField] private Material m_NightMaterial;
        [SerializeField] private Color m_NightColor;

        [ContextMenu("SetDay")]
        public void SetDay()
        {
            RenderSettings.skybox = m_DayMaterial;
            //RenderSettings.ambientEquatorColor = m_DayColor;
            RenderSettings.ambientSkyColor = m_DayColor;
        }

        [ContextMenu("SetNight")]
        public void SetNight()
        {
            RenderSettings.skybox = m_NightMaterial;
            //RenderSettings.ambientEquatorColor = m_NightColor;
            RenderSettings.ambientSkyColor = m_NightColor;
        }
        
        [ContextMenu("ResetPosition")]
        public void ResetPosition()
        {
            m_PlayerController.transform.position = Vector3.zero;
            m_PlayerController.transform.rotation = Quaternion.identity;
        }

        [ContextMenu("RestartScene")]
        public void RestartScene()
        {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        }
    }
}
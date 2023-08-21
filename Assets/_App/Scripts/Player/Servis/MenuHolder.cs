using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobaVR
{
    public class MenuHolder : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainPanel;
        [SerializeField] private GameObject m_LocationPanel;

        private GameObject m_LocationView;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene arg0)
        {
            if (m_LocationView == null)
            {
                return;
            }

            if (m_LocationView.TryGetComponent(out PhotonView photonView))
            {
                PhotonNetwork.Destroy(photonView);
            }
            else
            {
                Destroy(m_LocationView);
            }

            m_LocationView = null;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        public void SetLocationPanel(GameObject locationView)
        {
            m_LocationView = locationView;
            m_LocationView.transform.parent = m_LocationPanel.transform;
            m_LocationView.transform.localPosition = Vector3.zero;
            m_LocationView.transform.localScale = Vector3.one;
            m_LocationView.transform.localRotation = Quaternion.identity;
        }
    }
}
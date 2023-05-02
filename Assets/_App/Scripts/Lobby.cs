using UnityEngine;

namespace MobaVR
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private NetworkLobby m_NetworkLobby;

        private void Start()
        {
            m_NetworkLobby.Connect();
        }
    }
}
using MetaConference;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MobaVR
{
    public class GameSession : MonoBehaviourPunCallbacks
    {
        [Header("Network")]
        [SerializeField] private NetworkSession m_NetworkSession;
        [SerializeField] private InputVR m_InputVR;

        [Space]
        [Header("Players")]
        [SerializeField] private bool m_IsRedFirst = true;
        [SerializeField] private bool m_UseDifferentTeam = true;
        [SerializeField] private PlayerVR m_PlayerPrefab;
        [SerializeField] private Transform m_Position1;
        [SerializeField] private Transform m_Position2;

        private Player m_LocalPlayer;

        //public Player LocalPlayer => m_LocalPlayer;
        public Player LocalPlayer => photonView.Owner;

        private void Awake()
        {
            //Application.targetFrameRate = 60;
        }

        private void Start()
        {
            InitPlayer();
        }

        private void InitPlayer()
        {
            if (m_LocalPlayer != null)
            {
                return;
            }

            /*
            string prefabInputName = $"Players/InputVR_1";
            GameObject input = PhotonNetwork.Instantiate(prefabInputName, Vector3.zero, Quaternion.identity);
            if (input.TryGetComponent(out InputVR inputVR))
            {
                string prefabName = $"Players/PlayerVR_1";
                GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, m_InitPosition, Quaternion.identity);
                localPlayer.name += "_" + Random.Range(1, 1000);
                if (localPlayer.TryGetComponent(out PlayerVR playerVR))
                {
                    playerVR.SetLocalPlayer(inputVR);
                }
            }
            */

            string prefabName = $"Players/{m_PlayerPrefab.name}";
            Vector3 position;
            TeamType teamType = TeamType.RED;
            if (m_UseDifferentTeam)
            {
                int remainder = m_IsRedFirst ? 0 : 1;
                teamType = PhotonNetwork.CurrentRoom.PlayerCount % 2 == remainder
                    ? TeamType.RED
                    : TeamType.BLUE;
            }
            
            //GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, position, Quaternion.identity);

            GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
            localPlayer.name += "_" + Random.Range(1, 1000);
            if (localPlayer.TryGetComponent(out PlayerVR playerVR))
            {
                playerVR.SetTeam(teamType);
                playerVR.SetLocalPlayer(m_InputVR);
            }
        }
    }
}
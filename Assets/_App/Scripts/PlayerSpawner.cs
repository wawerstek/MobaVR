using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MobaVR
{
    public class PlayerSpawner : BasePlayerSpawner<PlayerVR>
    {
        [SerializeField] private InputVR m_InputVR;
        [SerializeField] private PlayerVR m_PlayerPrefab;
        public GameObject localPlayer;
        public ManagerDevice managerDevice; // Ссылка на объект ManagerDevice
        public GameObject EventSystemVR; //евент систем из других сцен

        private void Awake()
        {
            managerDevice = GameObject.Find("DeviceManager").GetComponent<ManagerDevice>();
        }

        public override PlayerVR SpawnPlayer(Team team)
        {
            string prefabName = $"Players/{m_PlayerPrefab.name}";

            if (managerDevice.PlayerCrate) // Проверяем, нужно ли создавать игрока
            {
                EventSystemVR.SetActive(true);
                localPlayer = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);

                localPlayer.name += "_" + Random.Range(1, 1000);

                if (localPlayer.TryGetComponent(out PlayerVR playerVR))
                {
                    playerVR.SetLocalPlayer(m_InputVR);
                    playerVR.InitPlayer();
                    playerVR.SetTeam(team);

                    return playerVR;
                }
            }
            else
            {
                EventSystemVR.SetActive(false);
            }

            return null;
        }
    }
}
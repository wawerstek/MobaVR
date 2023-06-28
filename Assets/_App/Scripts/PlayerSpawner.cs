using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class PlayerSpawner : BasePlayerSpawner<PlayerVR>
    {
        [SerializeField] private InputVR m_InputVR;
        [SerializeField] private PlayerVR m_PlayerPrefab;

        public override PlayerVR SpawnPlayer(Team team)
        {
            string prefabName = $"Players/{m_PlayerPrefab.name}";
            GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
            localPlayer.name += "_" + Random.Range(1, 1000);

            if (localPlayer.TryGetComponent(out PlayerVR playerVR))
            {
                playerVR.SetLocalPlayer(m_InputVR);
                playerVR.SetTeam(team);

                return playerVR;
            }

            return null;
        }
    }
}
using Michsky.MUIP;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class PlayerView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ProgressBar m_ProgressBar;
        [SerializeField] private Transform m_PointPlayer;

        private void Awake()
        {
            SetHealth(100f);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                Vector3 scale = transform.localScale;
                transform.parent = m_PointPlayer;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = scale;
            }
            else
            {
                transform.LookAt(Camera.main.transform);
            }
        }

        private void Update()
        {
            if (!photonView.IsMine && Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
            }
        }

        public void RpcSetHealth(float value)
        {
            photonView.RPC(nameof(SetHealth), RpcTarget.AllBuffered, value);
        }

        [PunRPC]
        public void SetHealth(float value)
        {
            if (value < 0)
            {
                value = 0f;
            }

            m_ProgressBar.currentPercent = value;
            m_ProgressBar.UpdateUI();
        }
    }
}
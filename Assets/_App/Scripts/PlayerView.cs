using Michsky.MUIP;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class PlayerView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ProgressBar m_ProgressBar;
        [SerializeField] private Transform m_PointPlayer;

        [SerializeField] private PlayerVR playerVR;

        [SerializeField] private float Life;


        private bool _Die;

        private void Awake()
        {
           
            SetHealth(100f);
            _Die=false;
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
            Life = value;

            if (value <= 0)
            {

                value = 0f;
                if (_Die == false)
                {
                    GameObject inputVRObject = GameObject.Find("InputVR");

                    if (inputVRObject != null)
                    {
                        LocalVR localVR = inputVRObject.GetComponent<LocalVR>();

                        if (localVR != null)
                        {
                            //������ ���������� ���� � ��������� ������
                            localVR.DieLocal();
                        }
                    }

                    //������ ���������� ���� � ������� ������
                    photonView.RPC(nameof(SetDie), RpcTarget.AllBuffered);
                    _Die = true;

                }
            }
            else
                {
                _Die = false;
            }

            //������ ����� �����
            photonView.RPC(nameof(SetHealth), RpcTarget.AllBuffered, value);
        }

        [PunRPC]
        public void SetHealth(float value)
        {

            m_ProgressBar.currentPercent = value;
            m_ProgressBar.UpdateUI();
        }


        [PunRPC]
        public void SetDie()
        {

            playerVR.DieRemote();
        }





    }
}
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
                if (m_PointPlayer == null)
                {
                    GameObject pointPlayer = GameObject.Find("PlayerViewPoint");
                    if (pointPlayer != null)
                    {
                        m_PointPlayer = pointPlayer.transform;
                    }
                }

                if (m_PointPlayer == null)
                {
                    return;
                }
                
                Vector3 scale = transform.localScale;
                transform.parent = m_PointPlayer;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = scale;
            }
            else
            {
                if (Camera.main != null)
                {
                    transform.LookAt(Camera.main.transform);
                }
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


            if (value < 0)
            {
                _Die = true;
                value = 0f;
                if (_Die == true)
                {
                    /*
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
                    */

                    //������ ���������� ���� � ������� ������
                    if (playerVR != null)
                    {
                        //photonView.RPC(nameof(SetDie), RpcTarget.AllBuffered);
                    }
                }
            }

            //������ ����� �����
            photonView.RPC(nameof(SetHealth), RpcTarget.AllBuffered, value);
        }

        [PunRPC]
        public void SetHealth(float value)
        {
            //float percent = value / m_ProgressBar.maxValue * 100;
            //m_ProgressBar.currentPercent = percent;
            m_ProgressBar.currentPercent = value;
            m_ProgressBar.UpdateUI();
        }
        
        [PunRPC]
        public void SetMaxHealth(float value)
        {
            m_ProgressBar.valueLimit = value;
            m_ProgressBar.maxValue = value;
            m_ProgressBar.currentPercent = value;
            m_ProgressBar.UpdateUI();
        }


        [PunRPC]
        public void SetDie()
        {

            playerVR.PlayerDie();
        }





    }
}
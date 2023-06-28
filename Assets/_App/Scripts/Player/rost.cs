using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace BNG
{
    public class rost : MonoBehaviourPun
    {
        [FormerlySerializedAs("ik_PUN")] [SerializeField] private VRIK[] m_IkElements;
        [SerializeField] private Transform m_LeftHand;
        [SerializeField] private Transform m_RightHand;
        
        [SerializeField] [ReadOnly] private float m_Height;

        public UnityEvent<float> OnCalibrated;
        
        public bool IsCalibrated { get; set; }
        
        private void Start()
        {
            IsCalibrated = false;
        }

        public void SetHeight(float height)
        {
            photonView.RPC(nameof(RpcSetHeight), RpcTarget.AllBuffered, height);
        }

        [PunRPC]
        public void RpcSetHeight(float height)
        {
            m_Height = height;
            if (m_Height > 0)
            {
                float fixHeight = m_Height + 0.08f;
                
                for (int i = 0; i < m_IkElements.Length; i++)
                {
                    m_IkElements[i].references.root.localScale = new Vector3(fixHeight, fixHeight, fixHeight);
                }

                float handScale = m_Height / 2f;
                m_LeftHand.transform.localScale = new Vector3(handScale, handScale, handScale);
                m_RightHand.transform.localScale = new Vector3(handScale, handScale, handScale);
            }
            
            OnCalibrated?.Invoke(height);
        }

        [PunRPC]
        public void RpcSetHeight_Deprecated(float rostValue)
        {
            //ôóíêöèÿ 

            //sizeF = (ik_PUN[1].solver.spine.headTarget.position.y - ik_PUN[1].references.root.position.y) / (ik_PUN[1].references.head.position.y - ik_PUN[1].references.root.position.y);


            //_Left_ruka.transform.localScale *= sizeF + 0.144f;
            //_Right_ruka.transform.localScale *= sizeF + 0.144f;
            //_Left_ruka.transform.localScale *= sizeF * 1f;
            //_Right_ruka.transform.localScale *= sizeF * 1f;


            //string My_Nick;
            //My_Nick = GetComponent<PhotonView>().Owner.NickName;


            //    var ldp = PhotonNetwork.PlayerList.ToList().Find(x => x.NickName == My_Nick);


            //if (ldp != null)//åñëè òàêîé îáúåêò ñóùåñòâóåò
            //    {
            //        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            //        _rost = (float)ldp.CustomProperties["Rost"];
            //        ldp.SetCustomProperties(h);
            //    }

            m_Height = rostValue;
            if (m_Height > 0)
            {
                //_Left_ruka.transform.localScale = new Vector3(-(_rost / 2f), _rost / 2f, _rost / 2f);
                //_Right_ruka.transform.localScale = new Vector3(_rost / 2f, _rost / 2f, _rost / 2f);

                //äåëàåì âñåì íàøèì ñêèíàì ðàçìåð
                for (int i = 0; i < m_IkElements.Length;)
                {
                    //Ñðàâíèòå âûñîòó ãîëîâû ìèøåíè ñ âûñîòîé êîñòè ãîëîâû, óìíîæüòå ìàñøòàá íà ýòî çíà÷åíèå.

                    //ik_PUN[i].references.root.localScale *= sizeF * 1; 

                    //ik_PUN[i].references.root.localScale *= _rost;
                    m_IkElements[i].references.root.localScale =
                        new Vector3(m_Height + 0.08f, m_Height + 0.08f, m_Height + 0.08f);
                    //ik_PUN[i].references.root.localScale = new Vector3(_rost, _rost, _rost);   - â äàííîì ñëó÷àå ðîñò íåìíîãî ìåíüøå ðåàëüíîãî, ïîýòîìó ïðèõîäèòñÿ äîáàâëÿòü 0.8

                    i++;
                }
            }


            return;
        }
    }
}
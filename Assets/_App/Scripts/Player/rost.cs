using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;
using System.Linq;
using Photon.Realtime;

namespace BNG
{
    public class rost : MonoBehaviourPun
    {

        public VRIK[] ik_PUN;//òåëî èãðîêà, êîòîðîå íóæíî óâåëè÷èòü
        //public float _rost_PUN;
        PhotonView PV;
        //êàê òîëüêî ñêðèïò îáó÷åíèå âûêëþ÷èòñÿ, ìû àêòèâèðóåì íàøå îðóæèå

        
        //public GameObject _Left_ruka;
        //public GameObject _Right_ruka;

        public float sizeF;



        public float _rost;
       
        
       // public GameObject _CenterEyeAnchor;
        public bool runRost;



        void Start()
        {
            PV = GetComponent<PhotonView>();
            runRost = false;

            //_Left_ruka.transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
           // _Right_ruka.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);



        }


        // Update is called once per frame
        void FixedUpdate()
        {

        }





        public void Rost_PUN(float rostValue)
        {
            //_Rost = ik_PUN[1].solver.spine.headTarget.position.y;


            //ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            //h.Add("Rost", PhotonNetwork.LocalPlayer.CustomProperties["Rost"] = _rost);
            //PhotonNetwork.LocalPlayer.SetCustomProperties(h);



            PV.RPC(nameof(RPC_ik_PUN), RpcTarget.AllBuffered, rostValue);

        }



        [PunRPC]
        public void RPC_ik_PUN(float rostValue)
        { //ôóíêöèÿ 

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

            _rost = rostValue;
            if (_rost > 0)
            {
                //_Left_ruka.transform.localScale = new Vector3(-(_rost / 2f), _rost / 2f, _rost / 2f);
                //_Right_ruka.transform.localScale = new Vector3(_rost / 2f, _rost / 2f, _rost / 2f);

                //äåëàåì âñåì íàøèì ñêèíàì ðàçìåð
                for (int i = 0; i < ik_PUN.Length;)
                {
                    //Ñðàâíèòå âûñîòó ãîëîâû ìèøåíè ñ âûñîòîé êîñòè ãîëîâû, óìíîæüòå ìàñøòàá íà ýòî çíà÷åíèå.

                    //ik_PUN[i].references.root.localScale *= sizeF * 1; 

                    //ik_PUN[i].references.root.localScale *= _rost;
                    ik_PUN[i].references.root.localScale = new Vector3(_rost+0.08f, _rost+0.08f, _rost+0.08f);
                    //ik_PUN[i].references.root.localScale = new Vector3(_rost, _rost, _rost);   - â äàííîì ñëó÷àå ðîñò íåìíîãî ìåíüøå ðåàëüíîãî, ïîýòîìó ïðèõîäèòñÿ äîáàâëÿòü 0.8

                    i++;
                }

            }


            return;

        }









    }
}
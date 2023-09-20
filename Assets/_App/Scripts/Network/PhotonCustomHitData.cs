using System;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using Object = UnityEngine.Object;

namespace MobaVR
{
    public static class PhotonCustomHitData
    {
        public static void Register()
        {
            PhotonPeer.RegisterType(typeof(HitData),
                                    (byte)'H',
                                    Serialize,
                                    Deserialize);
        }


        private static short size =
                sizeof(short) * 2 + // HitActionType + TeamType
                //sizeof(bool) * 2 + // CanApplyBySelf + CanApplyForTeammates
                sizeof(short) * 2 + // CanApplyBySelf + CanApplyForTeammates
                sizeof(float) * 1 + // Amount
                sizeof(float) * 3 + // Vector3 Position
                sizeof(int) * 1 * 3 // Player + PhotonViewOwner + PhotonView
            ;

        private static readonly byte[] m_MemData = new byte[size];

        private static short Serialize(StreamBuffer outStream, object customObject)
        {
            HitData hitData = (HitData)customObject;
            lock (m_MemData)
            {
                byte[] bytes = m_MemData;
                int index = 0;
                Protocol.Serialize((short)hitData.Action, bytes, ref index);
                
                Protocol.Serialize((short)(hitData.CanApplyBySelf ? 1 : 0), bytes, ref index);
                Protocol.Serialize((short)(hitData.CanApplyForTeammates ? 1 : 0), bytes, ref index);
                
                Protocol.Serialize(hitData.Amount, bytes, ref index);
                Protocol.Serialize(hitData.Position.x, bytes, ref index);
                Protocol.Serialize(hitData.Position.y, bytes, ref index);
                Protocol.Serialize(hitData.Position.z, bytes, ref index);
                Protocol.Serialize((short)hitData.TeamType, bytes, ref index);

                if (hitData.Player == null)
                {
                    // TODO: 
                    // Плеера можно найти всегда. Но у нас возможен вариант, когда WizardVR не будет существовать
                    // + существую предметы или зоны, которые дмажут и фотоны у них совпадают, поэтому игрок не получает урон у других игроков
                    // Protocol.Serialize(PhotonNetwork.LocalPlayer.ActorNumber, bytes, ref index);
                    Protocol.Serialize(-1, bytes, ref index);
                }
                else
                {
                    Protocol.Serialize(hitData.Player.ActorNumber, bytes, ref index);
                }

                if (hitData.PhotonOwner == null)
                {
                    Protocol.Serialize(-1, bytes, ref index);
                }
                else
                {
                    Protocol.Serialize(hitData.PhotonOwner.ViewID, bytes, ref index);
                }
                
                if (hitData.PhotonView == null)
                {
                    Protocol.Serialize(-1, bytes, ref index);
                }
                else
                {
                    Protocol.Serialize(hitData.PhotonView.ViewID, bytes, ref index);
                }

                outStream.Write(bytes, 0, size);
            }

            return size;
        }

        private static object Deserialize(StreamBuffer inStream, short length)
        {
            if (length != size)
            {
                return null;
            }

            HitData hitData = new HitData();
            lock (m_MemData)
            {
                inStream.Read(m_MemData, 0, length);
                int off = 0;

                Protocol.Deserialize(out short action, m_MemData, ref off);
                hitData.Action = (HitActionType)action;
                
                Protocol.Deserialize(out short canApplyBySelf, m_MemData, ref off);
                hitData.CanApplyBySelf = canApplyBySelf == 1;

                Protocol.Deserialize(out short canApplyForTeammates, m_MemData, ref off);
                hitData.CanApplyForTeammates = canApplyForTeammates == 1;
                
                Protocol.Deserialize(out hitData.Amount, m_MemData, ref off);

                Vector3 position;
                Protocol.Deserialize(out position.x, m_MemData, ref off);
                Protocol.Deserialize(out position.y, m_MemData, ref off);
                Protocol.Deserialize(out position.z, m_MemData, ref off);
                position.x = 100f;
                hitData.Position = position;

                Protocol.Deserialize(out short teamType, m_MemData, ref off);
                hitData.TeamType = (TeamType)teamType;

                Protocol.Deserialize(out int idPlayer, m_MemData, ref off);
                Player player = null;
                if (idPlayer != -1)
                {
                    player = PhotonNetwork.CurrentRoom.GetPlayer(idPlayer);
                }

                hitData.Player = player;
                if (player != null)
                {
                    PlayerVR[] players = GameObject.FindObjectsOfType<PlayerVR>();
                    foreach (PlayerVR playerVR in players)
                    {
                        if (playerVR.photonView.Owner.ActorNumber != player.ActorNumber)
                        {
                            continue;
                        }

                        hitData.PlayerVR = playerVR;
                        break;
                    }
                }

                Protocol.Deserialize(out int idPhotonOwner, m_MemData, ref off);
                hitData.PhotonOwner = idPhotonOwner > 0 ? PhotonView.Find(idPhotonOwner) : null;
                
                Protocol.Deserialize(out int idPhotonView, m_MemData, ref off);
                hitData.PhotonView = idPhotonView > 0 ? PhotonView.Find(idPhotonView) : null;
                
                hitData.DateTime = DateTime.Now;
            }

            return hitData;
        }
    }
}
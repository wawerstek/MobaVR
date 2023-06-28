using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{

    public class ChangeTeam : MonoBehaviour
    {
        [SerializeField] private List<TeamItem> teamItems = new List<TeamItem>();

        [SerializeField] private SkinCollection m_SkinCollection;
        
        public void ChangeAllTeams(TeamType teamType)
        {
            foreach (TeamItem teamItem in teamItems)
            {
                teamItem.SetTeam(teamType);
            }

            foreach (Skin skin in m_SkinCollection.Skins)
            {
                skin.SetTeam(teamType);
            }
        }
    }
}
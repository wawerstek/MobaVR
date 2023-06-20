using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{

    public class ChangeTeam : MonoBehaviour
    {
        [SerializeField] private List<TeamItem> teamItems = new List<TeamItem>();
        [SerializeField] private TeamType teamType;

        public void ChangeAllTeams(TeamType teamType)
        {
  

            // Ïðîõîäèìñÿ ïî âñåì îáúåêòàì è ìåíÿåì êîìàíäó
            foreach (TeamItem teamItem in teamItems)
            {
                teamItem.SetTeam(teamType);
            }

           
        }


    }
}
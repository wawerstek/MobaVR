using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    public class Teammate : TeamItem
    {
        [Header("Theme")]
        [SerializeField] private List<TeamItem> m_TeamItems = new();
        
        private void OnValidate()
        {
            if (m_TeamItems.Count == 0)
            {
                //m_TeamItems.AddRange(GetComponentsInChildren<TeamItem>());
            }
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            foreach (TeamItem teamItem in m_TeamItems)
            {
                teamItem.SetTeam(teamType);
            }
        }
    }
}
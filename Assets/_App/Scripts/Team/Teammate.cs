using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    /// <summary>
    /// Все элементы игрока кастомизирует под команду: красную или синию
    /// </summary>
    public class Teammate : TeamItem
    {

        [Header("Theme")]
        [SerializeField] private List<TeamItem> m_TeamItems = new ();
       // [SerializeField] private TeamType m_CurrentTeam = TeamType.RED;

        private void OnValidate()
        {
            //if (m_TeamItems.Count == 0)
            //{
            //    m_TeamItems.AddRange(GetComponentsInChildren<TeamItem>());
            //}

        }

        ////Функция обновления списка
        //private void UpdateTeamItems()
        //{
        //    m_TeamItems.Clear();
        //    m_TeamItems.AddRange(GetComponentsInChildren<TeamItem>());
        //}


        //public void ChangeTeamOnClick()
        //{
        //    // Переключаем команду на противоположную
        //    m_CurrentTeam = (m_CurrentTeam == TeamType.RED) ? TeamType.BLUE : TeamType.RED;

        //    // Применяем новую команду для всех объектов
        //    SetTeam(m_CurrentTeam);
        //}



        //эту функцию вызывать всегда, когда нужно применить цвет.
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
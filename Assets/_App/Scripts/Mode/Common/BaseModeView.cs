using UnityEngine;

namespace MobaVR
{
    public abstract class BaseModeView : MonoBehaviour
    {
        [SerializeField] protected BaseTeamScoreView m_RedTeamScoreView;
        [SerializeField] protected BaseTeamScoreView m_BlueTeamScoreView;
        [SerializeField] protected BaseTimeView m_RoundTimeView;
        [SerializeField] protected BaseVictoryView m_VictoryView;
        //[SerializeField] protected BaseTimeView m_GameTimeView;

        public BaseTeamScoreView RedTeamScoreView => m_RedTeamScoreView;
        public BaseTeamScoreView BlueTeamScoreView => m_BlueTeamScoreView;
        public BaseTimeView RoundTimeView => m_RoundTimeView;
        public BaseVictoryView VictoryView => m_VictoryView;
        //public BaseTimeView GameTimeView => m_GameTimeView;
    }
}
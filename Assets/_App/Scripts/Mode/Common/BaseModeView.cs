using UnityEngine;

namespace MobaVR
{
    public abstract class BaseModeView : MonoBehaviour
    {
        [SerializeField] protected BaseTeamScoreView m_RedTeamScoreView;
        [SerializeField] protected BaseTeamScoreView m_BlueTeamScoreView;
        [SerializeField] protected BaseTeamScoreView m_RedTeamKillScoreView;
        [SerializeField] protected BaseTeamScoreView m_BlueTeamKillScoreView;
        [SerializeField] protected BaseTimeView m_PreRoundTimeView;
        [SerializeField] protected BaseTimeView m_RoundTimeView;
        [SerializeField] protected BaseVictoryView m_VictoryView;
        //[SerializeField] protected BaseTimeView m_GameTimeView;

        public BaseTeamScoreView RedTeamScoreView => m_RedTeamScoreView;
        public BaseTeamScoreView BlueTeamScoreView => m_BlueTeamScoreView;
        public BaseTimeView RoundTimeView => m_RoundTimeView;
        public BaseTimeView PreRoundTimeView => m_PreRoundTimeView;
        public BaseVictoryView VictoryView => m_VictoryView;
        public BaseTeamScoreView RedTeamKillScoreView => m_RedTeamKillScoreView;
        public BaseTeamScoreView BlueTeamKillScoreView => m_BlueTeamKillScoreView;
        
        //public BaseTimeView GameTimeView => m_GameTimeView;
    }
}
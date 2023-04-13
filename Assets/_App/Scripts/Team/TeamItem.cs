using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public abstract class TeamItem : MonoBehaviour, ITeamItem
    {
        [SerializeField] [ReadOnly] protected TeamType m_TeamType;

        public TeamType TeamType => m_TeamType;

        public virtual void SetTeam(TeamType teamType)
        {
            m_TeamType = teamType;
        }

        public bool IsRed => m_TeamType == TeamType.RED;
        public bool IsBlue => m_TeamType == TeamType.BLUE;
    }
}
using UnityEngine;

namespace MobaVR
{
    public class TowerModeView : BaseModeView
    {
        [SerializeField]private MonsterCountView m_MonsterCountView;
        public MonsterCountView MonsterCountView => m_MonsterCountView;
    }
}
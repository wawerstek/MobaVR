using MobaVR.Content;
using UnityEngine;

namespace MobaVR
{
    public class TowerMode : GameMode
    {
        [SerializeField] private TowerModeContent m_Content;

        public TowerModeContent Content => m_Content;

        protected override void Awake()
        {
            base.Awake();
            InitStateMachine();
        }

        public override void InitStateMachine()
        {
            m_StateMachine.Init(this);
        }
    }
}
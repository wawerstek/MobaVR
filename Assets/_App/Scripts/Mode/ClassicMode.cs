using MobaVR.Content;
using UnityEngine;

namespace MobaVR
{
    public class ClassicMode : GameMode
    {
        [SerializeField] private ClassicModeContent m_Content;

        public ClassicModeContent Content => m_Content;

        private void Awake()
        {
            InitStateMachine();
        }

        public override void InitStateMachine()
        {
            m_StateMachine.Init(this);
        }
    }
}
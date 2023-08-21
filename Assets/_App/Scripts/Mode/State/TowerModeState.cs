using MobaVR.ClassicModeStateMachine;
using MobaVR.ClassicModeStateMachine.PVP;
using MobaVR.Content;

namespace MobaVR
{
    public abstract class TowerModeState : ModeState
    {
        protected TowerModeContent m_Content;
        
        public override void Init(GameMode mode)
        {
            base.Init(mode);
            if (!mode.TryGetComponent(out m_Content))
            {
                m_Content = FindObjectOfType<TowerModeContent>();
            }
        }
    }
}
using MobaVR.ClassicModeStateMachine;
using MobaVR.ClassicModeStateMachine.PVP;
using MobaVR.Content;

namespace MobaVR
{
    public abstract class PveModeState : ModeState
    {
        protected PveModeContent m_Content;
        
        public override void Init(GameMode mode)
        {
            base.Init(mode);
            if (!mode.TryGetComponent(out m_Content))
            {
                m_Content = FindObjectOfType<PveModeContent>();
            }
        }
    }
}
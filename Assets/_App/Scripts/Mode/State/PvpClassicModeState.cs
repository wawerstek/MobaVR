using MobaVR.ClassicModeStateMachine;
using MobaVR.Content;

namespace MobaVR
{
    public abstract class PvpClassicModeState : ModeState
    {
        protected ClassicModeContent m_Content;
        
        public override void Init(GameMode mode)
        {
            base.Init(mode);
            mode.TryGetComponent(out m_Content);
        }
    }
}
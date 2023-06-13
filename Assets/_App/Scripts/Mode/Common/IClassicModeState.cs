namespace MobaVR
{
    public interface IClassicModeState
    {
        public void InitMode();
        public void DeactivateMode();
        public void StartMode();
        public void ReadyRound();
        public void PlayRound();
        public void CompleteRound();
        public void CompleteMode();
    }
}
namespace MobaVR
{
    public interface IMonsterAnimatorListener
    {
        public void StartActivation();
        public void CompleteActivation();

        public void StartAttack();
        public void CompleteAttack();
    }
}
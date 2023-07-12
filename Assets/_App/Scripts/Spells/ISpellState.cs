namespace MobaVR
{
    public interface ISpellState
    {
        public bool IsPerformed();
        public bool TryInterrupt();
    }
}
namespace MobaVR
{
    public class PlayerDamageable : DamageablePun
    {
        private WizardPlayer m_WizardPlayer;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out m_WizardPlayer);
        }
    }
}
namespace MobaVR
{
    public class EmptySkinOnDie : OnDieBehaviour
    {
        public override void Die()
        {
            m_Skin.gameObject.SetActive(false);
        }

        public override void Reborn()
        {
            m_Skin.gameObject.SetActive(true);
        }
    }
}
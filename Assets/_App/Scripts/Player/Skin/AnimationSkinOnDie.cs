namespace MobaVR
{
    public class AnimationSkinOnDie : OnDieBehaviour
    {
        public override void Die()
        {
            m_Skin.gameObject.SetActive(true);

            m_Animator.enabled = true;
            m_Vrik.enabled = false;
            m_EyeAnimationHandler.enabled = false;

            m_Animator.SetTrigger("Die");

            if (m_HideTimeout > 0)
            {
                m_Skin.StartCoroutine(WaitAndHideSkin());
            }
            else
            {
                m_Skin.gameObject.SetActive(false);
            }
        }

        public override void Reborn()
        {
            if (m_HideTimeout > 0)
            {
                //m_Skin.StopCoroutine(WaitAndHideSkin());
                m_Skin.StopAllCoroutines();
            }
            
            m_Skin.gameObject.SetActive(true);
            
            m_Animator.enabled = true;
            m_Vrik.enabled = true;
            m_EyeAnimationHandler.enabled = true;

            m_Animator.ResetTrigger("Die");
            m_Animator.SetTrigger("Reborn");
        }
    }
}
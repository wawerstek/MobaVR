using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class RoundTimeView : BaseTimeView
    {
        [SerializeField] private TextMeshPro m_TimeText;

        public override void Show()
        {
            gameObject.SetActive(true);
            m_TimeText.enabled = true;
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            m_TimeText.enabled = false;
        }
        
        public override void UpdateTime(float time)
        {
            if (time < 0)
            {
                time = 0f;
            }

            m_TimeText.text = $"{time:F1}";
        }
    }
}
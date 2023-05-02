using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TimeText;
        [SerializeField] private float m_HideDelay = 2.5f;

        private bool m_IsShow = false;
        private float m_Time = 0f;

        private void Awake()
        {
            m_TimeText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (m_IsShow)
            {
                //TODO: bag with time
                m_Time -= Time.deltaTime;

                if (m_Time <= 0)
                {
                    m_Time = 0f;
                    Show(false);
                }

                m_TimeText.text = $"{m_Time:F} sec";
            }
        }

        public void SetTime(float time)
        {
            //TODO: bag with time
            //m_Time = time;
            m_Time = time + 1f;
        }

        public void Show(bool isShow)
        {
            if (m_IsShow == isShow)
            {
                return;
            }

            m_IsShow = isShow;
            m_TimeText.gameObject.SetActive(m_IsShow);

            if (m_IsShow)
            {
                Invoke(nameof(Hide), m_HideDelay);
            }
        }

        private void Hide()
        {
            m_IsShow = false;
            m_TimeText.gameObject.SetActive(m_IsShow);
        }
    }
}
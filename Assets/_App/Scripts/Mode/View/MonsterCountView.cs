using System;
using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class MonsterCountView : BaseTextView
    {
        [SerializeField] private TextMeshPro m_TextView;

        public override void Show()
        {
            gameObject.SetActive(true);
            m_TextView.enabled = true;
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            m_TextView.enabled = false;
        }

        public override void UpdateText(string message)
        {
            m_TextView.text = message;
        }
    }
}
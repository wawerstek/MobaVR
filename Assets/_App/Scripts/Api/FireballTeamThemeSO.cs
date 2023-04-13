using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "Fireball", menuName = "MobaVR API/Create big fireball theme")]
    public class FireballTeamThemeSO : ScriptableObject
    {
        [SerializeField] private Material m_Material;
        [SerializeField] private Color m_MainColor;
        [SerializeField] private Color m_SubColor;
        [SerializeField] private Color m_TransparentColor;
        [SerializeField] private Gradient m_Gradient;

        public Material Material => m_Material;
        public Color MainColor => m_MainColor;
        public Color SubColor => m_SubColor;
        public Color TransparentColor => m_TransparentColor;
        public Gradient Gradient => m_Gradient;
    }
}
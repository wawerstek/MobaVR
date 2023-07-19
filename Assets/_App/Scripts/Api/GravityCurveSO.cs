using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "GravityCurve", menuName = "MobaVR API/Create gravity curve")]
    public class GravityCurveSO : ScriptableObject
    {
        [SerializeField] private AnimationCurve m_AnimationCurve;

        public AnimationCurve AnimationCurve => m_AnimationCurve;
    }
}
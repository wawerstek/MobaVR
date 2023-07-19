using CloudFine.ThrowLab;
using UnityEngine;

namespace MobaVR
{
    [CreateAssetMenu(fileName = "ThrowConfigurationSet", menuName = "MobaVR API/Create Throw Configuration Set")]

    public class ThrowConfigurationSetSO : ScriptableObject
    {
        [SerializeField] private ThrowConfigurationSet m_ThrowConfigurationSet;
        
        public ThrowConfigurationSet ThrowConfigurationSet => m_ThrowConfigurationSet;
    }
}
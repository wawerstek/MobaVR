using UnityEngine;

namespace MobaVR
{
    public class EnvironmentMode : BaseEnvironmentMode
    {
        //[SerializeField] private DestroyPropCollection m_DestroyPropCollection;

        public override void ResetEnvironment()
        {
            DestroyPropCollection destroyPropCollection = FindObjectOfType<DestroyPropCollection>();
            
            if (destroyPropCollection != null)
            {
                destroyPropCollection.Restore();
            }
        }
    }
}
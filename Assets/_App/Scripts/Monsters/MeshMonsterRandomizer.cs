using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    public class MeshMonsterRandomizer : MonoBehaviour
    {
        [SerializeField] private bool m_UseRandomMeshes = true;
        [SerializeField] private List<GameObject> m_Meshes = new();
        
        private void Awake()
        {
            if (m_UseRandomMeshes)
            {
                foreach (GameObject mesh in m_Meshes)
                {
                    mesh.SetActive(Random.value > 0.4f);
                }
            }
        }
    }
}
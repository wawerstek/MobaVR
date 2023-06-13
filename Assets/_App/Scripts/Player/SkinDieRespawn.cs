using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDieRespawn : MonoBehaviour
{
    [SerializeField] private Material[] initialMaterials;  // Изначальные материалы объекта
    [SerializeField] private Material[] replacementMaterials;  // Материалы для замены

    private SkinnedMeshRenderer skinnedMeshRenderer;  // Компонент SkinnedMeshRenderer

    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void Die()
    {
        initialMaterials = skinnedMeshRenderer.materials;

        if (replacementMaterials.Length > 0)
        {
            skinnedMeshRenderer.materials = replacementMaterials;
        }
    }

    public void Respawn()
    {
        if (initialMaterials.Length > 0)
        {
            skinnedMeshRenderer.materials = initialMaterials;
        }
    }
}
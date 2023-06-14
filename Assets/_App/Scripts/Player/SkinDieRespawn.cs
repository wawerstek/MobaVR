using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDieRespawn : MonoBehaviour
{
    [SerializeField] private Material[] initialMaterials;  // Èçíà÷àëüíûå ìàòåðèàëû îáúåêòà
    [SerializeField] private Material[] replacementMaterials;  // Ìàòåðèàëû äëÿ çàìåíû

    private SkinnedMeshRenderer skinnedMeshRenderer;  // Êîìïîíåíò SkinnedMeshRenderer
    private bool isDie = false;
    
    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        initialMaterials = skinnedMeshRenderer.materials;
    }

    [ContextMenu("Die")]
    public void Die()
    {
        if (isDie)
        {
            return;
        }
        
        isDie = true;

        initialMaterials = skinnedMeshRenderer.materials;
        if (replacementMaterials.Length > 0)
        {
            skinnedMeshRenderer.materials = replacementMaterials;
        }
    }

    [ContextMenu("Respawn")]
    public void Respawn()
    {
        isDie = false;
        
        if (initialMaterials.Length > 0)
        {
            skinnedMeshRenderer.materials = initialMaterials;
        }
    }
}
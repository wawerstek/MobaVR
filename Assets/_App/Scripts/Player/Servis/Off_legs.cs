using System.Collections;
using System.Collections.Generic;
using MobaVR;
using UnityEngine;


public class Off_legs : MonoBehaviour
{
    [SerializeField] private GameObject[] Legs;

    [SerializeField] private SkinCollection m_SkinCollection;
    
    public void ToggleObjects(bool isActive)
    {
        foreach (GameObject obj in Legs)
        {
            obj.SetActive(isActive);
        }
        
        foreach (Skin skin in m_SkinCollection.Skins)
        {
            skin.SetVisibilityLegs(isActive);
        }
    }
}

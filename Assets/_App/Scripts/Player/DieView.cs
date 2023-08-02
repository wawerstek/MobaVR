using MobaVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieView : MonoBehaviour
{
    [SerializeField] private WizardPlayer wizardPlayer;

    private void OnEnable()
    {
        wizardPlayer.OnDie += OnDie;
        wizardPlayer.OnReborn += OnReborn;
    }

    private void OnDisable()
    {
        wizardPlayer.OnDie -= OnDie;
        wizardPlayer.OnReborn -= OnReborn;
    }

    private void OnDie()
    {
        
    }

    private void OnReborn()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using MobaVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieView : MonoBehaviour
{
    [SerializeField] private WizardPlayer wizardPlayer;

    public GameObject TextDie;

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
        TextDie.SetActive(true);
    }

    private void OnReborn()
    {
        TextDie.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using BNG;

public class Urok_02 : MonoBehaviour
{
    [SerializeField]
    private Transform targetRed; // Точка для команды "RED"

    [SerializeField]
    private Transform targetBlue; // Точка для команды "BLUE"

    private CharacterActions characterActions;

    public PlayerSpawner spawnerReference;  //Skript na kotorom visit nash personazh
    public string team; //tip komandy
    private void Awake()
    {
        characterActions = GetComponent<CharacterActions>();
    }

    private void Start()
    {
        //poluchaem komandu.
        if (spawnerReference.localPlayer != null)
        {
            PlayerVR playerScript = spawnerReference.localPlayer.GetComponent<PlayerVR>();
            if (playerScript != null)
            {
                //team = playerScript.m_Teammate;
                
                if (playerScript.TeamType == TeamType.RED)
                {
                    team = "RED";
                }

                if (playerScript.TeamType == TeamType.BLUE)
                {
                    team = "BLUE";
                }


            }
        }
    }

    private void Update()
    {
        // Проверка на второй шаг обучения
        if(characterActions.CurrentStepIndex == 1)
        {
            SetTargetPointBasedOnTeam();
        }
    }

    private void SetTargetPointBasedOnTeam()
    {
        

        if (team == "RED")
        {
            characterActions.tutorialSteps[characterActions.CurrentStepIndex].targetPoint = targetRed;
        }
        else if (team == "BLUE")
        {
            characterActions.tutorialSteps[characterActions.CurrentStepIndex].targetPoint = targetBlue;
        }
    }
    
    
    public void FinishUrok02()
    {
        if(characterActions.tutorialSteps.Length > 1) // проверка, чтобы убедиться, что у нас есть хотя бы один шаг обучения
        {
            characterActions.tutorialSteps[1].isTaskCompleted = true;
            
        }
    }
    
    
}

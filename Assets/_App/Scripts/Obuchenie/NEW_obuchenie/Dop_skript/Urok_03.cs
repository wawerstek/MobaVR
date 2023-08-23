using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using BNG;

public class Urok_03 : MonoBehaviour
{
    [System.Serializable]
    public class SoundEntry
    {
        public string soundName;
        public AudioClip soundClip;
    }

    public SaveInfoClass _SaveInfoClass;//tut poluchaem klass igroka
    public SoundEntry[] soundEntries; // Массив для записей звука
    public string ID; // Переменная ID для идентификации звука

    public GameObject[] objectsToCheck;

    [SerializeField]
    private Transform targetRed;

    [SerializeField]
    private Transform targetBlue;

    private CharacterActions characterActions;

    public PlayerSpawner spawnerReference;  // Скрипт, на котором находится наш персонаж
    public string team;

    private void Awake()
    {
        characterActions = GetComponent<CharacterActions>();
    }

    private void Start()
    {
        if (spawnerReference.localPlayer != null)
        {
            PlayerVR playerScript = spawnerReference.localPlayer.GetComponent<PlayerVR>();
            if (playerScript != null)
            {
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
        if (characterActions.CurrentStepIndex == 2)
        {
            CheckObjects();
            SetTargetPointBasedOnTeam();
            ID = _SaveInfoClass.targetID;
        }
        AssignSoundByID();
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

    void CheckObjects()
    {
        bool allDisabled = true;
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.activeInHierarchy)
            {
                allDisabled = false;
                break;
            }
        }

        if (allDisabled)
        {
            Debug.Log("Все объекты выключены");
            FinishUrok03();
            StartCoroutine(EnableObjectsAfterDelay(20f));
        }
    }

    IEnumerator EnableObjectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject obj in objectsToCheck)
        {
            obj.SetActive(true);
        }
    }

    public void FinishUrok03()
    {
        if (characterActions.tutorialSteps.Length > 2)
        {
            characterActions.tutorialSteps[2].isTaskCompleted = true;
        }
    }

    // Функция для установки звука на основе ID
    void AssignSoundByID()
    {
        
        // Если сейчас идёт не второй урок, выйти из функции
        if(characterActions.CurrentStepIndex != 2)
        {
          
            return;
        }
        
        foreach (SoundEntry entry in soundEntries)
        {
            if (entry.soundName == ID)
            {
                if (characterActions.CurrentStepIndex < characterActions.tutorialSteps.Length)
                {
                    characterActions.tutorialSteps[characterActions.CurrentStepIndex].mainTaskSound = entry.soundClip;
                }

                break;
            }
        }
    }
}

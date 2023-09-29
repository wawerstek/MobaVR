using System;
using MobaVR;
using UnityEngine.UI;
using Photon.Pun;
using Random = UnityEngine.Random;

public class NameSetter : MonoBehaviourPunCallbacks
{
    public InputField nameInputField;
    public string[] randomNames = { "Bobik", "Albert", "KlavaKoka", "Muromec", "Kengoru" }; // Пример списка

    public void SetPlayerName()
    {
        string enteredName = nameInputField.text;

        if (string.IsNullOrEmpty(enteredName))
        {
            enteredName = randomNames[Random.Range(0, randomNames.Length)];
        }

        string randomSuffix = Random.Range(1000, 9999).ToString();
        string fullName = enteredName + randomSuffix;

        BaseGameSession gameSession = FindObjectOfType<BaseGameSession>();
        if (gameSession == null || gameSession.LocalPlayer == null)
        {
            return;
        }

        if (gameSession.LocalPlayer != null)
        {
            gameSession.LocalPlayer.SetNickName(fullName);
        }
        
        /*
        if (gameSession.LocalPlayer.TryGetComponent(out NameDisplay nameDisplay))
        {
            nameDisplay.SetName(fullName);
        }
        */
        //PhotonNetwork.LocalPlayer.NickName = fullName;
    }
}
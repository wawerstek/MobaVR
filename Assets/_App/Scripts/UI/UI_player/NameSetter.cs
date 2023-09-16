using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

        PhotonNetwork.LocalPlayer.NickName = fullName;
    }
}
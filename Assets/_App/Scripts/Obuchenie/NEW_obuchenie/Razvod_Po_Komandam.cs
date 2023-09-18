using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
public class Razvod_Po_Komandam : MonoBehaviour
{
    public GameObject BazaRed;
    public GameObject BazaBlue;
    
    public PlayerSpawner spawnerReference;  //Skript na kotorom visit nash personazh
   
    
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void RunZona()
    {

        //Получаем команду
        //if (spawnerReference.localPlayer != null)
        PlayerVR playerScript = null;
        
        {
            PlayerVR[] players = FindObjectsOfType<PlayerVR>();
            foreach (PlayerVR playerVR in players)
            {
                if (playerVR.photonView.IsMine)
                {
                    playerScript = playerVR;
                    break;
                }
            }

            if (playerScript == null)
            {
                playerScript = spawnerReference.localPlayer.GetComponent<PlayerVR>();
            }

            if (playerScript != null)
            {
                //team = playerScript.m_Teammate;
                
                if (playerScript.TeamType == TeamType.RED)
                {
                    BazaRed.SetActive(true); 
                    BazaBlue.SetActive(false);
                }

                if (playerScript.TeamType == TeamType.BLUE)
                {
                    BazaRed.SetActive(false); 
                    BazaBlue.SetActive(true);
                }


            }
        }
                
    }
    
    

}

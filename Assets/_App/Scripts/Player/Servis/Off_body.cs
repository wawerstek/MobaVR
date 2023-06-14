using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ñêðèïò ïîëó÷àåò êîìàíäó ïî ïîòêëþ÷åíèþ îòîáðàæåíèÿ òåëà ëîêàëüíîãî èãðîêà. Ïîëó÷àåò êîìàíäó èç ãëàâíîãî ìåíþ àäìèíà
public class Off_body : MonoBehaviour
{

    public ChangeSkinPlayer _ChangeSkinPlayer; // Ññûëêà íà ñêðèïò, ñîäåðæàùèé ôóíêöèþ SkinON

    public GameObject[] objectsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void DisableAllObjects(bool isActive)
    //{
    //    foreach (GameObject obj in objectsToDisable)
    //    {
    //        obj.SetActive(isActive); // Âûêëþ÷àåì èëè âêëþ÷àåì îáúåêò 
    //    }
    //}

    public void DisableAllObjects(bool isActive)
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            GameObject obj = objectsToDisable[i];

            if (isActive)
            {
                if (i<2)
                {
                    obj.SetActive(true); // Âêëþ÷àåì ïåðâûå 2 îáúåêòà
                }
                _ChangeSkinPlayer.SkinON();
            }
            else
            {
                obj.SetActive(false); // Âûêëþ÷àåì âñå îáúåêòû
            }
        }
    }



}

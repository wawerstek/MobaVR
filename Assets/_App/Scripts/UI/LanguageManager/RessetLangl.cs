using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessetLangl : MonoBehaviour
{
    //скрипт который обновит текст, при смене языка.
    public GameObject RessetText;

    public void UpdateText()
    {
        RessetText.SetActive(false);
        RessetText.SetActive(true);
        
    }
}

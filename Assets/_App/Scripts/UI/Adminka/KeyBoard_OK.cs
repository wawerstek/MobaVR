using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard_OK : MonoBehaviour
{
    public GameObject keyboard;
    // Start is called before the first frame update
    public void ok()
    {
        keyboard.SetActive(false);
    }

}

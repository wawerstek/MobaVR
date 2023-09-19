using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RusEng : MonoBehaviour
{

    public GameObject Rus;
    public GameObject Eng; 
    public GameObject Secondary;
  
    // Start is called before the first frame update
    void Start()
    {
      LanguageManager.Instance.LanguageChanged += UpdateText;
      UpdateText();

    }


    private void UpdateText()
    {
        switch (LanguageManager.Instance.currentLanguage)
        {
            case LanguageManager.Language.Eng:
                Rus.SetActive(false);
                Eng.SetActive(true);
                Secondary.SetActive(false);
                break;
            case LanguageManager.Language.Rus:
                Rus.SetActive(true);
                Eng.SetActive(false);
                Secondary.SetActive(false);
                break;
            case LanguageManager.Language.Chn:
                Rus.SetActive(false);
                Eng.SetActive(true);
                Secondary.SetActive(false);
                break;
        }
    }


    private void OnDestroy()
    {
        LanguageManager.Instance.LanguageChanged -= UpdateText;
    }

    public void  swichlengRus()
    {
        Rus.SetActive(true);
        Eng.SetActive(false);
        Secondary.SetActive(false);
    }    
    
    public void  swichlengEng()
    {
        Rus.SetActive(false);
        Eng.SetActive(true);
        Secondary.SetActive(false);
    }    
    
    public void  swichSecondary()
    {
        Rus.SetActive(false);
        Eng.SetActive(false);
        Secondary.SetActive(true);
    }
    
}

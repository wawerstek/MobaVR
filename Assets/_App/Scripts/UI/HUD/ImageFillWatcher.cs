using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillWatcher : MonoBehaviour
{
    public Image targetImage; // картинка которую проверяем на заполняемость
    public Image controllerImage; // картинка c контроллером
    public ParticleSystem m_ParticleSystem; // Объект, FX

    private bool isFillActivated = false;
    private bool isProgressActivated = false;

    private void OnEnable()
    {
        isFillActivated = false;
        isProgressActivated = false;
    }

    private void Update()
    {
        CheckImageFill();
    }

    private void CheckImageFill()
    {
        if (!isFillActivated && targetImage.fillAmount >= 1.0f)
        {
            isFillActivated = true;
            isProgressActivated = false;
            
            m_ParticleSystem.Play();
            controllerImage.gameObject.SetActive(true);
            return;
        }

        if (!isProgressActivated && targetImage.fillAmount < 1.0f)
        {
            isProgressActivated = true;
            isFillActivated = false;
            
            m_ParticleSystem.Stop();
            controllerImage.gameObject.SetActive(false);
            return;
        }
    }
}
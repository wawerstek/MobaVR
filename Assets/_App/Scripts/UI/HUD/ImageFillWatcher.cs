using UnityEngine;
using UnityEngine.UI;

public class ImageFillWatcher : MonoBehaviour
{
    public Image targetImage; // картинка которую проверяем на заполняемость
    public GameObject objectToToggle; // Объект, FX

    private void Update()
    {
        CheckImageFill();
    }

    private void CheckImageFill()
    {
        if (targetImage.fillAmount >= 1.0f)
        {
            objectToToggle.SetActive(true);
        }
        else
        {
            objectToToggle.SetActive(false);
        }
    }
}
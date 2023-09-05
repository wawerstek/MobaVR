using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Массив камер, которые вы хотите использовать
    public RawImage cameraDisplay; // UI элемент для отображения видов с камер

    private int currentCameraIndex = 0; // Индекс текущей активной камеры

    private void Start()
    {
        // При запуске устанавливаем начальную камеру
        SwitchCamera(currentCameraIndex);
    }

    public void SwitchCamera(int newIndex)
    {
        if (newIndex >= 0 && newIndex < cameras.Length)
        {
            // Отключаем предыдущую камеру
            cameras[currentCameraIndex].enabled = false;

            // Включаем новую камеру
            currentCameraIndex = newIndex;
            cameras[currentCameraIndex].enabled = true;

            // Покажем вид с новой камеры на элементе UI
            cameraDisplay.texture = cameras[currentCameraIndex].targetTexture;
        }
    }

    public void NextCamera()
    {
        int newIndex = (currentCameraIndex + 1) % cameras.Length;
        SwitchCamera(newIndex);
    }

    public void PreviousCamera()
    {
        int newIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length;
        SwitchCamera(newIndex);
    }
}
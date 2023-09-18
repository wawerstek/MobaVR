using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morganie_zon : MonoBehaviour
{
    private Renderer objectRenderer;
    private bool isRendererActive = true;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Объект не имеет компонента Renderer!");
            enabled = false; // отключаем этот скрипт, чтобы избежать ошибок
            return;
        }

        InvokeRepeating("ToggleRendererVisibility", 1f, 1f);
    }

    private void ToggleRendererVisibility()
    {
        objectRenderer.enabled = !objectRenderer.enabled;
    }
}
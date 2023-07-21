using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using RootMotion.FinalIK;
using BNG;

public class ServiseSkin : MonoBehaviour
{

    public GameObject targetObject; // GameObject, для которого нужно найти дочерние элементы
    public Transform destinationParent; // Родительский элемент для созданных копий


    private void Start()
    {
        CopyAllChildren();
    }

    private void CopyAllChildren()
    {
        // Рекурсивно копируем все дочерние объекты
        CopyChildren(targetObject.transform, destinationParent);
    }

    private void CopyChildren(Transform source, Transform parent)
    {
        foreach (Transform child in source)
        {
            // Создаем копию дочернего объекта
            GameObject copiedChild = Instantiate(child.gameObject, parent);
            copiedChild.transform.localPosition = Vector3.zero; // Сбрасываем локальную позицию
            copiedChild.transform.localRotation = Quaternion.identity; // Сбрасываем локальный поворот
            copiedChild.transform.localScale = Vector3.one; // Сбрасываем локальный масштаб

            // Рекурсивно копируем дочерние объекты этого объекта
            CopyChildren(child, copiedChild.transform);

            // Выключаем компонент VR IK
            VRIK vrIK = copiedChild.GetComponent<VRIK>();
            if (vrIK != null)
            {
                vrIK.enabled = false;
            }

            // Выключаем компонент VR IK
            IKDummy ikDummy = copiedChild.GetComponent<IKDummy>();
            if (ikDummy != null)
            {
                ikDummy.enabled = true;
            }


            // Выключаем скопированный объект
            copiedChild.gameObject.SetActive(false);

        }
    }




}
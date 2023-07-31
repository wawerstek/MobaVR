using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using RootMotion.FinalIK;
using BNG;

public class ServiseSkin : MonoBehaviour
{

    public GameObject targetObject; // GameObject, ��� �������� ����� ����� �������� ��������
    public Transform destinationParent; // ������������ ������� ��� ��������� �����
    

    private void Start()
    {
        CopyAllChildren();
    }

    private void CopyAllChildren()
    {
        // ���������� �������� ��� �������� �������
        CopyChildren(targetObject.transform, destinationParent);
    }

    private void CopyChildren(Transform source, Transform parent)
    {
        foreach (Transform child in source)
        {
            // ������� ����� ��������� �������
            GameObject copiedChild = Instantiate(child.gameObject, parent);
            copiedChild.transform.localPosition = Vector3.zero; // ���������� ��������� �������
            copiedChild.transform.localRotation = Quaternion.identity; // ���������� ��������� �������
            copiedChild.transform.localScale = Vector3.one; // ���������� ��������� �������

            // ���������� �������� �������� ������� ����� �������
            CopyChildren(child, copiedChild.transform);

            // ��������� ��������� VR IK
            VRIK vrIK = copiedChild.GetComponent<VRIK>();
            if (vrIK != null)
            {
                vrIK.enabled = false;
            }


            // ���� ������ "Hands" � �������� ���, ���� ������
            Transform handsObject = copiedChild.transform.Find("Body/Base/Hands");
            if (handsObject != null)
            {
                //handsObject.gameObject.SetActive(false);
                handsObject.gameObject.SetActive(true);
            }
            
            if (copiedChild.TryGetComponent(out Skin skin))
            {
                skin.SetVisibilityFace(true);
                skin.SetVisibilityVR(true);
            }


            // ��������� ������������� ������
            copiedChild.gameObject.SetActive(false);

        }
    }




}
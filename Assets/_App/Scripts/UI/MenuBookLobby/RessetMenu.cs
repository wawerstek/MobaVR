using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessetMenu : MonoBehaviour
{
    public GameObject objectToActivate; // ������, ������� �� ������ ������������ ����� ���������

    void Start()
    {
        // ��� ������ ������� ������ ��������
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(45); // ���� 5 ������
        objectToActivate.SetActive(true);   // ���������� ������
    }
}

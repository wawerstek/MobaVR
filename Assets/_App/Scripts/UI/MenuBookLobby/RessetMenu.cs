using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessetMenu : MonoBehaviour
{
    public GameObject objectToActivate; // ������, ������� �� ������ ������������ ����� ���������

    void Start()
    {
        // ��� ������ ������� ������ ��������
        
    }
    
    void OnEnable()
    {
        // ��� ������ ������� ������ ��������
        StartCoroutine(ActivateAfterDelay());
    }
    

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(5); // ���� 5 ������
        objectToActivate.SetActive(true);   // ���������� ������
    }
}

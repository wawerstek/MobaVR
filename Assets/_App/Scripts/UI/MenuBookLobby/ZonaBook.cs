using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using BNG;

public class ZonaBook : MonoBehaviour
{
    public GameObject objectToActivate; //��� ���� �����
    public GameObject Book_Menu; //��� ������� ��� �������� ����. ������ ������ ����
    public GameObject Book; //��� ������� ��� �������� �����������, ������� ����� ������ ������
    public Transform destinationParent; //�����, ���� ������ ����������� �����
    private int playerCount = 0; //���������� �������, ����������� � ����
    //public MeshRenderer meshRenderer; //���������� ��� ����
    public GameObject meshRenderer; //���������� ��� ����
    public string targetID; //ID �����. ���� ����� ��� ��������� � �� ���� ��������
    public GameObject[] childObjects;

    //����
    public AudioClip soundOnEnter;
    public AudioClip soundOffSoundMaps;
    public AudioClip soundOnExit;
    public AudioClip soundOnSoundMaps;

    private AudioSource audioSource;

    //�����
    public float fogDensityTarget = 0.5f; // ������� �������� fog density
    public float transitionDuration = 1f; // ������������ �������� � ��������

    private bool isInTrigger = false; // ����, ����������� �� ���������� ������ ��������
    private float initialFogDensity; // ��������� �������� fog density
    private float transitionTimer = 0f; // ������ ��� ��������




    private void Start()
    {
        // �������� ��������� AudioSource �� �������� ������� ��� ��� �������� ��������
        audioSource = GetComponent<AudioSource>();
        // ��������� ��������� �������� fog density
        initialFogDensity = RenderSettings.fogDensity;

    }



    private void Update()
    {
        if (isInTrigger)
        {
            // ���� � ��������, �������� ������ ������ fog density � �������� ��������
            if (transitionTimer < transitionDuration)
            {
                transitionTimer += Time.deltaTime;
                float t = transitionTimer / transitionDuration;
                float newDensity = Mathf.Lerp(initialFogDensity, fogDensityTarget, t);
                RenderSettings.fogDensity = newDensity;
            }
            else
            {
                // ��������� �������
                RenderSettings.fogDensity = fogDensityTarget;
            }
        }
        else
        {
            // ���� �� � ��������, ������ ������ fog density � ���������� ��������
            if (transitionTimer > 0f)
            {
                transitionTimer -= Time.deltaTime;
                float t = transitionTimer / transitionDuration;
                float newDensity = Mathf.Lerp(initialFogDensity, fogDensityTarget, t);
                RenderSettings.fogDensity = newDensity;
            }
            else
            {
                // ��������� �������
                RenderSettings.fogDensity = initialFogDensity;
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerLocalPlayer"))
        {
            playerCount++;
            if (playerCount == 1)
            {

                // ������������� ���� ��� ����� � ���������
                if (soundOnEnter != null)
                {
                    audioSource.PlayOneShot(soundOnEnter);
                    audioSource.PlayOneShot(soundOffSoundMaps);
                }

                //��������� �����
                isInTrigger = true;

                meshRenderer.SetActive(false);
                objectToActivate.SetActive(true);
                Book_Menu.SetActive(true);
                Book.SetActive(false);


               
                int childCount = other.transform.childCount;
                childObjects = new GameObject[childCount];

                // ������ ��� �������� ���������� �������� ��������
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = other.transform.GetChild(i);
                    childObjects[i] = child.gameObject;
                }

                // ������ ��� ����������� �� � destinationParent
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = childObjects[i].transform;


                    child.SetParent(destinationParent);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                    child.localScale = Vector3.one;
                }

                UpdateTargetID(targetID);


            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerLocalPlayer"))
        {
            playerCount--;
            if (playerCount == 0)
            {
                // ������������� ���� ��� ������ �� ����������
                if (soundOnExit != null)
                {
                    audioSource.PlayOneShot(soundOnExit);
                    audioSource.PlayOneShot(soundOnSoundMaps);
                }

                //��������� �����
                isInTrigger = false;


                meshRenderer.SetActive(true);
                objectToActivate.SetActive(false);
                Book_Menu.SetActive(false);
                Book.SetActive(true);

                int childCount = destinationParent.transform.childCount;

                // ������� ��������� ������ ��� �������� ������ �� �������� �������
                GameObject[] childObjects = new GameObject[childCount];

                // ���������� �� �������� �������� � ��������� ������
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = destinationParent.transform.GetChild(i);
                    childObjects[i] = child.gameObject;
                }

                // ���������� �������� ������� ������� � other
                foreach (GameObject childObject in childObjects)
                {
                    Transform childTransform = childObject.transform;
                    childTransform.SetParent(other.transform);
                    childTransform.localPosition = Vector3.zero;
                    childTransform.localRotation = Quaternion.identity;
                    childTransform.localScale = Vector3.one;
                    childObject.SetActive(false);
                }



                //// ��������� �������� ������� �� destinationParent ������� � other
                //foreach (Transform child in destinationParent)
                //{
                //    child.SetParent(other.transform);
                //    child.localPosition = Vector3.zero;
                //    child.localRotation = Quaternion.identity;
                //    child.localScale = Vector3.one;
                //    child.gameObject.SetActive(false);
                //}

            }
        }  
    }

    public void UpdateTargetID(string newTargetID)
    {
        targetID = newTargetID;

        // ������ �� �������� �������� � ��������� �������� � ������ ID
        foreach (Transform child in destinationParent)
        {
            Skin skin = child.GetComponent<Skin>();
            skin.gameObject.SetActive(false);

            if (skin != null && skin.ID == targetID)
            {
                skin.gameObject.SetActive(true);
            }
        }
    }


}

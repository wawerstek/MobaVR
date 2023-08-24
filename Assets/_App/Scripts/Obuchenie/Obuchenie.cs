using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;


public class Obuchenie : MonoBehaviour
{

    public ControllerBinding Button_Obuch = ControllerBinding.BButton; //�������
    public bool ObuchenieRun;
    public bool Test = false;
    public GameObject Personag;
    public GameObject Point;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        ObuchenieRun = false;
        Personag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ObuchenieRun == false)
        {

            //���� ������ �� ������
            if (InputBridge.Instance.GetControllerBindingValue(Button_Obuch) || Test)
            {

               RunTutorial();
            }
        }

    }
    
    [ContextMenu("RunTutorial")]
    public void RunTutorial(){
              //��� �� ������������� ����
    
                    //������� ��������
                   
    
                    // ������� ������ � ������ "CenterEyeAnchor" � ����������� ��� ���������� Player
                    Player = GameObject.Find("CenterEyeAnchor");
    
                    // ���� ������ ������, ������ Point �������� �������� ��� Player
                    if (Player != null)
                    {
                        RessetPlayerPoint();
                    }
                    
                    Personag.SetActive(true);
                    ObuchenieRun = true;
    }


    //���������� ����� �� ������� ����� �������� �� ������
    public void RessetPlayerPoint()
    {
        Point.transform.SetParent(Player.transform);

        // ���� �� ����� ������, ����� ��������� ���������� Point ���� (0,0,0) ������������ Player
        Point.transform.localPosition = Vector3.zero;
    }

}

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
    private float inputStartTime;
    private bool waitingForInput = false;

    // Start is called before the first frame update
    void Start()
    {
        ObuchenieRun = false;
        Personag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
            if (waitingForInput)
            {
                if (Time.time - inputStartTime >= 4.0f)
                {
                    RunTutorial();
                    waitingForInput = false;
                }
            }
            else if (InputBridge.Instance.GetControllerBindingValue(Button_Obuch) || Test)
            {
                waitingForInput = true;
                inputStartTime = Time.time;
            }
            else
            {
                inputStartTime = 0f;
            }
     
    }


    [ContextMenu("RunTutorial")]
    public void RunTutorial(){
        
                    ObuchenieRun = false;
                    Personag.SetActive(false);
    
                    Player = GameObject.Find("CenterEyeAnchor");
                    
                    if (Player != null)
                    {
                        RessetPlayerPoint();
                    }
                    
                    Personag.SetActive(true);
                        //     ObuchenieRun = true;
    }


    //точка перемещается на плеера
    public void RessetPlayerPoint()
    {
        Point.transform.SetParent(Player.transform);

        
        Point.transform.localPosition = Vector3.zero;
    }

}

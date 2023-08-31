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
    public SliderMenu Book01; 
    public SliderMenu Book02; 

    // Start is called before the first frame update
    void Start()
    {
        ObuchenieRun = false;
        Personag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
            if (waitingForInput || Test)
            {
                if (Time.time - inputStartTime >= 4.0f || Test)
                {
                    RunTutorial();
                    waitingForInput = false;
                    Test = false;
                }
            }
            else if (InputBridge.Instance.GetControllerBindingValue(Button_Obuch))
            {
                waitingForInput = true;
                inputStartTime = Time.time;
            }
            else
            {
                waitingForInput = false;
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
                    Personag.transform.localPosition = Vector3.zero;
                    Personag.SetActive(true);
                    CallResetMenu();
                    //     ObuchenieRun = true;
    }


    //точка перемещается на плеера
    public void RessetPlayerPoint()
    {
        Point.transform.SetParent(Player.transform);
        Point.transform.localPosition = Vector3.zero;
    }
    
    //обнуляем книгу
    public void CallResetMenu()
    {
        if (Book01 != null && Book02 != null)
        {
            Book01.RessetMenu();
            Book02.RessetMenu();
        }
    }
    

}

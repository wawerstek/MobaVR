using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.TextCore.Text;


public class Obuchenie : MonoBehaviour
{

    public ControllerBinding Button_Obuch = ControllerBinding.BButton; //кнопка B
    public ControllerBinding Button_Obuch_A = ControllerBinding.AButton; //кнопка A
    public bool ObuchenieRun;
    public bool Test = false;
    public GameObject Personag;
    public GameObject Point;
    public GameObject Player;
    private float requiredHoldTime = 4f; // Время удержания в секундах
    private float buttonHoldTimer = 0f;//для таймера
    
    private bool isButtonCalibrPressed = false;
    private bool isButtonCalibrBPressed = false;
    
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
        if (InputBridge.Instance.GetControllerBindingValue(Button_Obuch) &&
            InputBridge.Instance.GetControllerBindingValue(Button_Obuch_A))
        {
            isButtonCalibrPressed = true;
        }
        else
        {
            isButtonCalibrPressed = false;
            ObuchenieRun = false;
        }


        //сли зажато 2 клавиши
        if (isButtonCalibrPressed && ObuchenieRun == false)
        {
            buttonHoldTimer += Time.deltaTime;
                    
            // Проверка, достигнуто ли требуемое время удержания
            if (buttonHoldTimer >= requiredHoldTime)
            {
                RunTutorial();
                ObuchenieRun = true;
            }
            
        }
        else
        {
            buttonHoldTimer = 0f; // Обнуляем таймер
        }
        
        
            if (Test)
            {
                RunTutorial();
                Test = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using UnityEngine.UI;
using Photon.Pun;

//админка у правление с компьютера
public class Adminka : MonoBehaviourPunCallbacks
{
    
    
    [Header("Меню выбора сетевого режима")]
    public GameObject PanelLocad; // выбора сетевого режима
    [Header("Меню загрузки")]
    public GameObject PanelLocading; //меню загрузки
    
    [Header("Меню основа")]
    public GameObject PanelOsnova; //меню основа, где старт игры и стоп игры
    
    [Header("Меню игры")]
    public GameObject PanelGame; //меню игры
    
    [Header("Меню камер")]
    public GameObject PanelCamers; //меню камер

    private bool RunGames;//проверяем заработала ли игра или нет
    
    
    
    [Header("IP-адрес")]
    public InputField ipAddressInput; // ссылка на поле ввода IP-адреса
    
    [SerializeField] private ClassicGameSession m_GameSession;
    [SerializeField] private SessionSettings m_SessionSettings;
    [SerializeField] private StateMachineSwitcher m_StateMachineSwitcher;
    [SerializeField] private Environment m_Environment;
    [SerializeField] private ScenesEnvironment m_SceneEnvironment;

    private void Awake()
    {
        //админку при старте игры не удаляем
        DontDestroyOnLoad(gameObject);

        RunGames = false;
         LoadSavedIPAddress(); // Загрузить сохраненный IP-адрес при запуске приложения
        //нужен для загрузки сцен
        if (m_GameSession == null)
        {
            m_GameSession = FindObjectOfType<ClassicGameSession>();
        }

        if (m_Environment == null)
        {
            m_Environment = FindObjectOfType<Environment>();
        }

        if (m_SceneEnvironment == null)
        {
            m_SceneEnvironment = FindObjectOfType<ScenesEnvironment>();
        }

        if (m_StateMachineSwitcher == null)
        {
            m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
        }

        if (m_SessionSettings == null)
        {
            m_SessionSettings = FindObjectOfType<SessionSettings>();
        }
    }
    
    
    public void Start()
    {
        //включаем только мень выбора языка и сетевого режима игры
        PanelLocad.SetActive(true);
        PanelLocading.SetActive(false);
        PanelOsnova.SetActive(false);
        PanelGame.SetActive(false);
        PanelCamers.SetActive(false);
    }  

    void Update()
    {
        //если я не мастер клиент, то назначай меня мастером
        if (!PhotonNetwork.IsMasterClient)
        {
            SetMasterClient();
        }
        
        if (m_GameSession == null)
        {
            m_GameSession = FindObjectOfType<ClassicGameSession>();
        }
        else
        {
            if (RunGames == false)
            {
                loadGame();
                RunGames = true;
            }

        }

        if (m_Environment == null)
        {
            m_Environment = FindObjectOfType<Environment>();
        }

        if (m_SceneEnvironment == null)
        {
            m_SceneEnvironment = FindObjectOfType<ScenesEnvironment>();
        }

        if (m_StateMachineSwitcher == null)
        {
            m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
        }

        if (m_SessionSettings == null)
        {
            m_SessionSettings = FindObjectOfType<SessionSettings>();
        }
        
        
        
  
        
    }
    
    
    
    
    public void SetMasterClient()
    {
        if (m_GameSession != null)
        {
            m_GameSession.SetMaster();
        }
    }
    
    
    //выключаем меню загрузки
    public void off_loadMenu()
    {
        SaveIPAddress(); // Сохранить IP-адрес перед загрузкой сцены
        PanelLocad.SetActive(false);
        PanelLocading.SetActive(true);
    }    
    
    
    // Сохранить IP-адрес в PlayerPrefs
    private void SaveIPAddress() 
    {
        string ipAddress = ipAddressInput.text;
        PlayerPrefs.SetString("LastIPAddress", ipAddress);
        PlayerPrefs.Save();
    }
    
    // Загрузить IP-адрес из PlayerPrefs
    private void LoadSavedIPAddress() 
    {
        string lastIPAddress = PlayerPrefs.GetString("LastIPAddress", "");
        if (!string.IsNullOrEmpty(lastIPAddress)) {
            ipAddressInput.text = lastIPAddress;
        }
    }
    
    public void loadCamers()
    {
        
        PanelLocad.SetActive(false);
        PanelLocading.SetActive(false);
        PanelOsnova.SetActive(true);
        PanelGame.SetActive(false);
        PanelCamers.SetActive(true);
    }    
    
    public void loadGame()
    {
       
        PanelLocad.SetActive(false);
        PanelLocading.SetActive(false);
        PanelOsnova.SetActive(true);
        PanelGame.SetActive(true);
        PanelCamers.SetActive(false);
    }    
    
    
    
    
    #region Maps
    //включение карт
    public void ShowTavernMap()
    {
        m_SceneEnvironment.ShowTavernMap();
    }

    /*public void ShowSkyLandMap()
    {
        m_SceneEnvironment.ShowSkyLandMap();
    }*/

    public void ShowSkyLandWithPropMap()
    {
        m_SceneEnvironment.ShowSkyLandWithPropMap();
    }

    /*public void ShowMobaMap()
    {
        m_SceneEnvironment.ShowMobaMap();
    }*/

    /*public void ShowLichMap()
    {
        m_SceneEnvironment.ShowLichMap();
    }*/

    public void ShowNecropolisMap()
    {
        m_SceneEnvironment.ShowNecropolisMap();
    }

    public void ShowTowerMap()
    {
        m_SceneEnvironment.ShowTowerMap();
    }
    
    public void ShowDungeonMap()
    {
        m_SceneEnvironment.ShowDungeonMap();
    }

    public void ShowDefaultPvPMap()
    {
        m_SceneEnvironment.ShowSkyLandMap();
    }

    public void ShowDefaultPvEMap()
    {
        m_SceneEnvironment.ShowLichMap();
    }
    
    
    
    #endregion
    
    
    #region Game Mode

        public void StartMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartMode();
            }
        }

        public void CompleteMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompleteMode();
            }
        }

        public void DeactivateMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.DeactivateMode();
            }
        }

    #endregion

        #region PvP Mode

        public void StartPvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartPvPMode();
            }
        }

        public void CompletePvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompletePvPMode();
            }
        }

        public void DeactivatePvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.DeactivatePvPMode();
            }
        }

        public void SetClassMode()
        {
            if (m_StateMachineSwitcher == null)
            {
                m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
            }

            if (m_StateMachineSwitcher != null)
            {
                m_StateMachineSwitcher.SetClassMode();
            }
        }

        public void SetTimerMode()
        {
            if (m_StateMachineSwitcher == null)
            {
                m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
            }

            if (m_StateMachineSwitcher != null)
            {
                m_StateMachineSwitcher.SetTimerMode();
            }
        }

        #endregion

        /*#region PvE Mode

        public void StartPvEMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartPvEMode();
            }
        }

        public void CompletePvEMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompletePvEMode();
            }
        }
        

        #endregion*/

        
        
        // Функция, которая будет вызываться для выхода из игры
        public void QuitGame()
        {
            #if UNITY_EDITOR
            // Если игра запущена в редакторе Unity, просто остановите редактор
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // Если игра запущена вне редактора, вызовите функцию завершения
            Application.Quit();
            #endif
        }

    
    
}
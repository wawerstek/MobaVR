using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using BNG;

public class PersonagObuch : MonoBehaviour
{
    public Transform player; 
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    public Obuchenie ObuchenieScript; //skript kotoryy vernyot tochku trekkinga na igroka
    public PlayerSpawner spawnerReference;  //Skript na kotorom visit nash personazh
    public string team; //tip komandy


    public AudioClip footstepsSound; // Zvuk shagov
    private AudioSource footstepsSource; //Komponent AudioSource dlya zvuka shagov

    private AudioSource audioSource;
    [SerializeField] private bool isTalking = false; // peremennaya, chtoby zvuk i animatsiya vosproizvodilis' odin raz

    private bool ogidanie = false; // peremennaya, ustanavlivayet ozhidayet li personazh igroka ili bezhit k nemu

    //------------pervyy razgovor--------
    public AudioClip Sound01; // zvuk razgovora
    public Transform targetPointRed; //tochka komandy krasnykh
    public Transform targetPointBlue; //tochka komandy sinikh
    private bool urok01 = false;
    private bool urok02 = false;

    //------------vtoroy razgovor--------
    public AudioClip Sound02; //zvuk razgovora
    private bool urok03 = false;


    //------------tretiy razgovor pokhod v tir--------
    public Transform targetPointRedTir; //tochka komandy krasnykh
    public Transform targetPointBlueTir; //tochka komandy sinikh
    private bool urok04 = false;
    public SaveInfoClass _SaveInfoClass;//tut poluchaem klass igroka
    public string _targetID;
    public AudioClip SoundAtakaDefender; // zvuk zashchitnika
    public AudioClip SoundAtakaRanger; // zvuk luchnika
    public AudioClip SoundAtakaWizard; // zvuk maga



    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        //poluchaem komandu.
        if (spawnerReference.localPlayer != null)
        {
            PlayerVR playerScript = spawnerReference.localPlayer.GetComponent<PlayerVR>();
            if (playerScript != null)
            {
                //team = playerScript.m_Teammate;



                if (playerScript.TeamType == TeamType.RED)
                {
                    team = "RED";
                }

                if (playerScript.TeamType == TeamType.BLUE)
                {
                    team = "BLUE";
                }


            }
        }


        // Sozdaem i nastroivaem komponenty zvuk shagov
        footstepsSource = gameObject.AddComponent<AudioSource>();
        footstepsSource.clip = footstepsSound;
        footstepsSource.loop = true; 


    }

    void Update()
    {
        RotationPers();



        if (ogidanie == false)
        {
            agent.SetDestination(player.position);//govorim personazhu kuda topat

            //Proverka, dvigaetsya li personazh, i ustanovka animatsii sootvetstvenno
            if (agent.velocity.sqrMagnitude > 0f) // personazh dvigaetsya
            {
                animator.SetBool("isWalking", true);


                //Proveryaem, ne proigryvaetsya li zvuk shagov uzhe
                if (!footstepsSource.isPlaying)
                {
                    // Proigryvaem zvuk shagov
                    footstepsSource.Play();
                }
                


            }
            else //personazh stoit na meste
            {
                //Debug.Log("Персонаж стоит на месте");
                //Debug.Log("urok01 = " + urok01);
                //Debug.Log("urok02 = " + urok02);
                //Debug.Log("urok04 = " + urok04);

                animator.SetBool("isWalking", false);
                // Ostanavlivayem zvuk shagov
                footstepsSource.Stop();

                if (urok01 == false)
                {
                    CheckForConversation();
                }
                else if (urok02 == false)
                {
                    Urok2();
                }
                else if (urok04 == false)
                {
                    //Debug.Log("Включаем функцию UrokTir");
                    UrokTir();
                }

            }
        }



        if (ogidanie == true)
        {

                if (urok03 == false)
                {
                    Book01();
                }

        }



    }


    void RotationPers()
    {
        //поворот персонажа в сторону точки
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // чтобы смотрел только по горизонтали
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;
    }


    #region RunObuchenie
    void CheckForConversation()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 2.0f && !isTalking) // если персонаж близко к игроку и еще не говорит
        {

            animator.SetInteger("Obushenie", 1);
            // Воспроизведение анимации разговора

            // Воспроизведение звука
            audioSource.clip = Sound01;
            audioSource.Play();

            isTalking = true; // чтобы не воспроизводить звук и анимацию многократно

            //после того как звук проигрался перемещаем точку, к которой идёт персонаж в книгу
            StartCoroutine(MovePlayerAfterSound01());
           
        }
    } 
    
    void Urok2()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 2.0f) // если персонаж близко к точке
        {

            ogidanie = true; //говорим персонажу ждать игрока на месте
            ObuchenieScript.RessetPlayerPoint();//возвращаем точку трекинга на игрока
            isTalking = true;
            urok02 = true;
        }
    }

    IEnumerator MovePlayerAfterSound01()
    {
        yield return new WaitForSeconds(audioSource.clip.length); // Ждем окончания звука

        //if (team == RED)
        //{
        //    //перемещаем точку
        //    player.SetParent(targetPoint01);
        //    player.localPosition = Vector3.zero;
        //}

        if (team ==  "RED")
        {
            //перемещаем точку к красной базе
             player.SetParent(targetPointRed);
             player.localPosition = Vector3.zero;
        }
        else if (team == "BLUE")
        {
            //перемещаем точку к красной базе
            player.SetParent(targetPointBlue);
            player.localPosition = Vector3.zero;
        }


        urok01 = true; //первый урок прошёл

        //завершаем анимацию разговора
        animator.SetInteger("Obushenie", 0);
        
    }
    #endregion

    #region 01_Obuchenie_Book
        void Book01()
        {
        float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 2.0f && !isTalking) // если персонаж близко к игроку и еще не говорит
            {

                animator.SetInteger("Obushenie", 1);
                // Воспроизведение анимации разговора

                // Воспроизведение звука
                audioSource.clip = Sound02;
                audioSource.Play();

                isTalking = true; // чтобы не воспроизводить звук и анимацию многократно
                                  //после того как звук проигрался перемещаем точку, к которой идёт персонаж в книгу
                StartCoroutine(MovePlayerAfterSound02());
            }

        }

    IEnumerator MovePlayerAfterSound02()
    {
        yield return new WaitForSeconds(audioSource.clip.length); // Ждем окончания звука

        urok03 = true;

        //завершаем анимацию разговора
        animator.SetInteger("Obushenie", 0);

    }


    #endregion


    #region 02_Obuchenie_Tir

    public void Urok3Tir()
    {

        ogidanie = false;

        if (team == "RED")
        {
            //перемещаем точку к красной базе
            player.SetParent(targetPointRedTir);
            player.localPosition = Vector3.zero;
        }
        else if (team == "BLUE")
        {
            //перемещаем точку к красной базе
            player.SetParent(targetPointBlueTir);
            player.localPosition = Vector3.zero;
        }

        //Debug.Log("Точка выбрана куда топать персонажу");


    }


    void UrokTir()
    {
      float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 2.0f && !isTalking) // если персонаж близко к точке
        {


            //ogidanie = true; //говорим персонажу ждать игрока на месте
            ObuchenieScript.RessetPlayerPoint();//возвращаем точку трекинга на игрока


            _targetID = _SaveInfoClass.targetID;

            if (_targetID == "Defender")
            {
                // Воспроизведение звука
                audioSource.clip = SoundAtakaDefender;
                audioSource.Play();
            }
            else if (_targetID == "Ranger")
            { 
              // Воспроизведение звука
                audioSource.clip = SoundAtakaRanger;
                audioSource.Play();
            }
            else if (_targetID == "Wizard")
            { 
                // Воспроизведение звука
                audioSource.clip = SoundAtakaWizard;
                audioSource.Play();
            }

            isTalking = false;
            urok04 = true;
        }
    }



    #endregion







    //стопорим звук
    public void StopAllSounds()
    {
        audioSource.Stop();

    }

}

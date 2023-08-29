using UnityEngine;
using System.Collections;

public class CharacterActions : MonoBehaviour
{
    // Ссылка на Transform игрока.
    public Transform playerTransform;

    // Время ожидания перед следующим действием.
    public float waitingTime = 3.0f;

    // Максимальное расстояние до игрока, на котором будет срабатывать действие.
    public float maxDistanceToPlayer = 2.0f;

    // Приватные переменные для управления персонажем, звуком и анимацией.
    private CharacterMover mover; //шаги
    private EndTutorialHandler endTutorialHandler; //конец обучения
    public AudioSource audioSource;
    private Animator animator;
    private float waitingTimeStart = 1.0f;
    private bool playerApproached = false;//проверяем дошёл ли игрок до персонажа
    
    // Массив шагов обучения.
    public TutorialStep[] tutorialSteps;

    // Индекс текущего шага обучения.
    private int currentStepIndex = 0;
    
    public int CurrentStepIndex 
    {
        get { return currentStepIndex; }
    }
    

    // Индекс последнего воспроизведенного звука.
    private int lastPlayedSoundIndex = -1;
    
    private IEnumerator _rotateCoroutine;
    private bool continueTutorial = false;

    // Класс, описывающий каждый шаг обучения.
    [System.Serializable]
    public class TutorialStep
    {
        public int NomerUroka;
        public string description; // Описание шага.
        public AudioClip startSound; // Звук при старте шага.
        public string startAnimation; // Анимация при старте шага.
        public bool startSoundRun; //звук проигрался
        public Transform targetPoint; // Точка, к которой должен двигаться персонаж.
        public bool targetPointRun; //Персонаж подошёл к точке
        public AudioClip[] waitingForPlayerSounds; // Звуки ожидания игрока.
        public string[] waitingForPlayerAnimations; // Анимации ожидания игрока.
        public bool layerGoPersRun; //Игрок подошёл к персонажу
        public AudioClip mainTaskSound; // Главный звук задания.
        public string mainTaskAnimation; // Главная анимация задания.
        public bool mainTaskSoundRun; //Основной звук проигрался
        public bool stopUrok; // Переменная ожидания выполнения скрипта
        public AudioClip[] waitingForTaskCompletionSounds; // Звуки ожидания завершения задания.
        public string[] waitingForTaskCompletionAnimations; // Анимации ожидания завершения задания.
        public bool isTaskCompleted = false; // Флаг завершения задания.
    }

    // Метод вызывается при инициализации объекта.
    private void Awake()
    {
        // Получаем компоненты объекта.
        mover = GetComponent<CharacterMover>();
        endTutorialHandler  = GetComponent<EndTutorialHandler>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Метод вызывается при активации объекта.
    void OnEnable()
    {
        //сохраняем данные
        SaveTutorialStepsData();
        // Запускаем обучение.
        StartTutorial();
    }
   
    void OnDisable()
    {
        currentStepIndex = 0;
        LoadTutorialStepsData();
       
    }
    

    // Метод запуска обучения.
    public void StartTutorial()
    {
        // Если есть шаги обучения, запускаем первый шаг.
        if (tutorialSteps.Length > 0)
        {
            StartCoroutine(ExecuteStep(tutorialSteps[0]));
        }
    }

    // Корутина выполнения шага обучения.
    public IEnumerator ExecuteStep(TutorialStep step)
    {
        // Воспроизводим звук и анимацию начала шага.
        PlaySoundAndAnimation(step.startSound, step.startAnimation);

        // Если есть звук, ждем его завершения.
        if (step.startSound)
        {
            yield return new WaitForSeconds(step.startSound.length);
            step.startSoundRun = true; //флаг, что звук проигран, можно использовать в других скриптах
        }

        // Если есть целевая точка, двигаемся к ней.
        if (step.targetPoint)
        {
            mover.MoveToPoint(step.targetPoint.position);
        
           //Debug.Log("Останавливаем корутину которая управляет поворотами перса");
            if(_rotateCoroutine != null) 
            {
                StopCoroutine(_rotateCoroutine);
                _rotateCoroutine = null; // Не забывайте обнулить ссылку
            }
            
            // Ждем, пока персонаж не достигнет своего пункта назначения
            yield return new WaitWhile(() => !mover.HasReachedDestination());
            step.targetPointRun = true;//персонаж дошёл до точки
            
           // Debug.Log("Запускаем корутину которая управляет поворотами перса");
            _rotateCoroutine = RotateTowardsPlayerRoutine();
            StartCoroutine(_rotateCoroutine);
        }

        // Запускаем корутину ожидания игрока.
        yield return StartCoroutine(WaitForPlayerRoutine(step));
       
    }

   // Корутина ожидания игрока.
    private IEnumerator WaitForPlayerRoutine(TutorialStep step)
    {
        
        
        bool hasPlayedMainTaskSound = false;

        // Цикл продолжается, пока задание не выполнено.
        while (!step.isTaskCompleted)
        {
            
            // Если идет воспроизведение звука, ждем его завершения.
            if (audioSource.isPlaying)
            {
                yield return new WaitWhile(() => audioSource.isPlaying);
          
            }

            
            //говорим, что первый урок пройден
            if (currentStepIndex == 0)
            {
                step.isTaskCompleted = true;
            }
            
            // Ждем установленное время. 1 сек
            yield return new WaitForSeconds(waitingTimeStart);
            

            

            // Если игрок находится в пределах допустимого расстояния и главный звук задания еще не воспроизводился.
            if (Vector3.Distance(transform.position, playerTransform.position) <= maxDistanceToPlayer && !hasPlayedMainTaskSound && !audioSource.isPlaying)
            {
                step.layerGoPersRun = true;//игрок дошёл до персонажа
                playerApproached = true; // Игрок подошёл
                
                // Воспроизводим главный звук и анимацию задания.
                PlaySoundAndAnimation(step.mainTaskSound, step.mainTaskAnimation);
                
                /*// Если есть звук, ждем его завершения.
                if (step.mainTaskSound)
                {
                    yield return new WaitForSeconds(step.mainTaskSound.length);
                    step.mainTaskSoundRun = true;//говорим, что звук основной проигрался
                }*/
                
                if (step.mainTaskSound)
                {
                    yield return new WaitForSeconds(step.mainTaskSound.length);
                    step.mainTaskSoundRun = true;
    
                    if (step.stopUrok) // Проверяем, нужно ли остановить урок
                    {
                        yield return new WaitUntil(() => continueTutorial);
                    }
    
                    hasPlayedMainTaskSound = true;
                }

                
               
                hasPlayedMainTaskSound = true;
            }
            
            // Если игрок далеко и есть звуки ожидания игрока.
            else if(Vector3.Distance(transform.position, playerTransform.position) > maxDistanceToPlayer && step.waitingForPlayerSounds.Length > 0 && !hasPlayedMainTaskSound)
            {
                
                // Ждем установленное время.
                yield return new WaitForSeconds(waitingTime);
                // Если есть условие для прерывания ожидания, выполнить прерывание.
                if (playerApproached)
                {
                    playerApproached = false; // Игрок далеко
                    yield return null; // Сразу завершить текущую итерацию корутины
                    continue; // Пропустить остальной код и начать следующую итерацию цикла
                }
                
                
                int randomSoundIndex = GetRandomSoundIndex(step.waitingForPlayerSounds.Length);
                int randomAnimationIndex = GetRandomAnimationIndex(step.waitingForPlayerAnimations.Length);

                     PlaySoundAndAnimation(step.waitingForPlayerSounds[randomSoundIndex], step.waitingForPlayerAnimations[randomAnimationIndex]);
            }


                // Если есть звуки ожидания завершения задания, воспроизводим их.
                if (step.waitingForTaskCompletionSounds.Length > 0 && hasPlayedMainTaskSound && !step.isTaskCompleted)
                {
                    // Ждем установленное время.
                    yield return new WaitForSeconds(waitingTime);

                    int randomSoundIndex = GetRandomSoundIndex(step.waitingForTaskCompletionSounds.Length);

                    // Проверьте, что массив не пустой и индекс находится в пределах допустимого диапазона.
                    if (step.waitingForTaskCompletionSounds.Length > 0 && randomSoundIndex < step.waitingForTaskCompletionSounds.Length &&  !audioSource.isPlaying) 
                    {
                        audioSource.PlayOneShot(step.waitingForTaskCompletionSounds[randomSoundIndex]);
                        
                        int randomAnimationIndex = GetRandomAnimationIndex(step.waitingForTaskCompletionAnimations.Length);
                        // Точно такая же проверка для массива анимаций.
                        if (step.waitingForTaskCompletionAnimations.Length > 0 && randomAnimationIndex < step.waitingForTaskCompletionAnimations.Length && !string.IsNullOrEmpty(step.waitingForTaskCompletionAnimations[randomAnimationIndex])) 
                        {
                            SetAnimationParameter(step.waitingForTaskCompletionAnimations[randomAnimationIndex], true);
                        }

                    }


                    
                }

                // Если задание выполнено.
                if (step.isTaskCompleted)
                {
                // Переходим к следующему шагу обучения.
                NextStep();
                }
        }
    }

    
    // Получить случайный индекс звука, отличный от последнего воспроизведенного.
    private int GetRandomSoundIndex(int soundsCount)
    {
        int randomSoundIndex;
        do
        {
            randomSoundIndex = Random.Range(0, soundsCount);
        } while (randomSoundIndex == lastPlayedSoundIndex);

        lastPlayedSoundIndex = randomSoundIndex;
        return randomSoundIndex;
    }

    // Получить случайный индекс анимации.
    private int GetRandomAnimationIndex(int animationsCount)
    {
        return Random.Range(0, animationsCount);
    }

    // Метод воспроизводит звук и анимацию рандомную
    private void PlaySoundAndAnimation(AudioClip sound, string animationParameter)
    {
        if (sound && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sound);

            // Воспроизводим анимацию, только если у нас есть соответствующий параметр для этого звука
            if (!string.IsNullOrEmpty(animationParameter))
            {
                SetAnimationParameter(animationParameter, true);
                // Останавливаем анимацию, когда звук закончит играть
                StartCoroutine(StopAnimationWhenSoundStops(sound.length, animationParameter));
            }
        }
    }


    //эта корутина будет вместо PlaySoundAndAnimation
    private IEnumerator PlaySoundAndChangeAnimationParameter(AudioClip sound, string animationParameter)
    {
        if (sound && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sound);
            SetAnimationParameter(animationParameter, true);
            yield return new WaitForSeconds(sound.length);
            SetAnimationParameter(animationParameter, false);
        }
    }
    
    
    // Метод перехода к следующему шагу обучения.
    public void NextStep()
    {
       
        currentStepIndex++;
        // Если следующий шаг существует, запускаем его.
        if (currentStepIndex < tutorialSteps.Length)
        {
            StartCoroutine(ExecuteStep(tutorialSteps[currentStepIndex]));
        }
        else
        {
            endTutorialHandler.EndTutorialSequence();
            // Если обучение завершено, выводим сообщение в консоль.
           // Debug.Log("Tutorial completed!");
        }
    }
  
    //корутина поворота
    private IEnumerator RotateTowardsPlayerRoutine()
    {
        while (true)
        {
           
                
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                directionToPlayer.y = 0; // Убедитесь, что вращение происходит только по горизонтали
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                yield return null; // Это будет выполнять корутину каждый кадр

        }
    }
    
    //стопорим звук
    public void StopAllSounds()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    
    //в этой функции мы безопасно применяем параметру значение true
    private void SetAnimationParameter(string parameter, bool value)
    {
        if (!string.IsNullOrEmpty(parameter))
        {
            animator.SetBool(parameter, value);
        }
    }
    
    private IEnumerator StopAnimationWhenSoundStops(float soundDuration, string animationParameter)
    {
        // Ждем, пока звук закончит воспроизводиться
        yield return new WaitForSeconds(soundDuration);

        // Устанавливаем параметр анимации обратно в false
        SetAnimationParameter(animationParameter, false);
    }
    
    public bool IsMainTaskSoundPlaying(int stepIndex)
    {
        // Проверяем, воспроизводится ли звук mainTaskSound заданного шага обучения.
        if(stepIndex < 0 || stepIndex >= tutorialSteps.Length)
        {
            Debug.LogError("Invalid step index provided!");
            return false;
        }
    
        return audioSource.isPlaying && audioSource.clip == tutorialSteps[stepIndex].mainTaskSound;
    }
    
    //ожидание выполнения скрипта, после звука главного
    public void OnTutorialsCompleted()
    {
        continueTutorial = true;
    }
    
    //методы сохранения изначальных значений переменных
    public void SaveTutorialStepsData()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            PlayerPrefs.SetInt($"TutorialStep_{i}_startSoundRun", tutorialSteps[i].startSoundRun ? 1 : 0);
            PlayerPrefs.SetInt($"TutorialStep_{i}_targetPointRun", tutorialSteps[i].targetPointRun ? 1 : 0);
            PlayerPrefs.SetInt($"TutorialStep_{i}_layerGoPersRun", tutorialSteps[i].layerGoPersRun ? 1 : 0);
            PlayerPrefs.SetInt($"TutorialStep_{i}_mainTaskSoundRun", tutorialSteps[i].mainTaskSoundRun ? 1 : 0);
            PlayerPrefs.SetInt($"TutorialStep_{i}_isTaskCompleted", tutorialSteps[i].isTaskCompleted ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadTutorialStepsData()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].startSoundRun = PlayerPrefs.GetInt($"TutorialStep_{i}_startSoundRun", 0) == 1;
            tutorialSteps[i].targetPointRun = PlayerPrefs.GetInt($"TutorialStep_{i}_targetPointRun", 0) == 1;
            tutorialSteps[i].layerGoPersRun = PlayerPrefs.GetInt($"TutorialStep_{i}_layerGoPersRun", 0) == 1;
            tutorialSteps[i].mainTaskSoundRun = PlayerPrefs.GetInt($"TutorialStep_{i}_mainTaskSoundRun", 0) == 1;
            tutorialSteps[i].isTaskCompleted = PlayerPrefs.GetInt($"TutorialStep_{i}_isTaskCompleted", 0) == 1;
        }
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

namespace BNG {

    /// <summary>
    /// Объект, который может быть поднят захватчиком
    /// </summary>
    public class Grabbable : MonoBehaviour {
        
        /// <summary>
        /// Удерживается ли этот объект в настоящее время захватчиком
        /// </summary>
        public bool BeingHeld = false;

        /// <summary>
        /// Удерживается ли этот объект в настоящее время более чем одним захватчиком
        /// </summary>
        public bool BeingHeldWithTwoHands {
            get {
                if (heldByGrabbers != null && heldByGrabbers.Count > 1 && SecondaryGrabBehavior == OtherGrabBehavior.DualGrab) {
                    return true;
                }
                // Удерживается, и также удерживается определенный вторичный объект захвата
                else if (BeingHeld && SecondaryGrabbable != null && SecondaryGrabbable.BeingHeld == true) {
                    return true;
                }

                return false;
            }
        }

        List<Grabber> validGrabbers;

        /// <summary>
        /// Захватчик, который в настоящее время удерживает нас. Null, если не удерживается
        /// </summary>        
        protected List<Grabber> heldByGrabbers;

        public List<Grabber> HeldByGrabbers {
            get {
                return heldByGrabbers;
            }
        }

        /// <summary>
        /// Сохраните, было ли жесткое тело кинематическим при запуске или нет.
        /// </summary>
        protected bool wasKinematic;
        protected bool usedGravity;
        protected CollisionDetectionMode initialCollisionMode;
        protected RigidbodyInterpolation initialInterpolationMode;

        /// <summary>
        /// Подтягивается ли объект к захвату
        /// </summary>
        public bool RemoteGrabbing {
            get {
                return remoteGrabbing;
            }
        }        

        protected bool remoteGrabbing;

        [Header("Настройки захвата")]
        /// <summary>
        /// Настройте, какая кнопка используется для инициирования захвата
        /// </summary>
        [Tooltip("Это свойство позволяет указать, какую кнопку нужно нажать, чтобы взять объект. Обычно это 'Grab', но иногда вы можете использовать «Триггер» (например, стрелку). или наследовать Inherit")]
        public GrabButton GrabButton = GrabButton.Inherit;

        [Header("Grabtype Hold-удерживаем кнопку. Toggle-нажали на кнопку")]
       
        /// <summary>
        /// 'Inherit' унаследует этот параметр от граббера. 'Hold' требуется, чтобы пользователь удерживал нажатой кнопку захвата. 'Toggle' отбросит / отпустит захват при активации кнопки.
        /// </summary>
        [Tooltip("Inherit унаследует этот параметр от граббера. 'Hold' требуется, чтобы пользователь удерживал нажатой кнопку захвата. 'Toggle' отбросит / отпустит захват при активации кнопки.")]
        public HoldType Grabtype = HoldType.Inherit;

        /// <summary>
        /// Кинематическая физика фиксирует объект на месте на руке / захвате. Физический стык допускает столкновения с окружающей средой.
        /// </summary>
        [Tooltip("Позволяет указать, как этот объект будет удерживаться в Grabbers. (в руке) \n-Physics Joint  Физическое соединение ConfigurableJoint будет соединяться от Grabber к Grabbable.Это позволяет удерживаемым объектам по - прежнему сталкиваться с окружающей средой и не проходить сквозь стены / другие объекты.Жесткость суставов будет изменена в зависимости от того, с чем он сталкивается, чтобы убедиться, что он правильно выравнивается с руками во время взаимодействия и движения. \n-Kinematic кинематический Grabbable будет перемещен в Grabber, а его RigidBody будет установлено на Kinematic.Grabbable не допускает столкновения с другими объектами и может проходить сквозь стены.Предмет останется на месте, и это надежный способ поднять предметы, если вам не нужна физическая поддержка. \n-Velocity Скорость Объект Grabbable будет перемещаться с использованием силы с постоянной скоростью, применяемой в FixedUpdate. \n-None Нет Механизм захвата не применяется.Например, объекты, на которые можно взобраться, не прихватываются пользователем.Они остаются на месте при захвате.В этом случае Rigidbody не нужен.")]
        public GrabPhysics GrabPhysics = GrabPhysics.Velocity;

        /// <summary>
        /// Привязка к местоположению или захват в любом месте объекта
        /// </summary>
        [Tooltip("Привязка к местоположению или захват в любом месте объекта. \n- Precise можно подобрать где угодно.\n- Snap будет привязан к положению Grabber со смещением на любые указанные точки захвата.")]
        public GrabType GrabMechanic = GrabType.Precise;

        /// <summary>
        /// Как быстро прикрепить предмет к руке
        /// </summary>
        [Tooltip("Как быстро прикрепить предмет к руке")]
        public float GrabSpeed = 15f;

        /// <summary>
        /// Может ли объект быть подобран издалека. Должен находиться в пределах триггера удаленного захвата
        /// </summary>
        [Header("Дистанционный захват")]
        [Tooltip("Может ли объект быть подобран издалека. Должен находиться в пределах триггера удаленного захвата")]
        public bool RemoteGrabbable = false;

        public RemoteGrabMovement RemoteGrabMechanic = RemoteGrabMovement.Linear;

        /// <summary>
        /// Объект с максимальным расстоянием может быть удаленно захвачен. Неприменимо, если значение RemoteGrabbable равно false
        /// </summary>
        [Tooltip("Объект с максимальным расстоянием может быть удаленно захвачен. Неприменимо, если значение RemoteGrabbable равно false")]
        public float RemoteGrabDistance = 2f;

        /// <summary>
        /// Умножьте скорость контроллера на это при броске
        /// </summary>
        [Header("Бросание")]
        [Tooltip("Умножьте скорость контроллера на это при броске")]
        public float ThrowForceMultiplier = 2f;

        /// <summary>
        /// Умножьте угловую скорость контроллера на это при броске
        /// </summary>
        [Tooltip("Умножьте угловую скорость контроллера на это при броске")]
        public float ThrowForceMultiplierAngular = 1.5f; // Умножьте угловую скорость на это

        /// <summary>
        /// Отбросьте элемент, если центр объекта находится на таком расстоянии от центра захвата (в метрах). Установите значение 0, чтобы отключить разрыв расстояния.
        /// </summary>
        [Tooltip("Отбросьте элемент, если центр объекта находится на таком расстоянии от центра захвата (в метрах). Установите значение 0, чтобы отключить разрыв расстояния.")]
        public float BreakDistance = 0;

        /// <summary>
        /// Включение этого параметра скроет преобразование, указанное в свойстве графики руки граббера
        /// </summary>
        [Header("Варианты рук")]
        [Tooltip("Включение этого параметра скроет преобразование, указанное в свойстве графики руки граббера")]
        public bool HideHandGraphics = false;

        /// <summary>
        ///  Прикрепите этот предмет к рукам для лучшей устойчивости.
        ///  Не рекомендуется для детских захватов
        /// </summary>
        [Tooltip("Прикрепите этот предмет к рукам для мгновенного перемещения. Объект будет перемещаться в соотношении 1:1 с контроллером, но могут возникнуть проблемы с обнаружением быстро движущихся столкновений.")]
        public bool ParentToHands = false;

        /// <summary>
        /// Если значение true, модель руки будет прикреплена к захваченному объекту
        /// </summary>
        [Tooltip("Если значение true, модель руки будет прикреплена к захваченному объекту. Это отделяет его от соответствия 1: 1 с контроллером, но может выглядеть более реалистично.")]
        public bool ParentHandModel = true;

        [Tooltip("Если значение true, модель руки будет привязана к ближайшей точке захвата. В противном случае модель руки останется с захватом.")]
        public bool SnapHandModel = true;

        /// <summary>
        /// Установите значение false, чтобы отключить удаление. Если false, будет постоянно привязан к тому, что захватывает это. 
        /// </summary>
        [Header("Разное")]
        [Tooltip("Установите значение false, чтобы отключить удаление. Если false, будет постоянно привязан к тому, что захватывает это.\n-Возможно это нужно для того, чтобы объект прикрепился на постоянку, если стоит выключеная галка")]
        public bool CanBeDropped = true;

        /// <summary>
        /// Можно ли привязать этот объект к зонам привязки? Установите значение false, если вы никогда не хотите, чтобы это было быстро. Дальнейшую фильтрацию можно выполнить по зонам привязки
        /// </summary>
        [Tooltip("Можно ли привязать этот объект к зонам привязки? Установите значение false, если вы никогда не хотите, чтобы это было быстро. Дальнейшую фильтрацию можно выполнить по зонам привязки")]
        public bool CanBeSnappedToSnapZone = true;

        [Tooltip("Если значение true, то при удалении объекта кинематика всегда будет отключена, даже если изначально он был кинематическим.")]
        public bool ForceDisableKinematicOnDrop = false;

        [Tooltip("Если значение true, объект мгновенно переместится / повернется к захвату вместо использования скорости / силы. Это произойдет только в том случае, если в последнее время не происходило никаких столкновений. При использовании этого метода Захватываемое твердое тело будет мгновенно повернуто / перемещено с учетом настроек интерполяции. Может проходить сквозь объекты, если движется достаточно быстро.")]
        public bool InstantMovement = false;

        [Tooltip("Если значение true, все дочерние коллайдеры будут считаться доступными для захвата. Если значение равно false, вам нужно будет добавить компонент 'GrabbableChild' ко всем дочерним коллайдерам, которые вы хотите также считать доступными для захвата.")]
        public bool MakeChildCollidersGrabbable = false;

        [Header("Поза Руки По Умолчанию")]
        [Tooltip("Ручной контроллер может считывать это значение, чтобы определить, как анимировать при захвате этого объекта. 'AnimatorID' = укажите идентификатор аниматора, который будет установлен на ручном аниматоре после захвата этого объекта. 'HandPose' = используйте объект с возможностью создания сценария позы руки. 'AutoPoseOnce' = СДЕЛАЙТЕ автоматическую позу один раз после захвата этого объекта. 'AutoPoseContinuous' = Продолжайте бежать, пытаясь принять автоматическую позу, одновременно хватая этот предмет.")]
        public HandPoseType handPoseType = HandPoseType.HandPose;
        protected HandPoseType initialHandPoseType;

        [Tooltip("Если HandPoseType= 'HandPose', этот объект позы руки будет применен к руке при поднятии")]
        public HandPose SelectedHandPose;
        protected HandPose initialHandPose;

        /// <summary>
        ///Идентификатор аниматора используемой позы руки
        /// </summary>
        [Tooltip("Этот идентификатор позы руки будет передан Аниматору руки, когда он будет оснащен. Вы можете добавлять новые позы рук в файл Hand Pose Definitions.cs.")]
        public HandPoseId CustomHandPose = HandPoseId.Default;
        protected HandPoseId initialHandPoseId;

        /// <summary>
        /// Что делать, если другой граббер захватит это, будучи снаряженным. Двойной захват в настоящее время не поддерживается.
        /// </summary>
        [Header("Поведение при захвате Двумя руками")]
        [Tooltip("Что делать, если другой граббер захватит это, будучи снаряженным.")]
        public OtherGrabBehavior SecondaryGrabBehavior = OtherGrabBehavior.None;

        [Tooltip("Как вести себя, когда две руки хватают этот предмет. LookAt = Попросите первичный захват 'Взглянуть' На вторичный захват. Например, удерживание винтовки в правом контроллере приведет к ее вращению в направлении левого контроллера. AveragePositionRotation = Используйте точку и поворот в пространстве, которое находится на полпути между обоими захватами.")]
        public TwoHandedPositionType TwoHandedPosition = TwoHandedPositionType.Lerp;

        [Tooltip("Как далеко нужно перемещаться между позициями захвата. Например, 0,5 = на полпути между первичным и вторичным захватчиками. 0 = использовать положение первичного захватчика, 1 = использовать положение вторичного захвата.")]
        [Range(0.0f, 1f)]
        public float TwoHandedPostionLerpAmount = 0.5f;

        [Tooltip("Как вести себя, когда две руки хватают этот предмет. LookAt = Попросите первичный захват 'Взглянуть' На вторичный захват. Например, удерживание винтовки в правом контроллере приведет к ее вращению в направлении левого контроллера.  AveragePositionRotation = Используйте точку и поворот в пространстве, которое находится на полпути между обоими захватами.")]
        public TwoHandedRotationType TwoHandedRotation = TwoHandedRotationType.Slerp;
        
        [Tooltip("Как далеко до lerp / slerp между поворотом граббера. Например, 0,5 = на полпути между первичным и вторичным захватчиками. 0 = используйте вращение первичного захвата, 1 = используйте положение вторичного захвата.")]
        [Range(0.0f, 1f)]
        public float TwoHandedRotationLerpAmount = 0.5f;

        [Tooltip("Как реагировать, если вы держите предмет двумя руками, а затем отбрасываете основной захват. Например, вы можете захотеть отбросить объект, передать его в подержанную руку или вообще ничего не делать.")]
        public TwoHandedDropMechanic TwoHandedDropBehavior = TwoHandedDropMechanic.Drop;

        [Tooltip("Какой вектор использовать, когда Вращение Двумя Руками = Посмотрите На Вторичный. TwoHandedRotation = LookAtSecondary. Ex : Horizontal = Установка типа винтовки, в которой вы хотите прицелиться в объекты; Vertical = Установка типа ближнего боя, где объект расположен вертикально")]
        public TwoHandedLookDirection TwoHandedLookVector = TwoHandedLookDirection.Horizontal;        

        [Tooltip("Как быстро перейти к Вторичному захвату, если поведение захвата двумя руками = LookAt")]
        public float SecondHandLookSpeed = 40f;

        [Header("Вторичный Захватываемый Объект")]
        [Tooltip("Если указано, этот объект будет использоваться в качестве вторичного объекта захвата вместо того, чтобы полагаться на точки захвата этого объекта. Если 'TwoHandedGrabBehavior' (Поведение захвата двумя руками) указано как Look At, это объект, к которому будет повернут захват. Если 'TwoHandedGrabBehavior' (Поведение Захвата Двумя руками) указывается как Среднее вращение позиции, это объект, который граббер использует для вычисления позиции.")]
        public Grabbable SecondaryGrabbable;

        /// <summary>
        /// Захватываемый объект может быть захвачен только в том случае, если этот захват удерживается.
        /// Пример: Если вы хотите, чтобы часть оружия можно было захватить только в том случае, если удерживается само оружие.
        /// </summary>
        [Header("Ограничения на Захват")]
        [Tooltip("Захватываемый объект может быть захвачен только в том случае, если этот захват удерживается. Пример: Если вы хотите, чтобы часть оружия можно было захватить только в том случае, если удерживается само оружие.")]
        public Grabbable OtherGrabbableMustBeGrabbed = null;

        [Header("Настройки физических Соединений")]
        /// <summary>
        /// Какое усилие пружины приложить к соединению, когда что-то соприкасается с захватываемым предметом
        /// Более высокое усилие пружины сделает захват более жестким
        /// </summary>
        [Tooltip("Более высокое усилие пружины сделает захват более жестким")]
        public float CollisionSpring = 3000;

        /// <summary>
        /// Сколько силы сна нужно приложить к суставу, когда что-то соприкасается с захватываемым предметом
        /// </summary>
        [Tooltip("Сколько силы сна нужно приложить к суставу, когда что-то соприкасается с захватываемым предметом")]
        public float CollisionSlerp = 500;

        [Tooltip("Как ограничить движение настраиваемого сустава при столкновении с объектом. Положение может быть свободным, полностью заблокированным или ограниченным.")]
        public ConfigurableJointMotion CollisionLinearMotionX = ConfigurableJointMotion.Free;

        [Tooltip("Как ограничить движение настраиваемого сустава при столкновении с объектом. Положение может быть свободным, полностью заблокированным или ограниченным.")]
        public ConfigurableJointMotion CollisionLinearMotionY = ConfigurableJointMotion.Free;

        [Tooltip("Как ограничить движение настраиваемого сустава при столкновении с объектом. Положение может быть свободным, полностью заблокированным или ограниченным.")]
        public ConfigurableJointMotion CollisionLinearMotionZ = ConfigurableJointMotion.Free;

        [Tooltip("Ограничьте вращение вокруг осей X, чтобы оно было свободным, полностью заблокированным или ограниченным при столкновении с объектом.")]
        public ConfigurableJointMotion CollisionAngularMotionX = ConfigurableJointMotion.Free;

        [Tooltip("Ограничьте вращение вокруг осей Y, чтобы оно было свободным, полностью заблокированным или ограниченным при столкновении с объектом.")]
        public ConfigurableJointMotion CollisionAngularMotionY = ConfigurableJointMotion.Free;

        [Tooltip("Ограничьте вращение вокруг осей Z, чтобы оно было свободным, полностью заблокированным или ограниченным при столкновении с объектом.")]
        public ConfigurableJointMotion CollisionAngularMotionZ = ConfigurableJointMotion.Free;


        [Tooltip("Если значение true, скорость объекта будет скорректирована в соответствии с захватом. Это в дополнение к любым силам, добавляемым настраиваемым соединением.")]
        public bool ApplyCorrectiveForce = true;

        [Header("Настройки Захвата Скорости")]
        public float MoveVelocityForce = 3000f;
        public float MoveAngularVelocityForce = 90f;

        /// <summary>
        /// Время в секундах (Time.time), когда мы в последний раз брали этот предмет
        /// </summary>
        [HideInInspector]
        public float LastGrabTime;

        /// <summary>
        /// Время в секундах (Time.time), когда мы в последний раз отбрасывали этот элемент
        /// </summary>
        [HideInInspector]
        public float LastDropTime;

        /// <summary>
        /// Установите значение True, чтобы выбросить объект захвата, применив скорость контроллера к объекту захвата при падении.
        /// Установите значение False, если вы не хотите, чтобы объект можно было выбрасывать, или хотите применить свою собственную силу вручную
        /// </summary>
        [HideInInspector]
        public bool AddControllerVelocityOnDrop = true;

        // Общее расстояние между захватным устройством и захватным устройством.
        float journeyLength;

        public Vector3 OriginalScale { get; private set; }

        // Следите за объектами, которые сталкиваются с нами
        [Header("Показано для отладки : ")]
        [SerializeField]
        public List<Collider> collisions;

        // Последний раз в секундах (Time.time) с тех пор, как у нас произошло действительное столкновение
        public float lastCollisionSeconds { get; protected set; }

        /// <summary>
        /// Сколько секунд мы прошли без столкновений
        /// </summary>
        public float lastNoCollisionSeconds { get; protected set; }

        /// <summary>
        /// Не сталкивались ли мы недавно с каким-нибудь объектом
        /// </summary>
        public bool RecentlyCollided { 
            get {
                if(Time.time - lastCollisionSeconds <= 0.1f) {
                    return true;
                }

                if(collisions != null && collisions.Count > 0) {
                    return true;
                }
                return false;
            } 
        }

        // Если Time.time < запрашивает пружинное время, заставьте соединение быть упругим
        public float requestSpringTime { get; protected set; }

        /// <summary>
        /// Если для Grab Mechanic установлено значение Snap, установите positionandrotation для этого преобразования на основном захватчике
        /// </summary>
        protected Transform primaryGrabOffset;
        protected Transform secondaryGrabOffset;

        /// <summary>
        /// Возвращает активный компонент Точки захвата, если объект удерживается и назначена Точка захвата
        /// </summary>
        [HideInInspector]
        public GrabPoint ActiveGrabPoint;        

        [HideInInspector]
        public Vector3 SecondaryLookOffset;

        [HideInInspector]
        public Transform SecondaryLookAtTransform;

        [HideInInspector]
        public Transform LocalOffsetTransform;

        Vector3 grabPosition {
            get {
                if (primaryGrabOffset != null) {
                    return primaryGrabOffset.position;
                }
                else {
                    return transform.position;
                }
            }
        }

        [HideInInspector]
        public Vector3 GrabPositionOffset {
            get {
                if (primaryGrabOffset) {
                    return primaryGrabOffset.transform.localPosition;
                }

                return Vector3.zero;
            }
        }

        [HideInInspector]
        public Vector3 GrabRotationOffset {
            get {
                if (primaryGrabOffset) {
                    return primaryGrabOffset.transform.localEulerAngles;
                }
                return Vector3.zero;
            }
        }

        private Transform _grabTransform;

        // Расположите это на захватчике, чтобы получить точное местоположение
        public Transform grabTransform {
            get {
                if (_grabTransform != null) {
                    return _grabTransform;
                }

                _grabTransform = new GameObject().transform;
                _grabTransform.parent = this.transform;
                _grabTransform.name = "Grab Transform";
                _grabTransform.localPosition = Vector3.zero;
                // _grabTransform.hideFlags = HideFlags.HideInHierarchy;

                return _grabTransform;
            }
        }

        private Transform _grabTransformSecondary;

        // Расположите это на захватчике, чтобы получить точное местоположение
        public Transform grabTransformSecondary {
            get {
                if (_grabTransformSecondary != null) {
                    return _grabTransformSecondary;
                }

                _grabTransformSecondary = new GameObject().transform;
                _grabTransformSecondary.parent = this.transform;
                _grabTransformSecondary.name = "Захватите Вторичное преобразование";
                _grabTransformSecondary.localPosition = Vector3.zero;
                _grabTransformSecondary.hideFlags = HideFlags.HideInHierarchy;

                return _grabTransformSecondary;
            }
        }

        [Header("Точки Захвата")]
        /// <summary>
        /// Если Механизм захвата настроен на привязку, будет использоваться ближайшая точка захвата. Добавьте компонент Точки привязки к точке захвата, чтобы задать пользовательские позы рук и поворот.
        /// </summary>
        [Tooltip("Если Механизм захвата настроен на привязку, будет использоваться ближайшая точка захвата. Добавьте компонент Точки привязки к точке захвата, чтобы задать пользовательские позы рук и поворот.")]
        public List<Transform> GrabPoints;

        /// <summary>
        /// Можно ли переместить объект в сторону захвата. 
        /// Рычаги, кнопки, дверные ручки и другие типы объектов нельзя перемещать, поскольку они прикреплены к другому объекту или являются статичными.
        /// </summary>
        public bool CanBeMoved {
            get {
                return _canBeMoved;
            }
        }
        private bool _canBeMoved;

        protected Transform originalParent;
        protected InputBridge input;
        protected ConfigurableJoint connectedJoint;
        protected Vector3 previousPosition;
        protected float lastItemTeleportTime;
        protected bool recentlyTeleported;

        /// <summary>
        /// Установите для этого значение false, если вам нужно просмотреть поле отладки или вы не хотите использовать пользовательский инспектор
        /// </summary>
        [HideInInspector]
        public bool UseCustomInspector = true;

        /// <summary>
        /// Если и контроллер игрока предусмотрен, мы можем проверить движения игрока и внести определенные коррективы в физику.
        /// </summary>
        protected BNGPlayerController player {
            get {
                return GetBNGPlayerController();
            }
        }
        private BNGPlayerController _player;
        protected Collider col;
        protected Rigidbody rigid;

        public Grabber FlyingToGrabber {
            get {
                return flyingTo;
            }
        }
        protected Grabber flyingTo;

        protected List<GrabbableEvents> events;

        public bool DidParentHands {
            get {
                return didParentHands;
            }
        }
        protected bool didParentHands = false;

        protected void Awake() {
            col = GetComponent<Collider>();
            rigid = GetComponent<Rigidbody>();
            input = InputBridge.Instance;

            events = GetComponents<GrabbableEvents>().ToList();
            collisions = new List<Collider>();

            // Попробуйте parent, если здесь не найдено ни одного жесткого диска
            if (rigid == null && transform.parent != null) {
                rigid = transform.parent.GetComponent<Rigidbody>();
            }

            // Сохраните начальные свойства твердого тела, чтобы мы могли сбросить их позже по мере необходимости
            if (rigid) {
                initialCollisionMode = rigid.collisionDetectionMode;
                initialInterpolationMode = rigid.interpolation;
                wasKinematic = rigid.isKinematic;
                usedGravity = rigid.useGravity;

                // Позвольте нашему твердому телу быстро вращаться
                rigid.maxAngularVelocity = 25f;
            }

            // Сохраните начальный родительский элемент, чтобы мы могли сбросить его позже, если потребуется
            UpdateOriginalParent(transform.parent);

            validGrabbers = new List<Grabber>();

            // Установите исходный масштаб, основанный на мировых координатах, если таковые имеются
            if (transform.parent != null) {
                OriginalScale = transform.parent.TransformVector(transform.localScale);
            }
            else {
                OriginalScale = transform.localScale;
            }

            initialHandPoseId = CustomHandPose;
            initialHandPose = SelectedHandPose;
            initialHandPoseType = handPoseType;

            // статус разорванного движения
            _canBeMoved = canBeMoved();

            // Настройте любые дочерние объекты, доступные для захвата
            if (MakeChildCollidersGrabbable) {
                Collider[] cols = GetComponentsInChildren<Collider>();
                for(int x = 0; x < cols.Length; x++) {
                    // Сделайте дочерний элемент доступным для захвата, если он еще не создан
                    if (cols[x].GetComponent<Grabbable>() == null && cols[x].GetComponent<GrabbableChild>() == null) {
                        var gc = cols[x].gameObject.AddComponent<GrabbableChild>();
                        gc.ParentGrabbable = this;
                    }
                }
            }
        }

        public virtual void Update() {

            //вот мы взяли объект
            if (BeingHeld) {

                // ResetLockResets();

                // Что-то случилось с нашим Граббером. Отбросьте предмет
                if (heldByGrabbers == null) {
                    DropItem(null, true, true);
                    return;
                }

                // Убедитесь, что все коллизии допустимы
                filterCollisions();

                // Обозначение первичного захватчика кэша
                _priorPrimaryGrabber = GetPrimaryGrabber();

                // Обновить время столкновения
                if (collisions != null && collisions.Count > 0) {
                    lastCollisionSeconds = Time.time;
                    lastNoCollisionSeconds = 0;
                }
                else if (collisions != null && collisions.Count <= 0) {
                    lastNoCollisionSeconds += Time.deltaTime;
                }

                // Обновить элемент недавно телепортированного времени
                if (Vector3.Distance(transform.position, previousPosition) > 0.1f) {
                    lastItemTeleportTime = Time.time;
                }
                recentlyTeleported = Time.time - lastItemTeleportTime < 0.2f;

                // Перебираем удерживаемые захваты и смотрим, нужно ли нам удалять элемент, запускать события и т.д.
                for (int x = 0; x < heldByGrabbers.Count; x++) {
                    Grabber g = heldByGrabbers[x];

                    // Должны ли мы отбросить предмет, если он находится слишком далеко?
                    if (!recentlyTeleported && BreakDistance > 0 && Vector3.Distance(grabPosition, g.transform.position) > BreakDistance) {
                        //Debug.Log("Превышено Расстояние Разрыва. Выпадающий предмет.");
                        DropItem(g, true, true);
                        break;
                    }

                    // Должны ли мы отбросить предмет, если он больше не удерживает необходимый захват?
                    //если у нас есть оружие, которое мы должны брат ьв переменной OtherGrabbableMustBeGrabbed и при этом оно отпущено
                    if (OtherGrabbableMustBeGrabbed != null && !OtherGrabbableMustBeGrabbed.BeingHeld) {
                        // Фиксированные соединения работают нормально. У настраиваемых соединений есть проблемы
                        //if (GetComponent<ConfigurableJoint>() != null) {
                        //    DropItem(g, true, true);
                        //    break;
                        //}

                        //отпускание предмета
                        BeingHeld = false;
                        DropItem(g, true, true);
                        break;
                    }

                    //Запускайте любые соответствующие события
                    callEvents(g);
                }



                // Проверьте, чтобы родительские модели рук были привязаны к захвату
                if (ParentHandModel && !didParentHands) {
                    checkParentHands(GetPrimaryGrabber());
                }



                //Расположите руки в нужном месте
                positionHandGraphics(GetPrimaryGrabber());

                // Поверните захват, чтобы посмотреть на нашу второстепенную цель
                // ЧТО НУЖНО СДЕЛАТЬ : Переместите это в раздел обновления физики
                if (TwoHandedRotation == TwoHandedRotationType.LookAtSecondary && GrabPhysics == GrabPhysics.PhysicsJoint) {
                    checkSecondaryLook();
                }

                // Следите за тем, где мы были в каждом кадре
                previousPosition = transform.position;
            }
        }        

        public virtual void FixedUpdate() {

            if (remoteGrabbing) {
                UpdateRemoteGrab();
            }

            if (BeingHeld) {

                // Сбрасывайте все столкновения при каждом обновлении физики
                // Затем они заполняются в OnCollisionEnter / OnCollisionStay, чтобы убедиться, что у нас есть самая актуальная collisioninfo
                // Это может привести к созданию мусора, поэтому делайте это только в том случае, если мы удерживаем объект
                if (BeingHeld && collisions != null && collisions.Count > 0) {
                    collisions = new List<Collider>();
                }

                // Обновите любые физические свойства здесь
                if (GrabPhysics == GrabPhysics.PhysicsJoint) {
                    UpdatePhysicsJoints();
                }
                else if (GrabPhysics == GrabPhysics.FixedJoint) {
                    UpdateFixedJoints();
                }
                else if (GrabPhysics == GrabPhysics.Kinematic) {
                    UpdateKinematicPhysics();
                }
                else if (GrabPhysics == GrabPhysics.Velocity) {
                    UpdateVelocityPhysics();
                }
            }
        }        

        public virtual Vector3 GetGrabberWithGrabPointOffset(Grabber grabber, Transform grabPoint) {
            // Проверка на вменяемость
            if (grabber == null) {
                return Vector3.zero;
            }

            // Получить положение захвата, смещенное на точку захвата
            Vector3 grabberPosition = grabber.transform.position;
            if (grabPoint != null) {
                grabberPosition += transform.position - grabPoint.position;
            }

            return grabberPosition;

        }

        public virtual Quaternion GetGrabberWithOffsetWorldRotation(Grabber grabber) {

            if(grabber != null) {
                return grabber.transform.rotation;
            }

            return Quaternion.identity;
        }

        protected void positionHandGraphics(Grabber g) {
            if (ParentHandModel && didParentHands) {
                if (GrabMechanic == GrabType.Snap) {                    
                    if(g != null) {
                        g.HandsGraphics.localPosition = g.handsGraphicsGrabberOffset;
                        g.HandsGraphics.localEulerAngles = Vector3.zero;
                    }
                }
            }
        }

        /// <summary>
        /// Можно ли схватить этот объект? Не проверяет наличие допустимых захватов, только если он не удерживается, активен и т.д.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsGrabbable() {

            // Недействителен, если не активен
            if (!isActiveAndEnabled) {
                return false;
            }

            // Недопустимо, если удерживается и объект не имеет вторичного поведения захвата
            if (BeingHeld == true && SecondaryGrabBehavior == OtherGrabBehavior.None) {
                return false;
            }

            // Не поддается захвату, если задано значение двойного захвата, но была указана вторичная возможность захвата. Это означает, что мы не можем использовать точку захвата для этого объекта
            if (BeingHeld == true && SecondaryGrabBehavior == OtherGrabBehavior.DualGrab && SecondaryGrabbable != null) {
                return false;
            }

            // Убедитесь, что все условия соблюдены
            if (OtherGrabbableMustBeGrabbed != null && !OtherGrabbableMustBeGrabbed.BeingHeld) {
                return false;
            }

            return true;
        }



        public virtual void UpdateRemoteGrab() {

            // Линейное Перемещение
            if (RemoteGrabMechanic == RemoteGrabMovement.Linear) {
                CheckRemoteGrabLinear();
            }
            else if (RemoteGrabMechanic == RemoteGrabMovement.Velocity) {
                CheckRemoteGrabVelocity();
            }
            else if (RemoteGrabMechanic == RemoteGrabMovement.Flick) {
                CheckRemoteGrabFlick();
            }
        }


        
        public virtual void CheckRemoteGrabLinear() {
            // Освободитесь пораньше, если мы не будем дистанционно захватывать этот предмет
            if (!remoteGrabbing) {
                return;
            }

            // Перемещайте объект линейно как кинематическое твердое тело
            if (rigid && !rigid.isKinematic) {
                rigid.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigid.isKinematic = true;
            }

            Vector3 grabberPosition = GetGrabberWithGrabPointOffset(flyingTo, GetClosestGrabPoint(flyingTo));
            Quaternion remoteRotation = getRemoteRotation(flyingTo);
            float distance = Vector3.Distance(transform.position, grabberPosition);

            // достигнув пункта назначения, привязать к конечному положению преобразования
            // Как правило, это не будет выполнено, так как триггер захвата подхватит его первым
            if (distance <= 0.002f) {
                movePosition(grabberPosition);
                moveRotation(grabTransform.rotation);

                if (rigid) {
                    rigid.velocity = Vector3.zero;
                }

                if (flyingTo != null) {
                    flyingTo.GrabGrabbable(this);
                }
            }
            // Приближаемся, так что ускоряйся
            else if (distance < 0.03f) {
                movePosition(Vector3.MoveTowards(transform.position, grabberPosition, Time.fixedDeltaTime * GrabSpeed * 2f));
                moveRotation(Quaternion.Slerp(transform.rotation, remoteRotation, Time.fixedDeltaTime * GrabSpeed * 2f));
            }
            // Обычный Lerp
            else
            {
                movePosition(Vector3.Lerp(transform.position, grabberPosition, Time.fixedDeltaTime * GrabSpeed));
                moveRotation(Quaternion.Slerp(transform.rotation, remoteRotation, Time.fixedDeltaTime * GrabSpeed));
            }
        }


      
        public virtual void CheckRemoteGrabVelocity() {
            if (remoteGrabbing) {

                Vector3 grabberPosition = GetGrabberWithGrabPointOffset(flyingTo, GetClosestGrabPoint(flyingTo));
                Quaternion remoteRotation = getRemoteRotation(flyingTo);
                float distance = Vector3.Distance(transform.position, grabberPosition);

                //Перемещайте объект со скоростью, не используя силу тяжести
                if (rigid && rigid.useGravity) {
                    rigid.useGravity = false;

                    // Мгновенный поворот один раз
                    // transform.rotation = remoteRotation;
                }

                // достигнув пункта назначения, привязать к конечному положению преобразования
                // Как правило, это не будет выполнено, так как триггер захвата подхватит его первым
                if (distance <= 0.0025f) {
                    movePosition(grabberPosition);
                    moveRotation(grabTransform.rotation);

                    if (rigid) {
                        rigid.velocity = Vector3.zero;
                    }

                    if (flyingTo != null) {
                        flyingTo.GrabGrabbable(this);
                    }
                }
                else {
                    // Двигайтесь со скоростью
                    Vector3 positionDelta = grabberPosition - transform.position;

                    // Двигайтесь к руке, используя скорость
                    rigid.velocity = Vector3.MoveTowards(rigid.velocity, (positionDelta * MoveVelocityForce) * Time.fixedDeltaTime, 1f);

                    rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, GetGrabbersAveragedRotation(), Time.fixedDeltaTime * GrabSpeed));
                    //rigid.angularVelocity = Vector3.zero;
                    //moveRotation(Quaternion.Slerp(transform.rotation, remoteRotation, Time.fixedDeltaTime * GrabSpeed));
                }
            }
        }


        bool initiatedFlick = false;
        // Угловая скорость, необходимая для запуска силы щелчка
        float flickStartVelocity = 1.5f;

        /// <summary>
        /// Сколько времени в секундах должно потребоваться объекту для перехода к захвату при использовании типа удаленного захвата щелчком мыши
        /// </summary>
        float FlickSpeed = 0.5f;

        public float lastFlickTime;



       
        public virtual void InitiateFlick() {

            initiatedFlick = true;

            lastFlickTime = Time.time;

            Vector3 grabberPosition = flyingTo.transform.position;// GetGrabberWithGrabPointOffset(flyingTo, GetClosestGrabPoint(flyingTo));
            Quaternion remoteRotation = getRemoteRotation(flyingTo);
            float distance = Vector3.Distance(transform.position, grabberPosition);

            // Значение по умолчанию равно 1, но ускоряется, если закрыть
            float timeToGrab = FlickSpeed;
            if (distance < 1f) {
                timeToGrab = FlickSpeed / 1.5f;
            }
            else if (distance < 0.5f) {
                timeToGrab = FlickSpeed / 3;
            }

            Vector3 vel = GetVelocityToHitTargetByTime(transform.position, grabberPosition, Physics.gravity * 1.1f, timeToGrab);

            rigid.velocity = vel;
            // rigid.AddForce(vel, ForceMode.VelocityChange);

            // Больше не инициированный щелчок
            initiatedFlick = false;
        }




    
        public Vector3 GetVelocityToHitTargetByTime(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float timeToTarget) {

            Vector3 direction = targetPosition - startPosition;
            Vector3 horizontal = Vector3.Project(direction, Vector3.Cross(gravityBase, Vector3.Cross(direction, gravityBase)));
            
            float horizontalDistance = horizontal.magnitude;
            float horizontalSpeed = horizontalDistance / timeToTarget;

            Vector3 vertical = Vector3.Project(direction, gravityBase);
            float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));
            float verticalSpeed = (verticalDistance + ((0.5f * gravityBase.magnitude) * (timeToTarget * timeToTarget))) / timeToTarget;

            return (horizontal.normalized * horizontalSpeed) - (gravityBase.normalized * verticalSpeed);
        }


       
        public virtual void CheckRemoteGrabFlick() {
            if(remoteGrabbing) {

                // Мы уже запустили фильм?
                if (!initiatedFlick) {
                    // Получить угловую скорость от контроллера
                    if (InputBridge.Instance.GetControllerAngularVelocity(flyingTo.HandSide).magnitude >= flickStartVelocity) {
                        // Должно быть, по крайней мере, какое-то время между щелчками
                        if (Time.time - lastFlickTime >= 0.1f) {
                            InitiateFlick();
                        }
                    }
                }
            }
            else {
                initiatedFlick = false;
            }
        }

        public float FlickForce = 1f;




        
        public virtual void UpdateFixedJoints() {
            // Установить непрерывную динамику во время удерживания
            if (rigid != null && rigid.isKinematic) {
                rigid.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
            else {
                rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            // Отрегулируйте скорость элемента. Это сглаживает силы, становясь при этом жестким
            if (ApplyCorrectiveForce) {
                moveWithVelocity();
            }           
        }




        
        public virtual void UpdatePhysicsJoints() {

            // Залог, если ни один сустав не соединен
            if (connectedJoint == null || rigid == null) {
                return;
            }

            //Установить непрерывную динамику во время удерживания
            if (rigid.isKinematic) {
                rigid.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
            else {
                rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            // Обновляйте положение сустава в режиме реального времени
            if (GrabMechanic == GrabType.Snap) {
                connectedJoint.anchor = Vector3.zero;
                connectedJoint.connectedAnchor = GrabPositionOffset;
            }

            // Проверьте, не требует ли что-то упругого соединения
            // Например, пистолет может пожелать сделать шарнир пружинистым, чтобы применить отдачу к оружию с помощью дополнительной силы
            bool forceSpring = Time.time < requestSpringTime;

            // Хватайтесь за жесткий захват только в том случае, если после нашего последнего столкновения произошла небольшая задержка
            // Это предотвращает быстрое становление сустава жестким / пружинистым, что может привести к неустойчивому поведению
            bool afterCollision = collisions.Count == 0 && lastNoCollisionSeconds >= 0.1f;

            // Ничто не прикасается к нему, так что мы можем жестко держаться за руку
            // Двуручное оружие в настоящее время реагирует гораздо более плавно, если сустав жесткий, из-за того, как они смотрят На работу системы
            if ((BeingHeldWithTwoHands || afterCollision) && !forceSpring) {
                // Lock Angular, XYZ Motion
                // Make joint very rigid
                connectedJoint.rotationDriveMode = RotationDriveMode.Slerp;
                connectedJoint.xMotion = ConfigurableJointMotion.Limited;
                connectedJoint.yMotion = ConfigurableJointMotion.Limited;
                connectedJoint.zMotion = ConfigurableJointMotion.Limited;
                connectedJoint.angularXMotion = ConfigurableJointMotion.Limited;
                connectedJoint.angularYMotion = ConfigurableJointMotion.Limited;
                connectedJoint.angularZMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit sjl = connectedJoint.linearLimit;
                sjl.limit = 15f;

                SoftJointLimitSpring sjlsp = connectedJoint.linearLimitSpring;
                sjlsp.spring = 3000;
                sjlsp.damper = 10f;

                // Set X,Y, and Z drive to our values
                // Set X,Y, and Z drive to our values
                setPositionSpring(CollisionSpring, 10f);

                // Привод Slerp, используемый для вращения
                setSlerpDrive(CollisionSlerp, 10f);

                // Отрегулируйте скорость элемента. Это сглаживает силы, становясь при этом жестким
                if (ApplyCorrectiveForce) {
                    moveWithVelocity();
                }
            }
            else {
                // Сделать Пружинистым
                connectedJoint.rotationDriveMode = RotationDriveMode.Slerp;
                connectedJoint.xMotion = CollisionLinearMotionX;
                connectedJoint.yMotion = CollisionLinearMotionY;
                connectedJoint.zMotion = CollisionLinearMotionZ;
                connectedJoint.angularXMotion = CollisionAngularMotionX;
                connectedJoint.angularYMotion = CollisionAngularMotionY;
                connectedJoint.angularZMotion = CollisionAngularMotionZ;

                SoftJointLimitSpring sp = connectedJoint.linearLimitSpring;
                sp.spring = 5000;
                sp.damper = 5;

                // Set X,Y, and Z drive to our values
                setPositionSpring(CollisionSpring, 5f);

                // Slerp drive used for rotation
                setSlerpDrive(CollisionSlerp, 5f);
            }







            












            if (BeingHeldWithTwoHands && SecondaryLookAtTransform != null) {
                connectedJoint.angularXMotion = ConfigurableJointMotion.Free;

                setSlerpDrive(1000f, 2f);
                connectedJoint.angularYMotion = ConfigurableJointMotion.Limited;


                connectedJoint.angularZMotion = ConfigurableJointMotion.Limited;

                if (TwoHandedRotation == TwoHandedRotationType.LookAtSecondary) {
                    checkSecondaryLook();
                }
            }
        }

        void setPositionSpring(float spring, float damper) {

            if(connectedJoint == null) {
                return;
            }

            JointDrive xDrive = connectedJoint.xDrive;
            xDrive.positionSpring = spring;
            xDrive.positionDamper = damper;
            connectedJoint.xDrive = xDrive;

            JointDrive yDrive = connectedJoint.yDrive;
            yDrive.positionSpring = spring;
            yDrive.positionDamper = damper;
            connectedJoint.yDrive = yDrive;

            JointDrive zDrive = connectedJoint.zDrive;
            zDrive.positionSpring = spring;
            zDrive.positionDamper = damper;
            connectedJoint.zDrive = zDrive;
        }

        void setSlerpDrive(float slerp, float damper) {
            if(connectedJoint) {
                JointDrive slerpDrive = connectedJoint.slerpDrive;
                slerpDrive.positionSpring = slerp;
                slerpDrive.positionDamper = damper;
                connectedJoint.slerpDrive = slerpDrive;
            }
        }


        //Какой выбран захват в любом месте или точный
        public virtual Vector3 GetGrabberVector3(Grabber grabber, bool isSecondary) {
            // Щелкать
            if (GrabMechanic == GrabType.Snap) {
                return GetGrabberWithGrabPointOffset(grabber, isSecondary ? secondaryGrabOffset : primaryGrabOffset);
            }
            // Точный
            else
            {
                if (isSecondary) {
                    return grabTransformSecondary.position;
                }

                return grabTransform.position;
            }
        }



       
        public virtual Quaternion GetGrabberQuaternion(Grabber grabber, bool isSecondary) {

            if (GrabMechanic == GrabType.Snap) {
                return GetGrabberWithOffsetWorldRotation(grabber);
            }
            else {
                if (isSecondary) {
                    return grabTransformSecondary.rotation;
                }

                return grabTransform.rotation;
            }
        }

        /// <summary>
        /// Примените скорость к нашему захвату по направлению к нашему захвату
        /// </summary>
        void moveWithVelocity()
        {

            if (rigid == null) { return; }

            Vector3 destination = GetGrabbersAveragedPosition();

            float distance = Vector3.Distance(transform.position, destination);

            if (distance > 0.002f)
            {
                Vector3 positionDelta = destination - transform.position;

                // Move towards hand using velocity
                rigid.velocity = Vector3.MoveTowards(rigid.velocity, (positionDelta * MoveVelocityForce) * Time.fixedDeltaTime, 1f);
            }
            else
            {
                // Very close - just move object right where it needs to be and set velocity to 0 so it doesn't overshoot
                rigid.MovePosition(destination);
                rigid.velocity = Vector3.zero;
            }
        }

        float angle;
        Vector3 axis, angularTarget, angularMovement;

        void rotateWithVelocity() {

            if(rigid == null) {
                return;
            }

            bool noRecentCollisions = collisions != null && collisions.Count == 0 && lastNoCollisionSeconds >= 0.5f;
            bool moveInstantlyOneHand = InstantMovement; // MoveAngularVelocityForce >= 200f;
            bool moveInstantlyTwoHands = BeingHeldWithTwoHands && InstantMovement; // TwoHandedRotation == TwoHandedRotationType.LookAtSecondary && SecondHandLookSpeed > 20;

            if (InstantMovement == true && noRecentCollisions && (moveInstantlyOneHand || moveInstantlyTwoHands)) {
                //rigid.rotation = GetGrabbersAveragedRotation();
                rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, GetGrabbersAveragedRotation(), Time.fixedDeltaTime * SecondHandLookSpeed));

                // Может немедленно выйти
                return;
            }

            Quaternion rotationDelta = GetGrabbersAveragedRotation() * Quaternion.Inverse(transform.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            // Используйте ближайший поворот. Если больше 180 градусов, поверните в другую сторону
            if (angle > 180) {
                angle -= 360;
            }

            if (angle != 0) {
                angularTarget = angle * axis;
                angularTarget = (angularTarget * MoveAngularVelocityForce) * Time.fixedDeltaTime;

                angularMovement = Vector3.MoveTowards(rigid.angularVelocity, angularTarget, MoveAngularVelocityForce);

                if (angularMovement.magnitude > 0.05f) {
                    // rigid.centerOfMass = transform.InverseTransformPoint(GetGrabbersAveragedPosition());
                    rigid.angularVelocity = angularMovement;
                }

                // Защелкивается на месте, если очень близко
                if (angle < 1) {
                    rigid.MoveRotation(GetGrabbersAveragedRotation());
                    rigid.angularVelocity = Vector3.zero;
                }
            }
        }

        /// <summary>
        /// Получите предполагаемое мировое положение захвата (захватов), удерживающего этот объект. Факторы положения в вариантах захвата двумя руками
        /// </summary>
        /// <returns>Мировое положение граббера с учетом двуручного поведения.</returns>
        /// 


        public Vector3 GetGrabbersAveragedPosition() {
            // Начните с нашего основного граббера
            Vector3 destination = GetGrabberVector3(GetPrimaryGrabber(), false);

            // Добавить дополнительную позицию захвата
            if (SecondaryGrabBehavior == OtherGrabBehavior.DualGrab && TwoHandedPosition == TwoHandedPositionType.Lerp) {
                // Сначала проверьте Вторичный захват
                if (SecondaryGrabbable != null && SecondaryGrabbable.BeingHeld) {
                    // Добавить дополнительное положение захвата
                    destination = Vector3.Lerp(destination, SecondaryGrabbable.GetGrabberVector3(SecondaryGrabbable.GetPrimaryGrabber(), false), TwoHandedPostionLerpAmount);
                }
                // Проверьте, удерживает ли захват этот объект
                else if (heldByGrabbers != null && heldByGrabbers.Count > 1) {
                    destination = Vector3.Lerp(destination, GetGrabberVector3(heldByGrabbers[1], true), TwoHandedPostionLerpAmount);
                }
            }

            //Вернуть исходное положение захвата по умолчанию
            return destination;
        }



        
        public Quaternion GetGrabbersAveragedRotation() {
            // Начните с вращения нашего основного граббера
            Quaternion destination = GetGrabberQuaternion(GetPrimaryGrabber(), false);

            // Добавить дополнительную позицию захвата
            // Check Lerp / Slerp Setting
            if (SecondaryGrabBehavior == OtherGrabBehavior.DualGrab && TwoHandedRotation == TwoHandedRotationType.Lerp || TwoHandedRotation == TwoHandedRotationType.Slerp) {
                // Сначала проверьте Вторичный захват
                if (SecondaryGrabbable != null && SecondaryGrabbable.BeingHeld) {
                    if (TwoHandedRotation == TwoHandedRotationType.Lerp) {
                        destination = Quaternion.Lerp(destination, SecondaryGrabbable.GetGrabberQuaternion(SecondaryGrabbable.GetPrimaryGrabber(), false), TwoHandedRotationLerpAmount);
                    }
                    else {
                        destination = Quaternion.Slerp(destination, SecondaryGrabbable.GetGrabberQuaternion(SecondaryGrabbable.GetPrimaryGrabber(), false), TwoHandedRotationLerpAmount);
                    }
                }
                // Проверьте, удерживает ли захват этот объект
                else if (heldByGrabbers != null && heldByGrabbers.Count > 1) {
                    if (TwoHandedRotation == TwoHandedRotationType.Lerp) {
                        destination = Quaternion.Lerp(destination, GetGrabberQuaternion(heldByGrabbers[1], true), TwoHandedRotationLerpAmount);
                    }
                    else {
                        destination = Quaternion.Slerp(destination, GetGrabberQuaternion(heldByGrabbers[1], true), TwoHandedRotationLerpAmount);
                    }
                }
            }
            //Посмотрите На тип
            else if (SecondaryGrabBehavior == OtherGrabBehavior.DualGrab && TwoHandedRotation == TwoHandedRotationType.LookAtSecondary) {
                // Поверните наш основной захват в сторону нашего вторичного захвата
                // Сначала проверьте Вторичный захват
                if (SecondaryGrabbable != null && SecondaryGrabbable.BeingHeld) {
                    
                    Vector3 targetVector = GetGrabberVector3(SecondaryGrabbable.GetPrimaryGrabber(), false) - GetGrabberVector3(GetPrimaryGrabber(), false);

                    // Направление движения Вперед
                    if (TwoHandedLookVector == TwoHandedLookDirection.Horizontal) {
                        destination = Quaternion.LookRotation(targetVector, -GetPrimaryGrabber().transform.up) * Quaternion.AngleAxis(180f, Vector3.up) * Quaternion.AngleAxis(180f, Vector3.forward);
                    }
                    //Делать вверх / вниз
                    else if (TwoHandedLookVector == TwoHandedLookDirection.Vertical) {
                        destination = Quaternion.LookRotation(targetVector, -GetPrimaryGrabber().transform.right) * Quaternion.AngleAxis(90f, Vector3.right) * Quaternion.AngleAxis(180f, Vector3.forward) * Quaternion.AngleAxis(-90f, Vector3.up);
                    }
                }
                // Проверьте, удерживает ли захват этот объект
                else if (heldByGrabbers != null && heldByGrabbers.Count > 1) {
                    // destination = Quaternion.Lerp(destination, GetGrabberQuaternion(heldByGrabbers[1], true), TwoHandedRotationLerpAmount);
                }
            }

            return destination;
        }




        public virtual void UpdateKinematicPhysics()
        {

            //Пройденное расстояние равно затраченному времени, умноженному на скорость.
            float distCovered = (Time.time - LastGrabTime) * GrabSpeed;

            // Как далеко мы продвинулись
            float fractionOfJourney = distCovered / journeyLength;

            Vector3 destination = GetGrabbersAveragedPosition();
            Quaternion destRotation = grabTransform.rotation;

            // Положение обновления в режиме реального времени для упрощения предварительного просмотра преобразований захвата
            bool realtime = Application.isEditor;
            if (realtime)
            {

                destination = getRemotePosition(GetPrimaryGrabber());
                //destRotation = getRemoteRotation(GetPrimaryGrabber());
                rotateGrabber(false);


            }



          
                if (GrabMechanic == GrabType.Snap)
                {
                // Установите нашу позицию как долю расстояния между маркерами.
                Grabber g = GetPrimaryGrabber();

                    //Обновление локального преобразования в режиме реального времени
                    if (g != null)
                    {
                        if (ParentToHands)
                        {

                            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero - GrabPositionOffset, fractionOfJourney);
                            transform.localRotation = Quaternion.Lerp(transform.localRotation, grabTransform.localRotation, Time.deltaTime * 10);

                        }
                        // Расположите объект в мировом пространстве, используя физику
                        else
                        {
                            movePosition(Vector3.Lerp(transform.position, destination, fractionOfJourney));
                            moveRotation(Quaternion.Lerp(transform.rotation, destRotation, Time.deltaTime * 20));
                        }
                    }
                    else
                    {
                        movePosition(destination);
                        transform.localRotation = grabTransform.localRotation;
                    }
                }
                else if (GrabMechanic == GrabType.Precise)
                {
                    movePosition(grabTransform.position);
                    moveRotation(grabTransform.rotation);
                }

        }





        public virtual void UpdateVelocityPhysics()
        {

            // Make sure rotation is always free
            if (connectedJoint != null)
            {
                connectedJoint.xMotion = ConfigurableJointMotion.Free;
                connectedJoint.yMotion = ConfigurableJointMotion.Free;
                connectedJoint.zMotion = ConfigurableJointMotion.Free;
                connectedJoint.angularXMotion = ConfigurableJointMotion.Free;
                connectedJoint.angularYMotion = ConfigurableJointMotion.Free;
                connectedJoint.angularZMotion = ConfigurableJointMotion.Free;
            }

            // Make sure linear spring is off
            // Set X,Y, and Z drive to our values
            setPositionSpring(0, 0.5f);

            // Slerp drive used for rotation
            setSlerpDrive(5, 0.5f);

            // Update collision detection mode to ContinuousDynamic while being held
            if (rigid && rigid.isKinematic)
            {
                rigid.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
            else if (rigid)
            {
                rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            moveWithVelocity();
            rotateWithVelocity();

            //Parent to our hands if no colliisions present
            // This makes our object move 1:1 with our controller
            if (ParentToHands)
            {
                // Parent to hands if no collisions
                bool afterCollision = collisions.Count == 0 && lastNoCollisionSeconds >= 0.2f;
                // Set parent to us to keep movement smoothed
                if (afterCollision)
                {
                    Grabber g = GetPrimaryGrabber();
                    transform.parent = g.transform;
                }
                else
                {
                    transform.parent = null;
                }
            }
        }


        void checkParentHands(Grabber g) {

            //Если еу нас есть предмет в руке и есть граббер
            if (ParentHandModel && g != null) {


                // Точный - Продолжайте и немедленно моделируйте родительские руки. Выполняется в случае, если можно брать предмет где угодно
                if (GrabMechanic == GrabType.Precise) {
                    parentHandGraphics(g);
                }

                // Модели с защелкивающейся рукой, если они достаточно близко. Иначе можно брать предмет в определённой точке.
                else
                {
                    //Vector3 grabberPosition = g.transform.position;
                    Vector3 grabberPosition = grabTransform.position;
                    Vector3 grabbablePosition = transform.position;

                    float distance = Vector3.Distance(grabbablePosition, grabberPosition);

                    // Если объект можно переместить к захвату, подождите, пока предмет не окажется рядом, прежде чем привязать к нему руку
                    if (CanBeMoved) {

                        //Debug.Log("Объект можно привязать к руке, он не рычаг");
                        //перетащил его из условия ниже, т.к. условие не выполнялось и поэтому рука не перемещалась к игроку
                        parentHandGraphics(g);
                        // Достаточно близко, чтобы привязать ручную графику
                        if (distance < 0.001f ) {

                            //Debug.Log("Выполняем функцию привязки.. перемещения");
                            // Положение привязки
                            parentHandGraphics(g);

                            // Положение Модели Защелкивающейся Руки
                            if (g.HandsGraphics != null) {
                                g.HandsGraphics.localEulerAngles = Vector3.zero;
                                g.HandsGraphics.localPosition = g.handsGraphicsGrabberOffset;
                            }
                        }
                    }
                    else {
                        // Нельзя сдвинуть с места, так что давай и хватай
                        if (grabTransform != null && distance < 0.1f) {

                            // Положение привязки
                            parentHandGraphics(g);
                            positionHandGraphics(g); 

                            if (g.HandsGraphics != null) {
                                g.HandsGraphics.localEulerAngles = Vector3.zero;
                                g.HandsGraphics.localPosition = g.handsGraphicsGrabberOffset;
                            }
                        }
                    }
                }
            }
        }

        // Может ли этот объект быть перемещен к объекту, или он закреплен на месте / прикреплен к чему-то другому
        bool canBeMoved() {

            if (GetComponent<Rigidbody>() == null) {
                return false;
            }

            if (GetComponent<Joint>() != null) {
                return false;
            }

            return true;
        }

        void checkSecondaryLook() {

            // Создайте преобразование, чтобы посмотреть, смотрим ли мы на точный захват
            if (BeingHeldWithTwoHands) {
                if (SecondaryLookAtTransform == null) {
                    Grabber thisGrabber = GetPrimaryGrabber();
                    Grabber secondaryGrabber = SecondaryGrabbable.GetPrimaryGrabber();

                    GameObject o = new GameObject();
                    SecondaryLookAtTransform = o.transform;
                    SecondaryLookAtTransform.name = "LookAtTransformTemp";
                    // Точный захват может использовать текущее положение захвата
                    if (SecondaryGrabbable.GrabMechanic == GrabType.Precise) {
                        SecondaryLookAtTransform.position = secondaryGrabber.transform.position;
                    }
                    //В противном случае используйте точку привязки
                    else
                    {
                        Transform grabPoint = SecondaryGrabbable.GetGrabPoint();
                        if (grabPoint) {
                            SecondaryLookAtTransform.position = grabPoint.position;
                        }
                        else {
                            SecondaryLookAtTransform.position = SecondaryGrabbable.transform.position;
                        }

                        SecondaryLookAtTransform.position = SecondaryGrabbable.transform.position;
                    }

                    if (SecondaryLookAtTransform && thisGrabber) {
                        SecondaryLookAtTransform.parent = thisGrabber.transform;
                        SecondaryLookAtTransform.localEulerAngles = Vector3.zero;
                        SecondaryLookAtTransform.localPosition = new Vector3(0, 0, SecondaryLookAtTransform.localPosition.z);

                        // Переместить родительский элемент обратно в захват
                        SecondaryLookAtTransform.parent = secondaryGrabber.transform;
                    }
                }
            }

            // Мы не должны были бы стремиться ни к чему, если бы был указан объект захвата
            if (SecondaryGrabbable != null && !SecondaryGrabbable.BeingHeld && SecondaryLookAtTransform != null) {
                clearLookAtTransform();
            }

            Grabber heldBy = GetPrimaryGrabber();
            if (heldBy) {
                Transform grabberTransform = heldBy.transform;

                if (SecondaryLookAtTransform != null) {
                    Vector3 initialRotation = grabberTransform.localEulerAngles;

                    Quaternion dest = Quaternion.LookRotation(SecondaryLookAtTransform.position - grabberTransform.position, Vector3.up);
                    grabberTransform.rotation = Quaternion.Slerp(grabberTransform.rotation, dest, Time.deltaTime * SecondHandLookSpeed);

                    // Исключить повороты только на x и y
                    grabberTransform.localEulerAngles = new Vector3(grabberTransform.localEulerAngles.x, grabberTransform.localEulerAngles.y, initialRotation.z);
                }
                else {
                    rotateGrabber(true);
                }
            }
        }

        void rotateGrabber(bool lerp = false) {
            Grabber heldBy = GetPrimaryGrabber();
            if (heldBy != null) {
                Transform grabberTransform = heldBy.transform;

                if (lerp) {
                    grabberTransform.localRotation = Quaternion.Slerp(grabberTransform.localRotation, Quaternion.Inverse(Quaternion.Euler(GrabRotationOffset)), Time.deltaTime * 20);
                }
                else {
                    grabberTransform.localRotation = Quaternion.Inverse(Quaternion.Euler(GrabRotationOffset));
                }
            }
        }

        public Transform GetGrabPoint() {
            return primaryGrabOffset;
        }


     
        public virtual void GrabItem(Grabber grabbedBy) {

            // Убедитесь, что мы выпустили этот товар
            if (BeingHeld && SecondaryGrabBehavior != OtherGrabBehavior.DualGrab) {
                DropItem(false, true);
            }

            bool isPrimaryGrab = !BeingHeld;
            bool isSecondaryGrab = BeingHeld && SecondaryGrabBehavior == OtherGrabBehavior.DualGrab;

            //Официально проводится
            BeingHeld = true;
            LastGrabTime = Time.time;

            // Основной граббер только что схватил этот предмет
            if (isPrimaryGrab) {
                // Сначала убедитесь, что все значения сброшены
                ResetGrabbing();

                // Установите, куда элемент будет перемещаться на захватчике
                primaryGrabOffset = GetClosestGrabPoint(grabbedBy);
                secondaryGrabOffset = null;

                //// Теперь этот объект принадлежит нам
                //base.photonView.RequestOwnership();

                // Установите активную точку захвата, которую мы будем использовать
                if (primaryGrabOffset) {
                    ActiveGrabPoint = primaryGrabOffset.GetComponent<GrabPoint>();
                }
                else {
                    ActiveGrabPoint = null;
                }

                // Обновить Идентификатор Позы Руки
                if (primaryGrabOffset != null && ActiveGrabPoint != null) {
                    CustomHandPose = primaryGrabOffset.GetComponent<GrabPoint>().HandPose;
                    SelectedHandPose = primaryGrabOffset.GetComponent<GrabPoint>().SelectedHandPose;
                    handPoseType = primaryGrabOffset.GetComponent<GrabPoint>().handPoseType;
                }
                else {
                    CustomHandPose = initialHandPoseId;
                    SelectedHandPose = initialHandPose;
                    handPoseType = initialHandPoseType;
                }

                // Обновление, проводимое свойствами
                addGrabber(grabbedBy);
                grabTransform.parent = grabbedBy.transform;
                rotateGrabber(false);

                // Используйте центр захвата при привязке
                if (GrabMechanic == GrabType.Snap) {
                    grabTransform.localEulerAngles = Vector3.zero;
                    grabTransform.localPosition = -GrabPositionOffset;
                }
                // Точное удержание может использовать положение того, что мы захватываем
                else if (GrabMechanic == GrabType.Precise) {
                    grabTransform.position = transform.position;
                    grabTransform.rotation = transform.rotation;
                }

                // Сначала удалите все соединенные соединения, если это необходимо
                var projectile = GetComponent<Projectile>();
                if (projectile) {
                    var fj = GetComponent<FixedJoint>();
                    if (fj) {
                        Destroy(fj);
                    }
                }

                // Установите любые соответствующие соединения или необходимые компоненты
                if (GrabPhysics == GrabPhysics.PhysicsJoint) {
                    setupConfigJointGrab(grabbedBy, GrabMechanic);
                }
                else if (GrabPhysics == GrabPhysics.Velocity) {
                    setupVelocityGrab(grabbedBy, GrabMechanic);
                }
                else if (GrabPhysics == GrabPhysics.FixedJoint) {
                    setupFixedJointGrab(grabbedBy, GrabMechanic);
                }
                else if (GrabPhysics == GrabPhysics.Kinematic) {
                    setupKinematicGrab(grabbedBy, GrabMechanic);
                }

                // Остановите наш объект при первоначальном захвате
                if (rigid) {
                    rigid.velocity = Vector3.zero;
                    rigid.angularVelocity = Vector3.zero;
                }


                // Дайте событиям знать, что нас схватили
                for (int x = 0; x < events.Count; x++) {
                    events[x].OnGrab(grabbedBy);
                }

                checkParentHands(grabbedBy);

                // Модель Перемещения Руки
                if (GrabMechanic == GrabType.Precise && SnapHandModel && primaryGrabOffset != null && grabbedBy.HandsGraphics != null) {
                    grabbedBy.HandsGraphics.transform.parent = primaryGrabOffset;
                    grabbedBy.HandsGraphics.localPosition = grabbedBy.handsGraphicsGrabberOffset;
                    grabbedBy.HandsGraphics.localEulerAngles = grabbedBy.handsGraphicsGrabberOffsetRotation;
                }

                SubscribeToMoveEvents();

            }
            else if (isSecondaryGrab) {
                // Установите, куда элемент будет перемещаться на захватчике
                secondaryGrabOffset = GetClosestGrabPoint(grabbedBy);

                // Обновление, проводимое свойствами
                addGrabber(grabbedBy);

                grabTransformSecondary.parent = grabbedBy.transform;

                // Используйте центр захвата при привязке
                if (GrabMechanic == GrabType.Snap) {
                    grabTransformSecondary.localEulerAngles = Vector3.zero;
                    grabTransformSecondary.localPosition = GrabPositionOffset;
                }
                // Точное удержание может использовать положение того, что мы захватываем
                else if (GrabMechanic == GrabType.Precise) {
                    grabTransformSecondary.position = transform.position;
                    grabTransformSecondary.rotation = transform.rotation;
                }

                checkParentHands(grabbedBy);

                //Модель перемещения руки, если щелкнуть руками и точно
                if (GrabMechanic == GrabType.Precise && SnapHandModel && secondaryGrabOffset != null && grabbedBy.HandsGraphics != null) {
                    grabbedBy.HandsGraphics.transform.parent = secondaryGrabOffset;
                    grabbedBy.HandsGraphics.localPosition = grabbedBy.handsGraphicsGrabberOffset;
                    grabbedBy.HandsGraphics.localEulerAngles = grabbedBy.handsGraphicsGrabberOffsetRotation;
                }
            }

            //При необходимости скройте графику руки
            if (HideHandGraphics) {
                grabbedBy.HideHandGraphics();
            }

            journeyLength = Vector3.Distance(grabPosition, grabbedBy.transform.position);
        }

        protected virtual void setupConfigJointGrab(Grabber grabbedBy, GrabType grabType) {
            // Set up the new connected joint
            if (GrabMechanic == GrabType.Precise) {
                connectedJoint = grabbedBy.GetComponent<ConfigurableJoint>();
                connectedJoint.connectedBody = rigid;
                // Just let the autoconfigure handle the calculations for us
                connectedJoint.autoConfigureConnectedAnchor = true;
            }

            // Set up the physics joint for snapping
            else if (GrabMechanic == GrabType.Snap) {
                // Need to Fix Rotation on Snap Physics when close by
                transform.rotation = grabTransform.rotation;

                // Setup joint
                setupConfigJoint(grabbedBy);

                rigid.MoveRotation(grabTransform.rotation);
            }
        }

        protected virtual void setupFixedJointGrab(Grabber grabbedBy, GrabType grabType) {
            FixedJoint joint = grabbedBy.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = rigid;

            // Setup Fixed Joint in place
            if (GrabMechanic == GrabType.Precise) {
                // Just let the autoconfigure handle the calculations for us
                joint.autoConfigureConnectedAnchor = true;
            }
            // Setup the snap point manually
            else if (GrabMechanic == GrabType.Snap) {
                joint.autoConfigureConnectedAnchor = false;
                joint.anchor = Vector3.zero;
                joint.connectedAnchor = GrabPositionOffset;
            }
        }

        protected virtual void setupKinematicGrab(Grabber grabbedBy, GrabType grabType) {
            if (ParentToHands) {
                transform.parent = grabbedBy.transform;
            }

            if (rigid != null) {

                // При необходимости обновите режим обнаружения
                if (rigid.collisionDetectionMode == CollisionDetectionMode.Continuous || rigid.collisionDetectionMode == CollisionDetectionMode.ContinuousDynamic) {
                    rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }
                rigid.isKinematic = true;
            }
        }

        protected virtual void setupVelocityGrab(Grabber grabbedBy, GrabType grabType) {
            // Установите соединение, которое будет использоваться при движении со скоростью
            bool addJointToVelocityGrabbable = false;
            if(addJointToVelocityGrabbable) {
                if (GrabMechanic == GrabType.Precise) {
                    connectedJoint = grabbedBy.GetComponent<ConfigurableJoint>();
                    connectedJoint.connectedBody = rigid;
                    // Просто позвольте автоматической настройке выполнить вычисления за нас
                    connectedJoint.autoConfigureConnectedAnchor = true;
                }
                //Установите соединенный шарнир для защелкивания
                else if (GrabMechanic == GrabType.Snap) {
                    transform.rotation = grabTransform.rotation;
                    // Установочное соединение
                    setupConfigJoint(grabbedBy);
                    rigid.MoveRotation(grabTransform.rotation);
                }
            }

            // Отключите гравитацию, чтобы предотвратить физическую борьбу с ручным объектом
            rigid.useGravity = false;            
        }

        public virtual void GrabRemoteItem(Grabber grabbedBy) {
            flyingTo = grabbedBy;
            grabTransform.parent = grabbedBy.transform;
            grabTransform.localEulerAngles = Vector3.zero;
            grabTransform.localPosition = -GrabPositionOffset;

            grabTransform.localEulerAngles = GrabRotationOffset;            

            remoteGrabbing = true;
        }

        public virtual void ResetGrabbing() {
            if (rigid) {
                rigid.isKinematic = wasKinematic;
            }

            flyingTo = null;

            remoteGrabbing = false;

            collisions = new List<Collider>();
        }        

        public virtual void DropItem(Grabber droppedBy, bool resetVelocity, bool resetParent) {

            // Ничто не удерживает нас
            if (heldByGrabbers == null) {
                BeingHeld = false;
                return;
            }

            bool isPrimaryGrabber = droppedBy == GetPrimaryGrabber();
            bool isSecondaryGrabber = !isPrimaryGrabber && heldByGrabbers.Count > 1;

            if(isPrimaryGrabber) {

                // Следите за тем, держали ли нас двумя руками или нет, прежде чем уронить предмет
                bool wasHeldWithTwoHands = BeingHeldWithTwoHands;
                // Должны ли мы выпустить этот товар
                bool releaseItem = true;

                if (resetParent) {
                    ResetParent();
                }

                // отсоедините все соединения и установите для подключенного объекта значение null
                removeConfigJoint();

                // Снять Неподвижное Соединение
                if (GrabPhysics == GrabPhysics.FixedJoint && droppedBy != null) {
                    FixedJoint joint = droppedBy.gameObject.GetComponent<FixedJoint>();
                    if (joint) {
                        GameObject.Destroy(joint);
                    }
                }

                //  Если что-то вызывается drop для этого элемента, мы хотим убедиться, что родитель знает об этом
                // Reset's Grabber position, grabbable state, etc.
                if (droppedBy) {
                    droppedBy.DidDrop();
                }

                // No longer need move events
                UnsubscribeFromMoveEvents();

                // No longer have a primary Grab Offset set
                primaryGrabOffset = null;

                // No longer looking at a 2h object
                clearLookAtTransform();

                removeGrabber(droppedBy);

                didParentHands = false;

                // This object is being held by another grabber. Should we drop the item, transfer it over, or do nothing.
                if (wasHeldWithTwoHands) {

                    // Force Release
                    if(TwoHandedDropBehavior == TwoHandedDropMechanic.Drop) {
                        // Drop Secondary Object
                        if(SecondaryGrabbable != null && SecondaryGrabbable.BeingHeld) {
                            SecondaryGrabbable.DropItem(false, false);
                        }
                        else {
                            // Drop our own object
                            DropItem(heldByGrabbers[0]);                            
                        }
                    }
                    // Swap To other Hand Side
                    else if (TwoHandedDropBehavior == TwoHandedDropMechanic.Transfer) {

                        // We are going to transfer this item, so no need to release
                        releaseItem = false;

                        // Swap to new grabber
                        var newGrabber = heldByGrabbers[0];
                        Vector3 localHandsPos = Vector3.zero;
                        Vector3 localHandsRot = Vector3.zero;

                        if (newGrabber.HandsGraphics != null) {
                            Transform prev = newGrabber.HandsGraphics.parent;
                            newGrabber.HandsGraphics.parent = transform;
                            localHandsPos = newGrabber.HandsGraphics.localPosition;
                            localHandsRot = newGrabber.HandsGraphics.localEulerAngles;
                            newGrabber.HandsGraphics.parent = prev;
                        }

                        DropItem(newGrabber);
                        newGrabber.GrabGrabbable(this);

                        // Call Transfer Events
                        // OnTransferGrabber(Grabber from, Grabber to);

                        // Fix Hands position
                        if (newGrabber.HandsGraphics != null && ParentHandModel == true && GrabMechanic == GrabType.Precise) {
                            Transform prev = newGrabber.HandsGraphics.parent;
                            newGrabber.HandsGraphics.parent = transform;
                            newGrabber.HandsGraphics.localPosition = localHandsPos;
                            newGrabber.HandsGraphics.localEulerAngles = localHandsRot;
                            newGrabber.HandsGraphics.parent = prev;
                        }
                    }
                }
                // Release the object
                if(releaseItem) {

                    LastDropTime = Time.time;

                    // Release item and apply physics force to it
                    if (rigid != null && GrabPhysics != GrabPhysics.None) {
                        rigid.isKinematic = wasKinematic;
                        rigid.useGravity = usedGravity;
                        rigid.interpolation = initialInterpolationMode;
                        rigid.collisionDetectionMode = initialCollisionMode;
                    }

                    // Override Kinematic status if specified
                    if (ForceDisableKinematicOnDrop) {
                        rigid.isKinematic = false;
                        // Free of constraints if they were set
                        if (rigid.constraints == RigidbodyConstraints.FreezeAll) {
                            rigid.constraints = RigidbodyConstraints.None;
                        }
                    }

                    // On release event
                    /*
                    if (events != null) {
                        for (int x = 0; x < events.Count; x++) {
                            events[x].OnRelease();
                        }
                    }
                    */

                    // Reset hand pose
                    CustomHandPose = initialHandPoseId;
                    SelectedHandPose = initialHandPose;
                    handPoseType = initialHandPoseType;

                    // Apply velocity last
                    if (rigid && resetVelocity && droppedBy && AddControllerVelocityOnDrop&& GrabPhysics != GrabPhysics.None) {
                        // Make sure velocity is passed on
                        Vector3 velocity = droppedBy.GetGrabberAveragedVelocity() + droppedBy.GetComponent<Rigidbody>().velocity;
                        Vector3 angularVelocity = droppedBy.GetGrabberAveragedAngularVelocity() + droppedBy.GetComponent<Rigidbody>().angularVelocity;

                        if (gameObject.activeSelf) {
                            Release(velocity, angularVelocity);
                        }
                    }
                    
                    // On release event
                    if (events != null) {
                        for (int x = 0; x < events.Count; x++) {
                            events[x].OnRelease();
                        }
                    }
                    
                    /*
                    // On release event
                    if (events != null) {
                        for (int x = 0; x < events.Count; x++) {
                            events[x].OnReleaseCompleted();
                        }
                    }
                    */
                }
            }
            else if (isSecondaryGrabber) {
                //  If something called drop on this item we want to make sure the parent knows about it
                // Reset's Grabber position, grabbable state, etc.
                if (droppedBy) {
                    droppedBy.DidDrop();
                }

                removeGrabber(droppedBy);

                secondaryGrabOffset = null;

                // didParentHands = false;
            }

            BeingHeld = heldByGrabbers != null && heldByGrabbers.Count > 0;
        }

        void clearLookAtTransform() {
            if (SecondaryLookAtTransform != null && SecondaryLookAtTransform.transform.name == "LookAtTransformTemp") {
                GameObject.Destroy(SecondaryLookAtTransform.gameObject);
            }

            SecondaryLookAtTransform = null;
        }

        void callEvents(Grabber g) {
            if (events.Any()) {
                ControllerHand hand = g.HandSide;

                // Right Hand Controls
                if (hand == ControllerHand.Right) {
                    foreach (var e in events) {
                        e.OnGrip(input.RightGrip);
                        e.OnTrigger(input.RightTrigger);

                        if (input.RightTriggerUp) {
                            e.OnTriggerUp();
                        }
                        if (input.RightTriggerDown) {
                            e.OnTriggerDown();
                        }
                        if (input.AButton) {
                            e.OnButton1();
                        }
                        if (input.AButtonDown) {
                            e.OnButton1Down();
                        }
                        if (input.AButtonUp) {
                            e.OnButton1Up();
                        }
                        if (input.BButton) {
                            e.OnButton2();
                        }
                        if (input.BButtonDown) {
                            e.OnButton2Down();
                        }
                        if (input.BButtonUp) {
                            e.OnButton2Up();
                        }
                    }
                }

                // Left Hand Controls
                if (hand == ControllerHand.Left) {
                    for (int x = 0; x < events.Count; x++) {
                        GrabbableEvents e = events[x];
                        e.OnGrip(input.LeftGrip);
                        e.OnTrigger(input.LeftTrigger);

                        if (input.LeftTriggerUp) {
                            e.OnTriggerUp();
                        }
                        if (input.LeftTriggerDown) {
                            e.OnTriggerDown();
                        }
                        if (input.XButton) {
                            e.OnButton1();
                        }
                        if (input.XButtonDown) {
                            e.OnButton1Down();
                        }
                        if (input.XButtonUp) {
                            e.OnButton1Up();
                        }
                        if (input.YButton) {
                            e.OnButton2();
                        }
                        if (input.YButtonDown) {
                            e.OnButton2Down();
                        }
                        if (input.YButtonUp) {
                            e.OnButton2Up();
                        }
                    }
                }
            }
        }       

        public virtual void DropItem(Grabber droppedBy) {
            DropItem(droppedBy, true, true);
        }

        public virtual void DropItem(bool resetVelocity, bool resetParent) {
            DropItem(GetPrimaryGrabber(), resetVelocity, resetParent);
        }

        public void ResetScale() {
            transform.localScale = OriginalScale;
        }

        public void ResetParent() {
            transform.parent = originalParent;
        }

        public void UpdateOriginalParent(Transform newOriginalParent) {
            originalParent = newOriginalParent;
        }

        public void UpdateOriginalParent() {
            UpdateOriginalParent(transform.parent);
        }

        public ControllerHand GetControllerHand(Grabber g) {
            if(g != null) {
                return g.HandSide;
            }

            return ControllerHand.None;
        }

        /// <summary>
        /// Возвращает захватчик, который первым захватил этот элемент. Возвращает значение null, если оно не удерживается.
        /// </summary>
        /// <returns></returns>
        /// 
        public virtual Grabber GetPrimaryGrabber() {


            if(heldByGrabbers != null) {
                for (int x = 0; x < heldByGrabbers.Count; x++) {
                    if (heldByGrabbers[x] != null && heldByGrabbers[x].HeldGrabbable == this) {
                        return heldByGrabbers[x];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the closest valid grabber. 
        /// </summary>
        /// <returns>Returns null if no valid Grabbers in range</returns>
        public virtual Grabber GetClosestGrabber() {

            Grabber closestGrabber = null;
            float lastDistance = 9999;

            if (validGrabbers != null) {

                for (int x = 0; x < validGrabbers.Count; x++) {
                    Grabber g = validGrabbers[x];
                    if (g != null) {
                        float dist = Vector3.Distance(grabPosition, g.transform.position);
                        if(dist < lastDistance) {
                            closestGrabber = g;
                        }
                    }
                }
            }

            return closestGrabber;
        }

        public virtual Transform GetClosestGrabPoint(Grabber grabber) {
            Transform grabPoint = null;
            float lastDistance = 9999;
            float lastAngle = 360;
            if(GrabPoints != null) {
                int grabCount = GrabPoints.Count;
                for (int x = 0; x < grabCount; x++) {
                    Transform g = GrabPoints[x];

                    // Трансформация, возможно, была уничтожена
                    if (g == null) {
                        continue;
                    }

                    if (grabber == null)
                    {
                        continue;
                    }

                    float thisDist = Vector3.Distance(g.transform.position, grabber.transform.position);
                    if (thisDist <= lastDistance) {

                        // Проверьте наличие компонента Gradpoint, который может переопределять некоторые значения
                        GrabPoint gp = g.GetComponent<GrabPoint>();
                        if (gp) {

                            // Not valid for this hand side
                            if((grabber.HandSide == ControllerHand.Left && !gp.LeftHandIsValid) || (grabber.HandSide == ControllerHand.Right && !gp.RightHandIsValid)) {
                                continue;
                            }

                            // Angle is too great
                            float currentAngle = Quaternion.Angle(grabber.transform.rotation, g.transform.rotation);
                            if (currentAngle > gp.MaxDegreeDifferenceAllowed) {
                                continue;
                            }

                            // Last angle was better, don't use this one
                            if (currentAngle > lastAngle && gp.MaxDegreeDifferenceAllowed != 360) {
                                continue;
                            }

                            lastAngle = currentAngle;
                        }

                        grabPoint = g;
                        lastDistance = thisDist;
                    }
                }
            }

            return grabPoint;
        }

        /// <summary>
        /// Throw the object by applying velocity
        /// </summary>
        /// <param name="velocity">How much velocity to apply to the grabbable. Multiplied by ThrowForceMultiplier</param>
        /// <param name="angularVelocity">How much angular velocity to apply to the grabbable.</param>
        public virtual void Release(Vector3 velocity, Vector3 angularVelocity) {
            Vector3 releaseVelocity = velocity * ThrowForceMultiplier;
            Vector3 releaseAngularVelocity = velocity * ThrowForceMultiplierAngular;

            // Make sure this is a valid velocity
            if (float.IsInfinity(releaseVelocity.x) || float.IsNaN(releaseVelocity.x)) {
                return;
            }

            rigid.velocity = releaseVelocity;
            rigid.angularVelocity = releaseAngularVelocity;
            
            if (events != null) {
                for (int x = 0; x < events.Count; x++) {
                    events[x].OnApplyVelocity(releaseVelocity, angularVelocity);
                }
            }
        }

        public virtual bool IsValidCollision(Collision collision) {
            return IsValidCollision(collision.collider);
        }

        public virtual bool IsValidCollision(Collider col) {

            // Ignore Projectiles from grabbable collision
            // This way our grabbable stays rigid when projectils come in contact
            string transformName = col.transform.name;
            if (transformName.Contains("Projectile") || transformName.Contains("Bullet") || transformName.Contains("Clip")) {
                return false;
            }

            // Ignore Character Joints as these cause jittery issues
            if (transformName.Contains("Joint")) {
                return false;
            }

            // Ignore Character Controllers
            CharacterController cc = col.gameObject.GetComponent<CharacterController>();
            if (cc && col) {
                Physics.IgnoreCollision(col, cc, true);
                return false;
            }

            return true;
        }

        public virtual void parentHandGraphics(Grabber g) {
            if (g.HandsGraphics != null) {
                // Set to specified Grab Transform
                if (primaryGrabOffset != null) {
                    g.HandsGraphics.transform.parent = primaryGrabOffset;
                    didParentHands = true;
                }
                else {
                    g.HandsGraphics.transform.parent = transform;
                    didParentHands = true;
                }
            }
        }

        void setupConfigJoint(Grabber g) {
            connectedJoint = g.GetComponent<ConfigurableJoint>();
            connectedJoint.autoConfigureConnectedAnchor = false;
            connectedJoint.connectedBody = rigid;
            connectedJoint.anchor = Vector3.zero;
            connectedJoint.connectedAnchor = GrabPositionOffset;
        }

        void removeConfigJoint() {
            if (connectedJoint != null) {
                connectedJoint.anchor = Vector3.zero;
                connectedJoint.connectedBody = null;
            }
        }

        void addGrabber(Grabber g) {
            if (heldByGrabbers == null) {
                heldByGrabbers = new List<Grabber>();
            }

            if (!heldByGrabbers.Contains(g)) {
                heldByGrabbers.Add(g);
            }
        }

        void removeGrabber(Grabber g) {
            if (heldByGrabbers == null) {
                heldByGrabbers = new List<Grabber>();
            }
            else if (heldByGrabbers.Contains(g)) {
                heldByGrabbers.Remove(g);
            }

            Grabber removeGrabber = null;
            // Clean up any other latent grabbers
            for (int x = 0; x < heldByGrabbers.Count; x++) {
                Grabber grab = heldByGrabbers[x];
                if (grab.HeldGrabbable == null || grab.HeldGrabbable != this) {
                    removeGrabber = grab;
                }
            }

            if (removeGrabber) {
                heldByGrabbers.Remove(removeGrabber);
            }
        }

        /// <summary>
        /// Moves the Grabbable using MovePosition if rigidbody present. Otherwise use transform.position
        /// </summary>
        void movePosition(Vector3 worldPosition) {
            if (rigid) {
                rigid.MovePosition(worldPosition);
            }
            else {
                transform.position = worldPosition;
            }
        }

        /// <summary>
        /// Rotates the Grabbable using MoveRotation if rigidbody present. Otherwise use transform.rotation
        /// </summary>
        void moveRotation(Quaternion worldRotation) {
            if (rigid) {
                rigid.MoveRotation(worldRotation);
            }
            else {
                transform.rotation = worldRotation;
            }
        }

        protected Vector3 getRemotePosition(Grabber toGrabber) {

            return GetGrabberWithGrabPointOffset(toGrabber, GetClosestGrabPoint(toGrabber));

            //if (toGrabber != null) {
            //    Transform pointPosition = GetClosestGrabPoint(toGrabber);

            //    if(pointPosition) {
            //        Vector3 grabberPosition = toGrabber.transform.position;

            //        if (pointPosition != null) {
            //            grabberPosition += transform.position - pointPosition.position;
            //            //Vector3 offset = toGrabber.transform.InverseTransformPoint(pointPosition.position);
            //            //grabberPosition += offset;
            //        }

            //        return grabberPosition;
            //    }

            //    return grabTransform.position;
            //}

            //return grabTransform.position;
        }

        protected Quaternion getRemoteRotation(Grabber grabber) {

            if (grabber != null) {
                Transform point = GetClosestGrabPoint(grabber);
                if (point) {
                    Quaternion originalRot = grabTransform.rotation;
                    grabTransform.localRotation *= Quaternion.Inverse(point.localRotation);
                    Quaternion result = grabTransform.rotation;

                    grabTransform.rotation = originalRot;

                    return result;
                }
            }

            return grabTransform.rotation;
        }

        void filterCollisions() {
            for (int x = 0; x < collisions.Count; x++) {
                if (collisions[x] == null || !collisions[x].enabled || !collisions[x].gameObject.activeSelf) {
                    collisions.Remove(collisions[x]);
                    break;
                }
            }
        }

        /// <summary>
        /// A BNGPlayerController is optional, but if one is available we can check the last moved time in order to strengthen the physics joint during quick movements. This helps prevent jitter or flying objects in certain situations.
        /// </summary>
        /// <returns></returns>
        public virtual BNGPlayerController GetBNGPlayerController() {

            if (_player != null) {
                return _player;
            }

            // The player object can be used to determine if the object is about to move rapidly
            if (GameObject.FindGameObjectWithTag("Player")) {
                return _player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BNGPlayerController>();
            }
            else {
                return _player = FindObjectOfType<BNGPlayerController>();
            }
        }

        /// <summary>
        /// Request the Grabbable to use a springy joint for the next X seconds
        /// </summary>
        /// <param name="seconds">How many seconds to make the Grabbable springy.</param>
        public virtual void RequestSpringTime(float seconds) {
            float requested = Time.time + seconds;

            // Only apply if our request is longer than the current request
            if(requested > requestSpringTime) {
                requestSpringTime = requested;
            }
        }

        public virtual void AddValidGrabber(Grabber grabber) {

            if (validGrabbers == null) {
                validGrabbers = new List<Grabber>();
            }

            if (!validGrabbers.Contains(grabber)) {
                validGrabbers.Add(grabber);
            }
        }

        public virtual void RemoveValidGrabber(Grabber grabber) {
            if (validGrabbers != null && validGrabbers.Contains(grabber)) {
                validGrabbers.Remove(grabber);
            }
        }

        bool subscribedToEvents = false;
        bool grabbableIsLocked = false;

        /// <summary>
        /// Subscribe to any movement-related events that might cause our Grabbable to suddenly move far away.
        /// By subscribing to these events before they occur we can then respond better to these positional updates
        /// </summary>
        public virtual void SubscribeToMoveEvents() {

            // Object can't be moved, so no need for subscription
            if(!CanBeMoved || subscribedToEvents == true || GrabPhysics == GrabPhysics.None) {
                return;
            }

            // Lock the slide in place when teleporting or snap turning
            PlayerTeleport.OnBeforeTeleport += LockGrabbableWithRotation;
            PlayerTeleport.OnAfterTeleport += UnlockGrabbable;

            PlayerRotation.OnBeforeRotate += LockGrabbableWithRotation;
            PlayerRotation.OnAfterRotate += UnlockGrabbable;

            // Only needed for velocity and physics type movement
            if(GrabPhysics == GrabPhysics.Velocity || GrabPhysics == GrabPhysics.PhysicsJoint) {
                SmoothLocomotion.OnBeforeMove += LockGrabbable;
                SmoothLocomotion.OnAfterMove += UnlockGrabbable;
            }

            // Kinematic can use parenting
            if (GrabPhysics == GrabPhysics.Kinematic && ParentToHands == true) {
                SmoothLocomotion.OnBeforeMove += LockGrabbableWithRotation;
                SmoothLocomotion.OnAfterMove += UnlockGrabbable;
            }
            else if (GrabPhysics == GrabPhysics.Kinematic && ParentToHands == false) {
                SmoothLocomotion.OnBeforeMove += LockGrabbable;
                SmoothLocomotion.OnAfterMove += UnlockGrabbable;
            }

            subscribedToEvents = true;
        }

        public virtual void UnsubscribeFromMoveEvents() {
            if(subscribedToEvents) {
                PlayerTeleport.OnBeforeTeleport -= LockGrabbableWithRotation;
                PlayerTeleport.OnAfterTeleport -= UnlockGrabbable;

                PlayerRotation.OnBeforeRotate -= LockGrabbableWithRotation;
                PlayerRotation.OnAfterRotate -= UnlockGrabbable;

                // Specific lock types
                if (GrabPhysics == GrabPhysics.Velocity || GrabPhysics == GrabPhysics.PhysicsJoint) {
                    SmoothLocomotion.OnBeforeMove -= LockGrabbable;
                    SmoothLocomotion.OnAfterMove -= UnlockGrabbable;
                }

                // Kinematic can use parenting
                if (GrabPhysics == GrabPhysics.Kinematic && ParentToHands == true) {
                    SmoothLocomotion.OnBeforeMove -= LockGrabbableWithRotation;
                    SmoothLocomotion.OnAfterMove -= UnlockGrabbable;
                }
                else if (GrabPhysics == GrabPhysics.Kinematic && ParentToHands == false) {
                    SmoothLocomotion.OnBeforeMove -= LockGrabbable;
                    SmoothLocomotion.OnAfterMove -= UnlockGrabbable;
                }

                // Reset Lock Events
                lockRequests = 0;

                subscribedToEvents = false;
            }
        }

        private Transform _priorParent;

        private Vector3 _priorLocalOffsetPosition;
        private Quaternion _priorLocalOffsetRotation;

        private Grabber _priorPrimaryGrabber;
        bool lockPos, lockRot;
        int lockRequests = 0;

        public virtual void LockGrabbable() {
            // By default only lock position
            LockGrabbable(true, false, false);
        }

        // Lock both position and rotation
        public virtual void LockGrabbableWithRotation() {
            LockGrabbable(true, true, true);
        }

        public virtual void RequestLockGrabbable() {

            // Don't do anything if recent collision
            if(RecentlyCollided) {
                return;
            }

            lockRequests++;

            if (lockRequests == 1) {
                if (_priorPrimaryGrabber != null) {
                    // Lock via parenting
                    // Store position as well as parenting
                    _priorParent = transform.parent;
                    transform.parent = _priorPrimaryGrabber.transform;
                }
            }

            if (lockRequests > 0) {
                if (_priorPrimaryGrabber != null) {

                    _priorParent = transform.parent;
                    transform.parent = _priorPrimaryGrabber.transform;

                    // Store latest position offset
                    _priorLocalOffsetPosition = _priorPrimaryGrabber.transform.InverseTransformPoint(transform.position);
                }
            }
        }

        public virtual void RequestUnlockGrabbable() {

            // Don't do anything if recent collision
            if (RecentlyCollided) {
                return;
            }

            ResetLockResets();
        }

        public virtual void ResetLockResets() {
            if (lockRequests > 0) {

                if (transform.parent != _priorParent) {
                    transform.parent = _priorParent;
                }

                lockRequests = 0;
            }
        }

        /// <summary>
        /// Keep the Grabbable's position and /or rotation in place
        /// </summary>
        public virtual void LockGrabbable(bool lockPosition, bool lockRotation, bool overridePriorLock) {

            if (BeingHeld && (!grabbableIsLocked || overridePriorLock)) {

                if (_priorPrimaryGrabber != null) {

                    lockPos = lockPosition;
                    lockRot = lockRotation;

                    // Lock via parenting
                    if (lockPosition && lockRotation) {
                        // Store position as well as parenting
                        _priorLocalOffsetPosition = _priorPrimaryGrabber.transform.InverseTransformPoint(transform.position);

                        _priorParent = transform.parent;
                        transform.parent = _priorPrimaryGrabber.transform;
                    }
                   //  Individual locking
                    else {
                        if (lockPos) {
                            _priorLocalOffsetPosition = _priorPrimaryGrabber.transform.InverseTransformPoint(transform.position);
                        }

                        if (lockRot) {
                            _priorLocalOffsetRotation = Quaternion.FromToRotation(transform.forward, _priorPrimaryGrabber.transform.forward);
                        }
                    }

                    grabbableIsLocked = true;
                }
            }
        }

        /// <summary>
        /// Allow the Grabbable to move
        /// </summary>
        public virtual void UnlockGrabbable() {
            if (BeingHeld && grabbableIsLocked) {
                // Use parenting if both position and rotation are to be locked
                if(lockPos && lockRot) {
                    Vector3 dest = _priorPrimaryGrabber.transform.TransformPoint(_priorLocalOffsetPosition);
                    float dist = Vector3.Distance(transform.position, dest);
                    // Only move if gone far enough
                    if (dist > 0.001f) {
                        transform.position = _priorPrimaryGrabber.transform.TransformPoint(_priorLocalOffsetPosition);
                    }

                    // Only reparent if necessary
                    if(transform.parent != _priorParent) {
                        transform.parent = _priorParent;
                    }
                }
                else {
                    if (lockPos) {
                        Vector3 dest = _priorPrimaryGrabber.transform.TransformPoint(_priorLocalOffsetPosition);
                        float dist = Vector3.Distance(transform.position, dest);
                        // Only move if gone far enough
                        if (dist > 0.0005f) {
                            transform.position = dest;
                        }
                    }

                    if (lockRot) {
                        transform.rotation = _priorPrimaryGrabber.transform.rotation * _priorLocalOffsetRotation;
                    }
                }

                grabbableIsLocked = false;
            }
        }

        /// <summary>
        /// You can comment this function out if you don't need precise contacts. Otherwise this is necessary to check for world collisions while being held
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay(Collision collision) {

            // Can bail early
            if (!BeingHeld) {
                return;
            }

            for (int x = 0; x < collision.contacts.Length; x++) {
                ContactPoint contact = collision.contacts[x];
                // Keep track of how many objects we are colliding with
                if (BeingHeld && IsValidCollision(contact.otherCollider) && !collisions.Contains(contact.otherCollider)) {
                    collisions.Add(contact.otherCollider);
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {
            // Keep track of how many objects we are colliding with
            if (BeingHeld && IsValidCollision(collision) && !collisions.Contains(collision.collider)) {
                collisions.Add(collision.collider);
            }
        }

        private void OnCollisionExit(Collision collision) {
            // We only care about collisions when being held, so we can skip this check otherwise
            if (BeingHeld && collisions.Contains(collision.collider)) {
                collisions.Remove(collision.collider);
            }
        }

        bool quitting = false;
        void OnApplicationQuit() {
            quitting = true;
        }

        void OnDestroy() {
            if(BeingHeld && !quitting) {
                DropItem(false, false);
            }
        }




        void OnDrawGizmosSelected() {
            // Show Grip Points
            Gizmos.color = new Color(0, 1, 0, 0.5f);

            if (GrabPoints != null && GrabPoints.Count > 0) {
                for (int i = 0; i < GrabPoints.Count; i++) {
                    Transform p = GrabPoints[i];
                    if (p != null) {
                        Gizmos.DrawSphere(p.position, 0.02f);
                    }
                }
            }
            else {
                Gizmos.DrawSphere(transform.position, 0.02f);
            }
        }     
    }

    #region enums
    public enum GrabType {
        Snap,
        Precise
    }

    public enum RemoteGrabMovement {
        Linear,
        Velocity,
        Flick
    }

    public enum GrabPhysics {
        None = 2,
        PhysicsJoint = 0,
        FixedJoint = 3,
        Velocity = 4,
        Kinematic = 1
    }

    public enum OtherGrabBehavior {
        None,
        SwapHands,
        DualGrab
    }

    public enum TwoHandedPositionType {
        Lerp,
        None
    }

    public enum TwoHandedRotationType {
        Lerp,
        Slerp,
        LookAtSecondary,
        None
    }

    public enum TwoHandedDropMechanic {
        Drop,
        Transfer,
        None
    }

    public enum TwoHandedLookDirection {
        Horizontal,
        Vertical        
    }

    public enum HandPoseType {
        AnimatorID,
        HandPose,
        AutoPoseOnce,
        AutoPoseContinuous,
        None
    }

    #endregion
}
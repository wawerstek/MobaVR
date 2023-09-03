using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    /// <summary>
    /// Базовый класс для большого и маленького шаров
    /// Обработка всех столкновений пока в это классе.
    /// TODO: обрабатывать OnTriggerEnter в отдельных сущностях
    ///
    /// На игроках и щите должен висеть IsTrigger, чтобы прошла проверка
    /// </summary>
    [RequireComponent(typeof(Throwable))]
    public abstract class ThrowableSpell : Spell
        //, IThrowable
    {
        [Space]
        [Header("Spell")]
        [SerializeField] protected float m_DefaultDamage = 1f;
        [SerializeField] protected float m_Force = 4000f;

        [SerializeField] protected Throwable m_Throwable;

        protected bool m_IsThrown = false;

        public Throwable Throwable => m_Throwable;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_Throwable == null)
            {
                TryGetComponent(out m_Throwable);
            }
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
            //Use RpcDestroy for Network
            //Destroy(gameObject, m_DestroyLifeTime);
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                //PhotonNetwork.Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            //if (!collision.transform.CompareTag("Player"))
            //if (m_IsThrown)
            if (!photonView.IsMine)
            {
                //return;
            }

            if (m_IsThrown && !collision.transform.CompareTag("RemotePlayer"))
            {
                HandleCollision(collision.transform);
            }
        }

        //TODO
        protected void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                //return;
            }

            if (other.transform.TryGetComponent(out PhotonView colliderPhoton))
            {
                if (colliderPhoton == photonView)
                {
                    return;
                }
            }

            if (m_IsThrown)
            {
                if (other.transform.TryGetComponent(out Damageable damageable))
                {
                    HitData hitData = new HitData()
                    {
                        Amount = CalculateDamage(),
                        Player = PhotonNetwork.LocalPlayer,
                        PhotonOwner = photonView,
                        PhotonView = photonView,
                        PlayerVR = Owner.PlayerVR,
                        TeamType = TeamType
                    };

                    if (photonView.IsMine)
                    {
                        damageable.Hit(hitData);
                    }

                    HandleCollision(other.transform);
                }

                if (other.CompareTag("Item") && other.TryGetComponent(out BigShield bigShield))
                {
                    if (bigShield.Team.TeamType != m_TeamType)
                    {
                        HandleCollision(other.transform);
                    }
                }
            }
        }

        /*
        public virtual void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            m_Owner = wizardPlayer;
            m_TeamType = teamType;
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }
        */

        /*
        public virtual void Init(TeamType teamType)
        {
            //m_TeamItem.SetTeam(teamType);
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }
        */

        /*
        [PunRPC]
        public virtual void RpcInit(TeamType teamType)
        {
            m_TeamItem.SetTeam(teamType);
        }
        */

        protected abstract float CalculateDamage();

        protected abstract void HandleCollision(Transform interactable);
        //public abstract void Throw();
        //public abstract void ThrowByDirection(Vector3 direction);
    }
}
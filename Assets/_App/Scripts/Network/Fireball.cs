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
    public abstract class Fireball : MonoBehaviourPunCallbacks
    {
        [Space]
        [Header("Components")]
        [SerializeField] protected TeamItem m_TeamItem;
        [SerializeField] protected float m_DefaultDamage = 1f;
        [SerializeField] protected float m_Force = 4000f;
        [SerializeField] protected float m_DestroyLifeTime = 20.0f;

        protected bool m_IsThrown = false;
        protected WizardPlayer m_Owner;

        public TeamItem Team => m_TeamItem;
        public WizardPlayer Owner
        {
            get => m_Owner;
            set => m_Owner = value;
        }

        protected virtual void OnValidate()
        {
            if (m_TeamItem == null)
            {
                TryGetComponent(out m_TeamItem);
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
            Destroy(gameObject, m_DestroyLifeTime);
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
                return;
            }
            
            if (m_IsThrown && !collision.transform.CompareTag("RemotePlayer"))
            {
                InteractBall(collision.transform);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
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
                if (other.CompareTag("RemotePlayer") && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
                {
                    if (wizardPlayer == Owner)
                    {
                        return;
                    }
                    
                    wizardPlayer.Hit(this, CalculateDamage());
                }

                if (other.CompareTag("Item"))
                {
                    Shield shield = other.GetComponentInParent<Shield>();
                    if (shield != null)
                    {
                        shield.Hit(this, CalculateDamage());
                    }
                }
                
                InteractBall(other.transform);
            }
        }

        public void Init(TeamType teamType)
        {
            //m_TeamItem.SetTeam(teamType);
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }

        [PunRPC]
        public virtual void RpcInit(TeamType teamType)
        {
            m_TeamItem.SetTeam(teamType);
        }

        protected abstract float CalculateDamage();
        protected abstract void InteractBall(Transform interactable);
        public abstract void Throw();
        public abstract void ThrowByDirection(Vector3 direction);
    }
}
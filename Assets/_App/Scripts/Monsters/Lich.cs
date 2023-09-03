using System;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobaVR
{
    public class Lich : MonoBehaviourPunCallbacks, IExploding
    {
        #region Dependencies

        [Header("Dependencies")]
        //[SerializeField] private LichGame m_LichGame;
        [SerializeField] private MonsterView m_MonsterView;
        [SerializeField] private DamageNumberView m_DamageNumber;

        #endregion

        #region Components

        [SerializeField] private Transform m_Root;
        [SerializeField] private Collider m_BodyCollider;
        [SerializeField] private Animator m_Animator;

        #endregion

        #region Points

        [Space]
        [Header("Stats")]
        [SerializeField] private float m_Health = 2000f;
        [SerializeField] private float m_Damage = 20f;
        [SerializeField] [ReadOnly] private float m_CurrentHealth = 2000f;
        [SerializeField] private float m_DestroyTime = 5f;

        #endregion

        #region Spells

        [SerializeField] private bool m_CanCast = true;
        [SerializeField] private float m_Cooldown = 20f;
        [SerializeField] private List<LichSpell> m_LichSpells = new();
        [SerializeField] private List<Transform> m_SpellPoints = new();

        #endregion

        #region Animations

        private readonly int m_HashActivate = Animator.StringToHash("Activate");
        private readonly int m_HashIdle = Animator.StringToHash("Idle");
        private readonly int m_HashTaunt = Animator.StringToHash("Taunt");
        private readonly int m_HashAttack = Animator.StringToHash("Attack");
        private readonly int m_HashDeath = Animator.StringToHash("Die");

        #endregion

        #region Fields

        private bool m_IsImmortal = false;
        private bool m_CanGetDamage = true;
        private bool m_IsActive = false;

        private Renderer[] m_MeshRenderers;

        #endregion

        #region Properties

        public float Damage => m_Damage;
        public float CurrentHealth => m_CurrentHealth;
        public bool IsLife => m_CurrentHealth > 0f;

        #endregion

        #region Actions

        public Action OnInit;
        public Action OnMove;
        public Action OnAttack;
        public Action OnDeath;

        #endregion

        private void OnValidate()
        {
            if (m_Animator == null)
            {
                TryGetComponent(out m_Animator);
            }
        }

        private void Awake()
        {
            m_MeshRenderers = GetComponentsInChildren<Renderer>();
            RpcDeactivate_Monster();
            //Init();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BigFireBall fireBall))
            {
                //if (IsLife && !m_IsActive)
                if (!m_IsActive)
                {
                    //m_LichGame.StartGame();
                }
            }
        }

        #region Init

        public void Init()
        {
            photonView.RPC(nameof(RpcInit_Monster), RpcTarget.All);
        }

        [PunRPC]
        public void RpcInit_Monster()
        {
            m_CurrentHealth = m_Health;
            m_CanGetDamage = false;
            m_IsImmortal = true;

            m_BodyCollider.enabled = false;
            m_MonsterView.SetEnabled(false);

            Appear();
        }

        public void Activate()
        {
            photonView.RPC(nameof(RpcActivate_Monster), RpcTarget.All);
        }

        [PunRPC]
        public void RpcActivate_Monster()
        {
            m_CurrentHealth = m_Health;
            UpdateHealth();

            m_CanGetDamage = true;
            m_IsImmortal = false;

            m_BodyCollider.enabled = true;
            m_MonsterView.SetEnabled(true);

            m_IsActive = true;

            OnInit?.Invoke();

            //ReadyToCast();
        }

        public void Pause()
        {
            photonView.RPC(nameof(RpcPause_Monster), RpcTarget.All);
        }

        [PunRPC]
        public void RpcPause_Monster()
        {
            m_CanGetDamage = false;
            m_IsImmortal = true;

            CancelInvoke(nameof(Cast));
        }

        public void Release()
        {
            photonView.RPC(nameof(RpcRelease_Monster), RpcTarget.All);
        }

        [PunRPC]
        public void RpcRelease_Monster()
        {
            m_CanGetDamage = true;
            m_IsImmortal = false;

            ReadyToCast();
        }

        private void ReadyToCast()
        {
            if (PhotonNetwork.IsMasterClient && m_CanCast && IsLife)
            {
                CancelInvoke(nameof(Cast));
                Invoke(nameof(Cast), m_Cooldown);
            }
        }

        public void Cast()
        {
            if (PhotonNetwork.IsMasterClient && m_SpellPoints.Count > 0 && IsLife)
            {
                int pointPosition = Random.Range(0, m_SpellPoints.Count);
                photonView.RPC(nameof(RpcCast_Monster), RpcTarget.All, pointPosition);
                
                ReadyToCast();
            }
        }

        [PunRPC]
        public void RpcCast_Monster(int pointPosition)
        {
            Transform pointTransform = m_SpellPoints[pointPosition];
            if (m_LichSpells.Count == 0)
            {
                return;
            }

            LichSpell lichSpell = m_LichSpells[0];
            lichSpell.gameObject.SetActive(true);
            lichSpell.transform.position = pointTransform.position;
            //lichSpell.Activate();
            lichSpell.RpcActivate();
        }

        public void Deactivate()
        {
            if (!gameObject.activeSelf || photonView.ViewID <= 0)
            {
                return;
            }

            photonView.RPC(nameof(RpcDeactivate_Monster), RpcTarget.All);
        }

        [PunRPC]
        public void RpcDeactivate_Monster()
        {
            CancelInvoke(nameof(Cast));
            
            m_IsActive = false;

            m_CurrentHealth = m_Health;
            m_CanGetDamage = false;
            m_IsImmortal = true;

            m_BodyCollider.enabled = true;
            m_MonsterView.SetEnabled(true);
        }

        #endregion

        #region Hit

        public void Hit(HitData hitData)
        {
            RpcRpcHit_Monster(hitData.Amount);
        }

        /*
        public void RpcHit(float damage)
        {
            photonView.RPC(nameof(RpcRpcHit), RpcTarget.All, damage);
        }
        */

        [PunRPC]
        private void RpcRpcHit_Monster(float damage)
        {
            if (IsLife)
            {
                Vector3 position = m_DamageNumber.transform.position;
                GetDamage(position, damage);
            }
        }

        public void Die()
        {
            photonView.RPC(nameof(RpcDie_Monster), RpcTarget.All);
        }

        [PunRPC]
        private void RpcDie_Monster()
        {
            Die(true);
        }

        public void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
            if (!IsLife)
            {
                //
            }
        }

        private void GetDamage(Vector3 position, float damage)
        {
            if (!IsLife)
            {
                return;
            }

            float scaleNumber = 5f;

            if (m_CanGetDamage)
            {
                if (m_CurrentHealth >= 0)
                {
                    m_CurrentHealth -= damage;
                    m_DamageNumber.SpawnNumber(position, damage, Monster.MonsterDamageType.HP, scaleNumber);
                    //m_OrcSoundController.PlayHit();
                    //CreateBlood(position);

                    if (m_CurrentHealth <= 0)
                    {
                        m_CurrentHealth = 0;
                        Die();
                    }

                    UpdateHealth();
                }
            }
            else
            {
                m_DamageNumber.SpawnNumber(position, damage, Monster.MonsterDamageType.IMMORTAL, scaleNumber);
            }
        }

        private void Die(bool isGiveTreasure)
        {
            m_CurrentHealth = 0f;
            m_MonsterView.SetEnabled(false);
            m_BodyCollider.enabled = false;
            OnDeath?.Invoke();

            m_IsActive = false;

            foreach (Collider childCollider in transform.GetComponentsInChildren<Collider>())
            {
                //childCollider.enabled = false;
            }

            Dissolve();

            //m_LichGame.SetGameOver();
        }

        private void UpdateHealth()
        {
            float healthBarValue = m_CurrentHealth * 100 / m_Health;
            m_MonsterView.UpdateHealth(healthBarValue);
        }

        private void Dissolve()
        {
            float clip = 0f;
            DOTween
                .To(() => clip, x => clip = x, 1f, m_DestroyTime)
                .OnUpdate(() =>
                {
                    foreach (Renderer meshRenderer in m_MeshRenderers)
                    {
                        Dissolve(meshRenderer.material, clip);
                    }
                })
                .OnComplete(() =>
                {
                    //Destroy(gameObject, 2f);
                });
        }

        private void Appear()
        {
            float clip = 1f;
            foreach (Renderer meshRenderer in m_MeshRenderers)
            {
                Dissolve(meshRenderer.material, 1f);
            }

            DOTween
                .To(() => clip, x => clip = x, 0f, m_DestroyTime)
                .OnUpdate(() =>
                {
                    foreach (Renderer meshRenderer in m_MeshRenderers)
                    {
                        Dissolve(meshRenderer.material, clip);
                    }
                })
                .OnComplete(() => { Activate(); });
        }

        private void Dissolve(Material material, float clip)
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(
                material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        }

        #endregion
    }
}
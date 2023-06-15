using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobaVR
{
    public class MonsterPointSpawner : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public List<Monster> MonsterControllers = new List<Monster>();
        [SerializeField] private List<Monster> m_MonsterPrefabs = new List<Monster>();
        [SerializeField] private List<string> m_MonsterPaths = new List<string>();
        [SerializeField] private ParticleSystem m_CreatedEffect;

        public int MaxCountMonster = -1;
        public float DelayBetweenMonster = 10f;
        public float DelayBetweenMaxMonster = 4f;
        public bool CanSpawn = false;

        private void Start()
        {
            //RpcGenerateMonsters();
        }

        public void GenerateMonsters()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            photonView.RPC(nameof(RpcGenerateMonsters), RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void RpcGenerateMonsters()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (PhotonNetwork.IsMasterClient)
            {
                //ClearMonsters();

                CanSpawn = true;
                StartCoroutine(GenerateMonsterWave(0f));
            }
        }

        //public void ClearMonsters(bool isDie = false)
        public void ClearMonsters(bool isDie = true)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            photonView.RPC(nameof(RpcClearMonsters), RpcTarget.AllBuffered, isDie);
        }

        [PunRPC]
        private void RpcClearMonsters(bool isDie)
        {
            CanSpawn = false;

            StopAllCoroutines();

            //Monster[] monsters = GetComponentsInChildren<Monster>();
            //foreach (Monster monster in monsters)

            while (MonsterControllers.Count != 0)
            {
                Monster monster = MonsterControllers[0];
                if (isDie)
                {
                    monster.Die();
                }
                else
                {
                    Destroy(monster.gameObject);
                }
                
                if (monster != null)
                {
                    MonsterControllers.Remove(monster);
                }
            }
            
            /*
            foreach (Monster monster in MonsterControllers)
            {
                if (isDie)
                {
                    monster.Die();
                }
                else
                {
                    Destroy(monster.gameObject);
                }
            }
            */

            MonsterControllers.Clear();
        }

        public bool TryGenerateMonster()
        {
            if (MaxCountMonster == -1 || MonsterControllers.Count < MaxCountMonster)
            {
                SpawnMonster();
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator GenerateMonsterWave(float delayBetweenMonster)
        {
            yield return new WaitForSeconds(delayBetweenMonster);
            if (!CanSpawn)
            {
                yield break;
            }

            if (TryGenerateMonster())
            {
                StartCoroutine(GenerateMonsterWave(DelayBetweenMonster));
            }
            else
            {
                StartCoroutine(GenerateMonsterWave(DelayBetweenMaxMonster));
            }
        }

        private void SpawnMonster()
        {
            if (CanSpawn)
            {
                //photonView.RPC(nameof(RpcSpawnMonster), RpcTarget.AllBuffered);
                photonView.RPC(nameof(RpcSpawnMonster), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RpcSpawnMonster()
        {
            if (!CanSpawn)
            {
                return;
            }
            
            if (!gameObject.activeSelf)
            {
                return;
            }

            int randomMonster = Random.Range(0, m_MonsterPrefabs.Count);
            //Monster monsterController = Instantiate(m_MonsterPrefabs[randomMonster], transform) as Monster;
            GameObject monsterGameObject =
                PhotonNetwork.Instantiate($"Monsters/{m_MonsterPrefabs[randomMonster].name}",
                                          transform.position,
                                          Quaternion.Euler(0,180,0));
            Monster monster = monsterGameObject.GetComponent<Monster>();
            //monster.transform.localPosition = Vector3.zero;
            monster.OnInit = () => { monster.Activate(); };
            monster.OnDeath = () =>
            {
                MonsterControllers.Remove(monster);
                //GenerateMonster();

                /*
                if (m_WorldController.MonsterScore >= 10 && m_WorldController.MonsterScore < 20)
                {
                    WaveLimitMonster = 3;
                    DelayBetweenMonster = 5f;
                }

                if (m_WorldController.MonsterScore >= 20 && m_WorldController.MonsterScore < 30)
                {
                    WaveLimitMonster = 4;
                    DelayBetweenMonster = 5f;
                }

                if (m_WorldController.MonsterScore >= 30 && m_WorldController.MonsterScore < 50)
                {
                    WaveLimitMonster = 5;
                    DelayBetweenMonster = 4f;
                }

                if (m_WorldController.MonsterScore >= 50)
                {
                    WaveLimitMonster = 6;
                    DelayBetweenMonster = 4f;
                }
                */
            };

            MonsterControllers.Add(monster);
            m_CreatedEffect.Play();
        }
    }
}
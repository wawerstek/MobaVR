using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class DistancPersPlayer : MonoBehaviour
{
    public float avoidDistance = 1.0f; // ����������, ��� ������� �������� ������ �������� ������� ������
    public GameObject[] players; //��� ������
    private NavMeshAgent agent;
    public float distance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ���� ������ � ����� "RemotePlayer"
        players = GameObject.FindGameObjectsWithTag("LifeCollider");

        foreach (GameObject player in players)
        {
            
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= avoidDistance)
            {
                Vector3 avoidDirection = (transform.position - player.transform.position).normalized;
                Vector3 newDestination = transform.position + avoidDirection * avoidDistance*2;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(newDestination, out hit, avoidDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
        }
        
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoMiniDragon : MonoBehaviour
{
    public Transform[] characterMovePoints; // Массив точек, к которым персонаж должен перемещаться
    public Transform[] objectMovePoints;    // Массив точек, куда будут перемещаться объекты
    private NavMeshAgent agent;
    private int currentCharacterMovePointIndex = 0;
    private int currentObjectMovePointIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MoveToNextPoint());
    }

    private IEnumerator MoveToNextPoint()
    {
        while (true)
        {
            // Подождем 20 секунд перед перемещением к следующей точке для персонажа
            yield return new WaitForSeconds(20f);

            // Если достигли конца массива точек для персонажа, начнем сначала
            if (currentCharacterMovePointIndex >= characterMovePoints.Length)
            {
                currentCharacterMovePointIndex = 0;
            }

            // Установим следующую точку назначения для персонажа и включим NavMeshAgent
            agent.SetDestination(characterMovePoints[currentCharacterMovePointIndex].position);
            agent.isStopped = false;

            // Подождем, пока персонаж дойдет до точки
            while (!agent.isStopped)
            {
                yield return null;
            }

            // Выключим NavMeshAgent для персонажа
            agent.isStopped = true;
            currentCharacterMovePointIndex++;

            // Подождем 20 секунд перед перемещением к следующей точке для объекта
            yield return new WaitForSeconds(20f);

            // Если достигли конца массива точек для объекта, начнем сначала
            if (currentObjectMovePointIndex >= objectMovePoints.Length)
            {
                currentObjectMovePointIndex = 0;
            }

            // Переместим объекты к следующей точке
            foreach (var objTransform in objectMovePoints)
            {
                objTransform.position = objectMovePoints[currentObjectMovePointIndex].position;
            }

            currentObjectMovePointIndex++;
        }
    }
}

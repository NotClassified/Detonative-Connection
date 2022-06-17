using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;

    Coroutine moveRoutine;
    [SerializeField] Transform[] wayPoints;
    int wayPointIndex = 0;
    int wayPointNextIndex = 1;
    [SerializeField] float waitTimeAtPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] float visionDistance;
    [SerializeField] float visionAngel;

    private void OnDrawGizmos()
    {
        Transform lastWayPoint = wayPoints[0];
        foreach (Transform wayPoint in wayPoints)
        {
            Gizmos.DrawSphere(wayPoint.position, .2f);
            Gizmos.DrawLine(lastWayPoint.position, wayPoint.position);
            lastWayPoint = wayPoint;
        }
        Gizmos.DrawLine(lastWayPoint.position, wayPoints[0].position);
    }

    private IEnumerator Start()
    {
        while (GameManager.player == null)
            yield return null;
        player = GameManager.player.transform;

        //moveRoutine = StartCoroutine(Movement());
    }

    private void Update()
    {
        if (!GameManager.gameOver && SpotPlayer())
        {
            print("spotted");
            GameManager.gm.SpottedGameOver();
        }
    }

    IEnumerator Movement()
    {
        while (true)
        {
            float time = 0;
            Vector3 currentPoint = wayPoints[wayPointIndex].position;
            Vector3 nextPoint = wayPoints[wayPointNextIndex].position;
            Vector3 targetAngle = nextPoint - currentPoint;
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(targetAngle, Vector3.up);
            while (time < 1)
            {
                time += (Time.deltaTime * turnSpeed);
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, time);
                yield return null;
            }
            transform.rotation = targetRotation;

            time = 0;
            float distance = Vector3.Distance(currentPoint, nextPoint);
            while (time < 1)
            {
                time += (Time.deltaTime * moveSpeed) / distance;
                transform.position = Vector3.Lerp(currentPoint, nextPoint, time);
                yield return null;
            }
            transform.position = wayPoints[wayPointNextIndex].position;

            if (wayPointIndex + 1 < wayPoints.Length)
                wayPointIndex++;
            else
                wayPointIndex = 0;
            if (wayPointNextIndex + 1 < wayPoints.Length)
                wayPointNextIndex++;
            else
                wayPointNextIndex = 0;

            yield return new WaitForSeconds(waitTimeAtPoint);
        }
    }

    bool SpotPlayer()
    {
        if(player != null && Vector3.Distance(transform.position, player.position) < visionDistance)
        {
            Vector3 dirPlayer = (player.position - transform.position).normalized;
            float anglePlayer = Vector3.Angle(transform.forward, dirPlayer);
            if(anglePlayer < visionAngel)
            {
                if (!Physics.Linecast(transform.position + Vector3.up * .5f, player.position, 0))
                {
                    player.GetComponent<PlayerMovement>().LookAtEnemy(transform);
                    if (moveRoutine != null)
                        StopCoroutine(moveRoutine);
                    return true;
                }
            }
        }
        return false;
    }
}

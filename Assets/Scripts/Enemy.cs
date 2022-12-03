using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] Animator animator;

    Coroutine moveRoutine;
    [SerializeField] Transform[] wayPoints;
    int wayPointIndex = 0;
    int wayPointNextIndex = 1;
    [SerializeField] float waitTimeAtPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] Transform visionObject;
    [SerializeField] float visionDistance;
    [SerializeField] float visionAngel;
    RaycastHit visionHit;

    private void OnDrawGizmos()
    {
        if(wayPoints.Length > 0)
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
    }

    private IEnumerator Start()
    {
        while (GameManager.player == null)
            yield return null;
        playerPosition = GameManager.player.GetComponent<PlayerMovement>().visionObject;

        if(wayPoints.Length > 0)
            moveRoutine = StartCoroutine(Movement());
    }

    private void Update()
    {
        if (!GameManager.gm.gameOver && SpotPlayer())
        {
            GameManager.gm.SpottedGameOver();
        }
    }

    IEnumerator Movement()
    {
        while (true)
        {
            animator.SetBool("Walking", true);

            float time = 0;
            Vector3 currentPoint = wayPoints[wayPointIndex].position;
            Vector3 nextPoint = wayPoints[wayPointNextIndex].position;
            Vector3 targetAngle = nextPoint - currentPoint;
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(targetAngle, Vector3.up);

            Vector3 perp = Vector3.Cross(transform.forward, targetAngle);
            float dir = Vector3.Dot(perp, transform.up);
            if (dir > 0f)
                animator.SetFloat("Turn", 1f);
            else
                animator.SetFloat("Turn", 0f);

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

            animator.SetBool("Walking", false);
            yield return new WaitForSeconds(waitTimeAtPoint);
        }
    }

    bool SpotPlayer()
    {
        if(playerPosition != null && Vector3.Distance(visionObject.position, playerPosition.position) < visionDistance)
        {
            Vector3 dirPlayer = (playerPosition.position - visionObject.position).normalized;
            float anglePlayer = Vector3.Angle(transform.forward, dirPlayer);
            if(anglePlayer < visionAngel)
            {
                Physics.Linecast(visionObject.position, playerPosition.position, out visionHit);
                if (!visionHit.transform.CompareTag("Environment"))
                {
                    Debug.DrawLine(visionObject.position + Vector3.up, playerPosition.position, Color.red, 1f);
                    GameManager.player.GetComponent<PlayerMovement>().LookAtEnemy(transform);
                    if (moveRoutine != null)
                        StopCoroutine(moveRoutine);
                    animator.SetBool("Walking", false);
                    transform.LookAt(playerPosition, transform.up);
                    return true;
                }
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Transform[] wayPoints;
    int pointPos;
    [SerializeField] float waitTimeAtPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float trunSpeed;

    [SerializeField] float visionDistance;
    [SerializeField] float visionAngel;
    private IEnumerator Start()
    {
        while (GameManager.player == null)
            yield return null;
        player = GameManager.player.transform;

        //StartCoroutine(Movement());
    }

    private void Update()
    {
        if (SpotPlayer())
        {
            print("spotted");
        }
    }

    //IEnumerator Movement()
    //{

    //}

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
                    return true;
                }
            }
        }
        return false;
    }
}

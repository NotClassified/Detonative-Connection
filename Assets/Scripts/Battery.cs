using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{


    void Start()
    {
        transform.position = new Vector3(Random.Range(-12f, 12f), transform.position.y, Random.Range(-12f, 12f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{


    void Start()
    {
        transform.position = new Vector3(Random.Range(-12f, 12f), transform.position.y, Random.Range(-12f, 12f));
    }

    private void Update()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 2, 7);
        foreach (Collider coll in colls)
        {
            print(coll);
            if(coll.gameObject.CompareTag("Player"))
            {
                print(coll);
                print("yee");
                coll.gameObject.GetComponent<PlayerInventory>().CollectBattery();
                print(coll);
                Destroy(gameObject);
                return;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] float radiusPickUp;

    private void Update()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, radiusPickUp);
        foreach (Collider coll in colls)
        {
            if(coll.gameObject.CompareTag("Player"))
            {
                coll.gameObject.GetComponent<PlayerInventory>().CollectBattery();
                GameManager.gm.BatteryPickUp(int.Parse(name.Substring(7)) - 1);
                Destroy(gameObject);
                return;
            }
        }
    }
}

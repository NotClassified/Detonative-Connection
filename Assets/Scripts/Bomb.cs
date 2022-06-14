using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] Transform controlTransfrom;
    [SerializeField] float radiusConnect;
    [SerializeField] GameObject[] chargers;
    [SerializeField] Material chargedColor;
    int numCharged = 0;

    void Update()
    {
        Collider[] colls = Physics.OverlapSphere(controlTransfrom.position, radiusConnect);
        foreach (Collider coll in colls)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                int playerBatteries = coll.gameObject.GetComponent<PlayerInventory>().ConnectBattery();
                for (int i = 0; i < playerBatteries; i++)
                {
                    chargers[numCharged].GetComponent<Renderer>().material = chargedColor;
                    numCharged++;
                }
            }
        }
    }
}

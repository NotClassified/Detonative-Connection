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

    [SerializeField] GameObject connectUI;

    void Update()
    {
        Collider[] colls = Physics.OverlapSphere(controlTransfrom.position, radiusConnect);
        bool inRangeOfPlayer = false;
        foreach (Collider coll in colls)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                inRangeOfPlayer = true;
                connectUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                    ConnectBatteries(coll);
            }
        }
        if (!inRangeOfPlayer)
            connectUI.SetActive(false);
    }

    void ConnectBatteries(Collider coll)
    {
        int playerBatteries = coll.gameObject.GetComponent<PlayerInventory>().ConnectBattery();
        if(playerBatteries != 0)
        {
            for (int i = 0; i < playerBatteries; i++)
            {
                chargers[numCharged].GetComponent<Renderer>().material = chargedColor;
                numCharged++;
            }
        }
        //else
            //show UI that says "no batteries"
    }
}

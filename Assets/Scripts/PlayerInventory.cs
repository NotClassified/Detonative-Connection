using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int batteries;

    public void CollectBattery()
    {
        batteries++;
    }
    public int ConnectBattery()
    {
        int numBatteries = batteries;
        batteries = 0;
        return numBatteries;
    }
}

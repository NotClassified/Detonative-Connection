using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int batteries;
    [SerializeField] TextMeshProUGUI batteryText;

    public void CollectBattery()
    {
        batteries++;
        batteryText.text = batteries.ToString();
    }
    public int ConnectBattery()
    {
        int numBatteries = batteries;
        batteries = 0;
        batteryText.text = batteries.ToString();
        return numBatteries;
    }
}

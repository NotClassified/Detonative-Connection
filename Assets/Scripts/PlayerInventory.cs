using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI batteryText;

    private void Awake()
    {
        batteryText.text = GameManager.batteries.ToString();
    }

    public void CollectBattery()
    {
        GameManager.batteries++;
        batteryText.text = GameManager.batteries.ToString();
    }
    public int ConnectBattery()
    {
        int numBatteries = GameManager.batteries;
        GameManager.batteries = 0;
        batteryText.text = GameManager.batteries.ToString();
        return numBatteries;
    }
}

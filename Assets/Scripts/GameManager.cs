using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform batteryParent;
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] int numBatteries;

    void Start()
    {
        for (int i = 0; i < numBatteries; i++)
            Instantiate(batteryPrefab, batteryParent);
    }
}

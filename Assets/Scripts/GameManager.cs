using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform batteryParent;
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] int numBatteries;
    public static GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < numBatteries; i++)
            Instantiate(batteryPrefab, batteryParent);
    }
}

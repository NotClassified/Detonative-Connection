using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform batteryParent;
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] int numBatteries;
    public static GameObject player;
    public static bool gameOver;
    public static GameManager gm;

    [SerializeField] GameObject tutorialUIParent;
    [SerializeField] GameObject spottedUIParent;
    [SerializeField] GameObject explodedUIParent;
    [SerializeField] GameObject victoryUIParent;
    [SerializeField] GameObject timerUIParent;
    [SerializeField] TextMeshProUGUI timer;

    IEnumerator Start()
    {
        gm = this;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < numBatteries; i++)
            Instantiate(batteryPrefab, batteryParent);

        tutorialUIParent.SetActive(true);
        while (!Input.anyKey)
            yield return null;
        tutorialUIParent.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.transform.position = new Vector3(0, 1);
            gameOver = false;
            spottedUIParent.SetActive(false);
        }

    }

    public void SpottedGameOver()
    {
        gameOver = true;
        spottedUIParent.SetActive(true);
    }

    void BatteryFull()
    {

    }
}

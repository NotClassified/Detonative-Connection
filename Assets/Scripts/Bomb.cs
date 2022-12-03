using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] Transform controlTransfrom;
    [SerializeField] float radiusConnect;
    public GameObject[] chargers;
    [SerializeField] Material chargedColor;

    [SerializeField] GameObject taskUI;
    [SerializeField] Slider batteriesConnectedSlider;
    [SerializeField] GameObject connectUI;
    [SerializeField] GameObject noBatteriesUI;

    [SerializeField] GameObject evacuateUI;
    [SerializeField] Slider distanceSlider;
    [SerializeField] float safeDistance;
    [SerializeField] float distanceFromPlayer;

    private void Start()
    {
        taskUI.SetActive(true);
    }

    void Update()
    {
        Collider[] colls = Physics.OverlapSphere(controlTransfrom.position, radiusConnect);
        bool inRangeOfPlayer = false;
        foreach (Collider coll in colls)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                inRangeOfPlayer = true;
                if (GameManager.numCharged < chargers.Length)
                {
                    if(!noBatteriesUI.activeSelf)
                        connectUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                        ConnectBatteries(coll);
                }
                else
                {
                    connectUI.SetActive(false);
                }
            }
        }
        if (!inRangeOfPlayer || GameManager.gm.gameOver)
        {
            connectUI.SetActive(false);
            noBatteriesUI.SetActive(false);
        }

        if (evacuateUI.activeSelf)
        {
            distanceFromPlayer = Vector3.Distance(controlTransfrom.position, GameManager.player.transform.position);
            if(distanceFromPlayer < safeDistance)
                distanceSlider.value = (safeDistance - distanceFromPlayer) / safeDistance;
            else
            {
                evacuateUI.SetActive(false);
                GameManager.gm.Victory();
            }
        }
    }

    void ConnectBatteries(Collider coll)
    {
        int playerBatteries = coll.gameObject.GetComponent<PlayerInventory>().ConnectBattery();
        if (playerBatteries != 0)
        {
            for (int i = 0; i < playerBatteries; i++)
            {
                chargers[GameManager.numCharged].GetComponent<Renderer>().material = chargedColor;
                GameManager.numCharged++;
                batteriesConnectedSlider.value = GameManager.numCharged;
            }
            if (GameManager.numCharged == chargers.Length)
            {
                AllBatteriesConnected();
            }
        }
        else
        {
            connectUI.SetActive(false);
            noBatteriesUI.SetActive(true);
        }
    }
    public void ConnectBatteries(int charged)
    {
        for (int i = 0; i < charged; i++)
        {
            chargers[i].GetComponent<Renderer>().material = chargedColor;
            batteriesConnectedSlider.value = GameManager.numCharged;
        }
    }

    public void AllBatteriesConnected()
    {
        GameManager.gm.BatteryFull();
        taskUI.SetActive(false);
        evacuateUI.SetActive(true);
    }
}

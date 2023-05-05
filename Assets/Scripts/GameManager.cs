using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameObject player;
    public bool gameOver = false;
    public static GameManager gm;
    public Bomb bombScript;

    [SerializeField] GameObject PauseUIParent;
    [SerializeField] GameObject tutorialUIParent;
    Coroutine tutorialRoutine;
    [SerializeField] GameObject spottedUIParent;
    [SerializeField] GameObject explodedUIParent;
    [SerializeField] GameObject victoryUIParent;
    [SerializeField] GameObject timerUIParent;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] int timeBomb;
    Coroutine countDownRoutine;

    public static int batteries;
    public static int numCharged = 0;
    public Transform batteryParent;
    public GameObject[] batteriesOnMap;
    public static bool[] batteriesOnMapDestroy;
    public Vector3 bombSpawn;

    void Start()
    {
        gm = this;
        player = GameObject.FindGameObjectWithTag("Player");
        tutorialRoutine = StartCoroutine(ShowTutorial());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        bombScript.ConnectBatteries(numCharged);

        if(batteriesOnMapDestroy == null)
        {
            batteriesOnMapDestroy = new bool[batteryParent.childCount];
        }

        //get batteries if there are no references and all bateries havn't been used yet
        if (batteriesOnMap.Length == 0)
        {
            batteriesOnMap = new GameObject[batteriesOnMapDestroy.Length];
            foreach (Transform child in batteryParent)
            {
                batteriesOnMap[int.Parse(child.name.Substring(7)) - 1] = child.gameObject;
            }
        }
        //check each battery to see if it was picked up, if it was, then destroy that battery
        for (int i = 0; i < batteriesOnMapDestroy.Length; i++)
        {
            if (batteriesOnMapDestroy[i])
                Destroy(batteriesOnMap[i]);
        }

        //if the player gets spotted after connecting all batteries, spawn player next to bomb
        if (numCharged == bombScript.chargers.Length)
        {
            bombScript.AllBatteriesConnected();
            player.transform.position = bombSpawn;
            tutorialUIParent.SetActive(false);
        }
    }

    public void StartShowTutorial()
    {
        if (tutorialRoutine != null)
            StopCoroutine(tutorialRoutine);
        tutorialRoutine = StartCoroutine(ShowTutorial());
    }
    IEnumerator ShowTutorial()
    {
        tutorialUIParent.SetActive(true);
        yield return new WaitForEndOfFrame();
        while (!Input.anyKeyDown)
            yield return null;
        tutorialUIParent.SetActive(false);
    }

    public void Continue()
    {
        PauseUIParent.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        //reset game progress
        numCharged = 0;
        batteries = 0;
        gameOver = false;
        Time.timeScale = 1;
        batteriesOnMapDestroy = new bool[batteriesOnMap.Length];
        SceneManager.LoadScene(1);
    }
    public void RestartNoReset()
    {
        //restart level, do not reset game progress
        gameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        gameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartShowTutorial();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseUIParent.activeSelf)
            {
                PauseUIParent.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                PauseUIParent.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void SpottedGameOver()
    {
        gameOver = true;
        timerUIParent.SetActive(false);
        spottedUIParent.SetActive(true);
        Invoke("RestartNoReset", 2.5f);
    }

    public void BatteryPickUp(int index)
    {
        //keep track of which batteries were picked up
        batteriesOnMapDestroy[index] = true;
    }

    public void BatteryFull()
    {
        timerUIParent.SetActive(true);
        countDownRoutine = StartCoroutine(CountDown(timeBomb));
    }

    IEnumerator CountDown(int timeSeconds)
    {
        int timeMinutes = 0;
        while (timeSeconds >= 60)
        {
            timeMinutes++;
            timeSeconds -= 60;
        }

        while (timeSeconds > 0 || timeMinutes > 0)
        {
            timeSeconds--;
            if (timeSeconds < 0)
            {
                timeSeconds = 59;
                timeMinutes--;
            }

            string seconds = timeSeconds - 10 < 0 ? "0" + timeSeconds.ToString() : timeSeconds.ToString();
            timerText.text = timeMinutes.ToString() + ":" + seconds;
            yield return new WaitForSeconds(1f);
        }
        timerUIParent.SetActive(false);
        explodedUIParent.SetActive(true);
        gameOver = true;
        Invoke("Restart", 3f);
    }
    
    public void Victory()
    {
        victoryUIParent.SetActive(true);
        timerUIParent.SetActive(false);
        StopCoroutine(countDownRoutine);
        gameOver = true;
        numCharged = 0;
        batteries = 0;
        batteriesOnMapDestroy = new bool[batteriesOnMap.Length];
        Invoke("Quit", 2f);
    }
}

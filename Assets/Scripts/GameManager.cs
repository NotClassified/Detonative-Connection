using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameObject player;
    public static bool gameOver;
    public static GameManager gm;

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

    void Start()
    {
        gm = this;
        player = GameObject.FindGameObjectWithTag("Player");
        tutorialRoutine = StartCoroutine(ShowTutorial());
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
            }
            else
            {
                PauseUIParent.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void SpottedGameOver()
    {
        gameOver = true;
        timerUIParent.SetActive(false);
        spottedUIParent.SetActive(true);
        Invoke("Restart", 2.5f);
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
        Invoke("Quit", 2f);
    }
}

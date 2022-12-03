using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject tutorialUIParent;
    [SerializeField] GameObject BackUI;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowTutorial()
    {   
        tutorialUIParent.SetActive(true);
        BackUI.SetActive(true);
    }
    public void Back()
    {
        tutorialUIParent.SetActive(false);
        BackUI.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    bool currentSelectionIsStart = true;
    bool isGameOverScreen = false;

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    [SerializeField] int firstLevelIndex = 1;

    private void Start() 
    {
        if(currentSelectionIsStart)
        {
            startButton.Select();
        }
        else
        {
            quitButton.Select();
        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene(firstLevelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    void OnMenuToggle()
    {
        if(currentSelectionIsStart)
        {
            quitButton.Select();
            currentSelectionIsStart = false;
        } 
            else 
                {
                    startButton.Select();
                    currentSelectionIsStart = true;
                }
    }

    void OnMenuSelect()
    {
        if(currentSelectionIsStart)
        {
            GameStart();
        } 
        else 
            {
                QuitGame();
            }
    }

    




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.alictus.sdklite;
public class GameManager : MonoBehaviour
{
    public static bool isGameStarted = false;
    public static bool isGameOver = false;
    #region singleton

    public static GameManager instance;



    private void Awake()
    {
        if (instance!=null)
        {
            return;
        }
        else
        {
            instance = this;
        }
        Application.targetFrameRate = 60;
    }
    #endregion

    private void Start()
    {
        StartGame();

    }

    private void StartGame()
    {
        LevelManager.Instance.StartGame();
        isGameStarted = true;
        isGameOver = false;
    }

   
    public void GameOverFail()
    {
        isGameOver = true;
        isGameStarted = false;

        LevelManager.Instance.GameOver();
        ViewController.instance.GotoFailScreen();
    }
    public void GameOverSuccess()
    {
        AlictusSDK.LevelComplete(LevelManager.Instance.GetCurrentLevel(), true);

        LevelManager.Instance.GameOver();
        LevelManager.Instance.character.mainCharController.enabled = false;
        LevelManager.Instance.character.fpunchMechanism.isPunching = false;
        LevelManager.Instance.character.fpunchMechanism.StopAllCoroutines();
        ViewController.instance.GoToSuccessScreen();
        
    }
}

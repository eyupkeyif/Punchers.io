using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Analytics;
public class ViewController : MonoBehaviour
{
    #region Singleton
    public static ViewController instance;

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

 
    }
    #endregion

    [SerializeField] ViewManager viewManager;
    private MainMenuView mainMenuView;
    private InGameView inGame;
    private SuccessView successView;
    private FailView failView;
    private void Start()
    {
        viewManager.ShowStartView();

        mainMenuView= viewManager.GetView<MainMenuView>();
        inGame = viewManager.GetView<InGameView>();
        successView = viewManager.GetView<SuccessView>();
        failView = viewManager.GetView<FailView>();
        if (inGame!=null)
        {
            inGame.SetLevelTextBar();
        }
        if (mainMenuView!=null)
        {
            mainMenuView.SetMainMenuLevelTextBar();
                mainMenuView.PlayeButtonEvent += StartPlay;

            
        }

        if (successView!= null)
        {
            successView.SuccessButtonEvent += OnLoadNextLevel;
            successView.SuccessParticlePlay();
        }
        if (failView!= null)
        {
            failView.FailButtonEvent += LoadCurrentLevel;
            failView.FailParticlePlay();
        }

    }

    public void StartPlay()
    {
        viewManager.Show(inGame);
        LevelManager.Instance.StartCharacterPlay();
        UpdateProgressBar();
        LevelManager.Instance.OnDeadEnemyCount += LevelManager.Instance.CountEnemyDead;

        LevelManager.Instance.OnLevelCompleted += GameManager.instance.GameOverSuccess;
        try
        {
            List<Parameter> paramList = new List<Parameter>{
            new Firebase.Analytics.Parameter("progression_1", "Level" + LevelManager.Instance.GetCurrentLevel().ToString()),
            new Firebase.Analytics.Parameter("progression_2", "Completed")
            };
            GAEvent.LogEvent("Progression", paramList);
        }
        catch
        {
        }

    }


    public void EnableProgressBar()
    {
        inGame.EnableProgressBar();
    }

    public void UpdateProgressBar()
    {
            
        inGame.SetProgressBarAmount(LevelManager.Instance.GetProgress());
        inGame.SetProgressBarText(LevelManager.Instance.GetTotalDeadEnemy(), LevelManager.Instance.GetMaxDeadEnemy());

    }

    public void DisableProgressBar()
    {
        inGame.DisableProgressBar();
    }

    public void OnLoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
        viewManager.Show(mainMenuView);
    }
    public void LoadCurrentLevel()
    {
        LevelManager.Instance.LoadCurrentLevel();
        viewManager.Show(mainMenuView);
    }

    public void GotoFailScreen()
    {
        viewManager.Show(failView);
    }
    public void GoToSuccessScreen()
    {
        viewManager.Show(successView);
    }
}

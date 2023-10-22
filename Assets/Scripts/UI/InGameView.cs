using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InGameView : View
{
    [SerializeField] ProgressBar progressBar;
    [SerializeField] TextMeshProUGUI levelCount;
    [SerializeField] float progressAmount;
    private GameManager GM;
    public delegate void ActionDelegate();
    public ActionDelegate pauseButtonEvent;

    [SerializeField] CustomButton pauseButton;

    public override void Initialize()
    {
        pauseButton.onPointerUpEvent += Pause;
        ResetProgressBarAmount();
    }
    private void OnEnable()
    {
        ResetProgressBarAmount();
    }

    public void ResetProgressBarAmount()
    {

        progressBar.DirectlyFillProgressBar(0);
    }


    public void SetProgressBarAmount(float amount)
    {
        if (progressBar.gameObject.activeInHierarchy)
        {
            progressBar.FillProgressBar(amount);
        }
    }

    public void SetProgressBarText(int count, int maxCount)
    {

        if (progressBar)
        {
            progressBar.SetProgressText(count, maxCount);
        }
    }
    public void SetLevelTextBar()
    {
        levelCount.text = "LV: " + PlayerPrefs.GetInt("CurrentLevel");
    }

    public void DisableProgressBar()
    {

        progressBar.gameObject.SetActive(false);
    }
    public void EnableProgressBar()
    {
        progressBar.gameObject.SetActive(true);
    }

    public void Pause()
    {
        pauseButtonEvent?.Invoke();

    }
}

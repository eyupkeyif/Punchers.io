using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider fillBar;
    [SerializeField] TextMeshProUGUI progressText;
    float time = 1f;
    public void FillProgressBar(float amount)
    {
        StartCoroutine(FillBarCoroutine(amount));
    }

    IEnumerator FillBarCoroutine(float amount)
    {
        float timer = 0;

        while (timer<=time)
        {

            if (fillBar && gameObject.activeInHierarchy)
            {
                fillBar.value = Mathf.Lerp(fillBar.value, amount, Mathf.Pow(timer / time, 2));
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

    }

    public void DirectlyFillProgressBar(float amount)
    {

        fillBar.value = amount;
    }

    public void SetProgressText(int count, int maxCount)
    {

        progressText.text = count + " / " + maxCount;
    }

}

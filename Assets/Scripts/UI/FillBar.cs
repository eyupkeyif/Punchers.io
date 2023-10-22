using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillBar : MonoBehaviour
{
    [SerializeField] float width = 150f;
    [SerializeField] GameObject bar;
    List<HealthProgress> healthBars = new();

    public void SetHealthBar(int maxHealth, Color color)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject bars = Instantiate(bar, transform);
            bars.SetActive(true);
            float xPos = -0.5f * width * (maxHealth - 1) + width * i;

            bars.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPos, 0, 0);

            HealthProgress healthProgress = bars.GetComponent<HealthProgress>();

            if(healthProgress != null)
            {
                healthProgress.SetColor(color);
                healthBars.Add(healthProgress);
            }
            else
            {
                Debug.LogWarning("Health Progress not found.");
            }

        }
    }

    public void SetHealth(int currentHealth)
    {
        for(int ind = 0; ind < healthBars.Count; ind++)
        {
            if(ind < currentHealth)
            {
                healthBars[ind].IncreaseHealth();
            }
            else
            {
                healthBars[ind].DecreaseHealth();

            }
            
        }
        if (currentHealth<=0)
        {
            Destroy(gameObject);
        }
    }

}

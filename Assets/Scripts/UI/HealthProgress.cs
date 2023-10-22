using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthProgress : MonoBehaviour
{
    [SerializeField] Image slider;

  public void IncreaseHealth()
    {

        slider.gameObject.SetActive(true);
    }
    public void DecreaseHealth()
    {

        slider.gameObject.SetActive(false);
       
    }

    public void SetColor(Color color)
    {
        slider.color = color;
    }

    
}

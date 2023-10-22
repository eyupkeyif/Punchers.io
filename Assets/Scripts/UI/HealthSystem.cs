using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class HealthSystem : MonoBehaviour
{
    public GameObject healthBarCanvas;
    public Transform healthPointPos;
    [SerializeField] Color healthBarColor;

    BarFollow barFollowScript;
    FillBar fillBarScript;
    CharacterController controller;

    private void Awake()
    {

    }

    public void Start()
    {

    }

    public void SetupHealthBarCanvas(int maxhealth)
    {
        controller = GetComponent<CharacterController>();
        GameObject healthBar = Instantiate(healthBarCanvas, healthPointPos.position, Quaternion.identity);
        healthBar.SetActive(true);
        barFollowScript = healthBar.GetComponent<BarFollow>();

        fillBarScript = healthBar.GetComponentInChildren<FillBar>();
        fillBarScript.SetHealthBar(maxhealth, healthBarColor);
        barFollowScript.SetupBar(gameObject, controller.height, healthPointPos);
    }
   

    public void SetHealth(int currentHealth)
    {
        fillBarScript.SetHealth(currentHealth);
    }

    public void ChangeColor()
    {

        healthBarCanvas.GetComponentInChildren<Image>().color = Color.green;
    }


}

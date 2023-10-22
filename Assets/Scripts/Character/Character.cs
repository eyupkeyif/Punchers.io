using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.UI;
public class Character : PuncherBase
{
    private Controller controller;
    public CharacterController mainCharController;
    public PunchMechanism fpunchMechanism;
    bool isSpeedBooster = false;
    protected override void SetupComponents()
    {
        base.SetupComponents();
        mainCharController.enabled=true;
        healthSystem.SetupHealthBarCanvas(stats.maxHealth);
        healthSystem.SetHealth(stats.currentHealth);
        
        stats.maxHealth = 5;
        controller = GetComponent<Controller>();
        controller.SetupStats(stats);
        Debug.Log(numOfKills);
    }

    protected override void AddKill()
    {
        base.AddKill();
        OnEnemyDeath?.Invoke();

    }
    protected override void Killed()
    {

        GameManager.instance.GameOverFail();
        base.Killed();

        StopAllCoroutines();
    }

    public override void SpeedBooster(float movSpeed)
    {
        if (!isSpeedBooster)
        {
            StartCoroutine(SpeedBoosterCoroutine(movSpeed));

        }
    }


    IEnumerator SpeedBoosterCoroutine(float Speed)
    {
        isSpeedBooster = true;
        float initialSpeed = controller.movementSpeed;


        controller.movementSpeed *= Speed;
        yield return new WaitForSeconds(5f);
        controller.movementSpeed = initialSpeed;
        isSpeedBooster = false;
    }

}

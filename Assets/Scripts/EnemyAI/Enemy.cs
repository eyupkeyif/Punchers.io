using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : PuncherBase
{
    private EnemyController enemyController;
    private EnemyAttack enemyAttack;
    bool isRunningBooster = false;

    protected override void SetupComponents()
    {
        base.SetupComponents();

        healthSystem.SetupHealthBarCanvas(stats.maxHealth);
        healthSystem.SetHealth(stats.currentHealth);

        enemyAttack = GetComponent<EnemyAttack>();
        enemyAttack.SetupStats(stats);

        enemyController = GetComponent<EnemyController>();
        enemyController.SetupStats(stats);
    }

    public override void ShakeCam(Collider collider)
    {
        base.ShakeCam(collider);

        if (collider.transform.root.gameObject.GetComponent<Character>())
        {
            Camera.main.GetComponent<CameraManager>().ShakeCamera(0.2f, 0.3f);
        }
    }

    public override void SpeedBooster(float movSpeed)
    {
        if (!isRunningBooster)
        {

            StartCoroutine(SpeedBoosterCoroutine(movSpeed));

        }
    }


    IEnumerator SpeedBoosterCoroutine(float Speed)
    {
        float initialSpeed = enemyController.movementSpeed;

        isRunningBooster = true;

        enemyController.movementSpeed *= Speed;
        yield return new WaitForSeconds(5f);
        enemyController.movementSpeed = initialSpeed;
        isRunningBooster = false;
    }

}

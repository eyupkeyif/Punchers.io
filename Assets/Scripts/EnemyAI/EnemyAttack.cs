using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController enemyController;

    private List<Collider> enemyList = new();

    private float checkRadius = 8f;
    private float checkInterval = 3f;

    [Range(0f, 10f)]
    public float agressiveness = 3f;



    private void Start()
    {
        SetupComponents();

        StartToCheckEnvironment();
    }

    public void SetupStats(Stats stats)
    {

    }

    private void StartToCheckEnvironment()
    {
        StartCoroutine(CheckCoroutine());
    }

    private IEnumerator CheckCoroutine()
    {
        while (true)
        {
            int maxColliders = 10;

            Collider[] hitColliders = new Collider[maxColliders];

            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, checkRadius, hitColliders);

            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].GetType() == typeof(CharacterController) && hitColliders[i].transform.root.gameObject != gameObject) // filter colliders, just take character controllers
                {
                    enemyList.Add(hitColliders[i]);
                }
            }

            AttackDecider();

            enemyList.Clear();

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void AttackDecider()
    {
        if (enemyList.Count <= 0)
            return;

        int randomInt = Random.Range(0, 10);

        if (randomInt <= agressiveness)
        {
            Collider target = GetRandomEnemy();

            enemyController.SetTarget(target.transform);
        }
    }

    private Collider GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemyList.Count - 1);

        return enemyList[randomIndex];
    }

    private void SetupComponents()
    {
        enemyController = GetComponent<EnemyController>();
    }


}

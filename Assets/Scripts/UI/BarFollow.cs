using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarFollow : MonoBehaviour
{

    GameObject enemyObj;
    float enemyH;
    Transform healthBarPos;
    [SerializeField] float offsetZ;
   public void SetupBar(GameObject enemy, float enemyHeight, Transform healthBarPoint)
    {
        enemyObj = enemy;
        enemyH = enemyHeight;
        healthBarPos = healthBarPoint;

    }

    private void UpdatePosition()
    {
        if (enemyObj != null)
        {
            transform.position = new Vector3(enemyObj.transform.position.x, healthBarPos.position.y, enemyObj.transform.position.z+offsetZ);
        }
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }
}

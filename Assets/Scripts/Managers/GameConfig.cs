using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
[System.Serializable]
public class EnemyUnit
{
    public GameObject enemyBody;
    public List<GameObject> enemyPunches;

}



public class GameConfig : MonoBehaviour
{
     #region Singleton
    public static GameConfig Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    

    public float mapSize;

    public int enemyCountInGame = 7;

    public GameObject characterPrefab;

    public List<EnemyUnit> enemyList;
    public List<GameObject> collectibleList;
    public List<GameObject> normalCollectibleList;
}

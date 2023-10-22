using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.alictus.sdklite;
public class LevelManager : MonoBehaviour
{

    #region Singleton
    public static LevelManager Instance { get; private set; }
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

        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
    }
    [SerializeField] int currentLevel = 1;

    #endregion
    [HideInInspector] public Character character;
    public int normalCollectiblecount,collectibleCount;
    [HideInInspector] public List<PuncherBase> enemiesInGame = new();
    [HideInInspector] public List<CollectibleManager> collectiblesInGame = new();
    [HideInInspector] public List<NormalCollectibleManager> normalCollectiblesInGame = new();

    public delegate void DeadEnemyCount();
    public event DeadEnemyCount OnDeadEnemyCount;
    public delegate void LevelCompleteDelegate();
    public event LevelCompleteDelegate OnLevelCompleted;
    public int maxDeadEnemy;
    [SerializeField] Joystick joystick;
   
    public void StartCharacterPlay()
    {

        joystick.Enable();
        SpawnCharacter();
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.SpawnToInitialPosition();
        cameraManager.StartFollow(character.gameObject);
    }
    public void StartGame()
    {
        
        SpawnInitialEnemies();
        SpawnInitialCollectible();
        SpawnInitialNormalCollectibles();

    }

    public void GameOver()
    {
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.StopFollow();
        joystick.Disable();
       
    }

    private void SpawnCharacter()
    {
        if (character!= null)
        {
            character.OnEnemyDeath += CountEnemyDead;
            return;
        }
        else
        {

            GameObject spawnedChar = Instantiate(GameConfig.Instance.characterPrefab);
            //spawnedChar.transform.position = GetRandomPointOnMap();
            character = spawnedChar.GetComponent<Character>();
            character.OnEnemyDeath += CountEnemyDead;


        }
    }

    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < GameConfig.Instance.enemyCountInGame; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnInitialNormalCollectibles()
    {
        for (int i = 0; i < normalCollectiblecount; i++)
        {
            NormalCollectibleSpawn();
        }

    }

    private void SpawnEnemy()
    {
        int randInt = Random.Range(0, GameConfig.Instance.enemyList.Count );

        EnemyUnit randomEnemyUnit = GameConfig.Instance.enemyList[randInt];
        GameObject randomEnemyPrefab = randomEnemyUnit.enemyBody;

        Vector3 spawnPos = GetRandomPointOnMap();

        GameObject spawnedEnemyObj = Instantiate(randomEnemyPrefab, spawnPos, Quaternion.identity);

        PuncherBase spawnedEnemy = spawnedEnemyObj.GetComponent<PuncherBase>();
        enemiesInGame.Add(spawnedEnemy);
    }

    private void SpawnCollectible()
    {
        int randInt = Random.Range(0, GameConfig.Instance.collectibleList.Count);
        GameObject collectiblePrefab = GameConfig.Instance.collectibleList[randInt];

        Vector3 spawnPos = GetRandomPointOnMap();
        Vector3 spawnPosOffset = spawnPos + new Vector3(0, .2f, 0);
        GameObject spawnedCollectible = Instantiate(collectiblePrefab);

        spawnedCollectible.transform.position = spawnPosOffset;
        spawnedCollectible.transform.rotation = Quaternion.Euler(0, -90, -20);

        CollectibleManager spawnedCollectibleItem = spawnedCollectible.GetComponent<CollectibleManager>();
        collectiblesInGame.Add(spawnedCollectibleItem);

    }

    private void SpawnInitialCollectible()
    {

        for (int i = 0; i < collectibleCount; i++)
        {
            SpawnCollectible();
        }
    }

    private void NormalCollectibleSpawn()
    {

            int randomInt = Random.Range(0, GameConfig.Instance.normalCollectibleList.Count);
            GameObject normalCollectiblePrefab = GameConfig.Instance.normalCollectibleList[randomInt];

            Vector3 normalSpawnPos = GetRandomPointOnMap();

            Vector3 normalSpawnOffset =normalSpawnPos+ new Vector3(0, 0.5f, 0);

            GameObject normalSpawnedCollectible = Instantiate(normalCollectiblePrefab, normalSpawnOffset, Quaternion.identity);
            NormalCollectibleManager spawnedNormalCOllectible = normalSpawnedCollectible.GetComponent<NormalCollectibleManager>();
            normalCollectiblesInGame.Add(spawnedNormalCOllectible);
        
    }

    private Vector3 GetRandomPointOnMap()
    {
        float mapRadius = GameConfig.Instance.mapSize * 0.5f;

        float randX = Random.Range(-mapRadius, mapRadius);
        float randZ = Random.Range(-mapRadius, mapRadius);

        Vector3 spawnPoint = new Vector3(randX, 0, randZ);

        return spawnPoint;
    }

    public void RemoveKilledEnemy(PuncherBase killedEnemy)
    {
        enemiesInGame.Remove(killedEnemy);

        Invoke("SpawnEnemy", 0.5f);
    }

    public void RemoveCollectedItem(CollectibleManager collectedItem)
    {
        collectiblesInGame.Remove(collectedItem);
        Invoke("SpawnCollectible", 1f);
    }
    public void RemoveNormalCollectedItem(NormalCollectibleManager normalCollectedItem)
    {
        normalCollectiblesInGame.Remove(normalCollectedItem);
        Invoke("NormalCollectibleSpawn", 0.5f);
    
    }

    public int GetTotalDeadEnemy()
    {

        return character.numOfKills;
    }

    public int GetMaxDeadEnemy()
    {
        maxDeadEnemy = currentLevel + 1;
        return maxDeadEnemy;
    }

    public float GetProgress()
    {

        float progressAmount = (float)character.numOfKills / (float)maxDeadEnemy;
        return progressAmount;

    }
    public int GetCurrentLevel()
    {

        return currentLevel;
    }
    public void LoadNextLevel()
    {
        currentLevel += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        AlictusSDK.LevelTag = "CurrentLevel";
        AlictusSDK.LevelIndex = PlayerPrefs.GetInt("CurrentLevel");


    }
    public void LoadCurrentLevel()
    {

        AlictusSDK.LevelTag = "CurrentLevel";
        AlictusSDK.LevelIndex = PlayerPrefs.GetInt("CurrentLevel");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void CountEnemyDead()
    {
        if (character.numOfKills >= maxDeadEnemy)
        {
            OnLevelCompleted?.Invoke();
        }
    }

    
}

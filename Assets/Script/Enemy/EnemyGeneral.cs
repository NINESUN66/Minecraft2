using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{
    [Header("BaseEnemy随时间生成概率调整(秒)")]
    public float 基础敌人以最大概率生成的持续时间 = 180f;
    public float 基础敌人稳定刷新概率 = 0.2f;
    public float baseEnemyInterval = 1.0f;

    [Header("BetterEnemy随时间生成概率调整(秒)")]
    public float 更强的敌人初始刷新时间 = 120f;
    public float 更强的敌人稳定刷新时间 = 360f;
    public float 更强的敌人稳定刷新概率 = 0.5f;
    public float betterEnemyInterval = 2.0f;

    [Header("BestEnemy随时间生成概率调整(秒)")]
    public float 最强的敌人初始刷新时间 = 300f;
    public float 最强的敌人稳定刷新概率 = 0.5f;
    public float bestEnemyInterval = 3.0f;

    private float baseEnemyTimer;
    private float betterEnemyTimer;
    private float bestEnemyTimer;
    private float gameTime;

    private GameObject baseEnemyPrefab;
    private GameObject betterEnemyPrefab;
    private GameObject bestEnemyPrefab;

    private float baseEnemyProbability;
    private float betterEnemyProbability;
    private float bestEnemyProbability;

    private Camera mainCamera;
    private GameObject player;

    private float camHeight;
    private float camWidth;

    private void Start()
    {
        baseEnemyTimer = 0f;
        betterEnemyTimer = 0f;
        bestEnemyTimer = 0f;
        gameTime = 0f;

        mainCamera = Camera.main;
        player = GameObject.Find("Player");

        baseEnemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/BaseEnemy");
        betterEnemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/BetterEnemy");
        bestEnemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/BestEnemy");

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        baseEnemyTimer += Time.deltaTime;
        betterEnemyTimer += Time.deltaTime;
        bestEnemyTimer += Time.deltaTime;

        UpdateProbabilities();

        if (baseEnemyTimer >= baseEnemyInterval)
        {
            baseEnemyTimer = 0f;
            TrySpawnEnemy(baseEnemyProbability, baseEnemyPrefab);
        }

        if (betterEnemyTimer >= betterEnemyInterval)
        {
            betterEnemyTimer = 0f;
            TrySpawnEnemy(betterEnemyProbability, betterEnemyPrefab);
        }

        if (bestEnemyTimer >= bestEnemyInterval)
        {
            bestEnemyTimer = 0f;
            TrySpawnEnemy(bestEnemyProbability, bestEnemyPrefab);
        }
    }

    private void UpdateProbabilities()
    {
        // BaseEnemy生成概率
        if (gameTime <= 基础敌人以最大概率生成的持续时间)
        {
            baseEnemyProbability = Mathf.Lerp(1f, 基础敌人稳定刷新概率, gameTime / 基础敌人以最大概率生成的持续时间);
        }
        else
        {
            baseEnemyProbability = 基础敌人稳定刷新概率;
        }

        // BetterEnemy生成概率
        if (gameTime <= 更强的敌人初始刷新时间)
        {
            betterEnemyProbability = 0f;
        }
        else if (gameTime <= 更强的敌人稳定刷新时间)
        {
            betterEnemyProbability = Mathf.Lerp(0f, 更强的敌人稳定刷新概率, (gameTime - 更强的敌人初始刷新时间) / (更强的敌人稳定刷新时间 - 更强的敌人初始刷新时间));
        }
        else
        {
            betterEnemyProbability = 更强的敌人稳定刷新概率;
        }

        // BestEnemy生成概率
        if (gameTime <= 最强的敌人初始刷新时间)
        {
            bestEnemyProbability = 0f;
        }
        else
        {
            bestEnemyProbability = Mathf.Lerp(0f, 最强的敌人稳定刷新概率, (gameTime - 最强的敌人初始刷新时间) / 最强的敌人初始刷新时间);
            bestEnemyProbability = Mathf.Min(bestEnemyProbability, 最强的敌人稳定刷新概率);
        }
    }

    private void TrySpawnEnemy(float probability, GameObject enemyPrefab)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < probability)
        {
            Vector3 spawnPosition = GetSpawnPositionOutsideCameraView();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPositionOutsideCameraView()
    {
        int randomDirection = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (randomDirection)
        {
            case 0: // 左
                spawnPosition = new Vector3(player.transform.position.x - camWidth / 2 - 1, Random.Range(player.transform.position.y - camHeight / 2, player.transform.position.y + camHeight / 2), 0);
                break;
            case 1: // 右
                spawnPosition = new Vector3(player.transform.position.x + camWidth / 2 + 1, Random.Range(player.transform.position.y - camHeight / 2, player.transform.position.y + camHeight / 2), 0);
                break;
            case 2: // 上
                spawnPosition = new Vector3(Random.Range(player.transform.position.x - camWidth / 2, player.transform.position.x + camWidth / 2), player.transform.position.y + camHeight / 2 + 1, 0);
                break;
            case 3: // 下
                spawnPosition = new Vector3(Random.Range(player.transform.position.x - camWidth / 2, player.transform.position.x + camWidth / 2), player.transform.position.y - camHeight / 2 - 1, 0);
                break;
        }

        return spawnPosition;
    }
}

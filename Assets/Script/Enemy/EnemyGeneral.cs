using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{
    [Header("BaseEnemy��ʱ�����ɸ��ʵ���(��)")]
    public float �������������������ɵĳ���ʱ�� = 180f;
    public float ���������ȶ�ˢ�¸��� = 0.2f;
    public float baseEnemyInterval = 1.0f;

    [Header("BetterEnemy��ʱ�����ɸ��ʵ���(��)")]
    public float ��ǿ�ĵ��˳�ʼˢ��ʱ�� = 120f;
    public float ��ǿ�ĵ����ȶ�ˢ��ʱ�� = 360f;
    public float ��ǿ�ĵ����ȶ�ˢ�¸��� = 0.5f;
    public float betterEnemyInterval = 2.0f;

    [Header("BestEnemy��ʱ�����ɸ��ʵ���(��)")]
    public float ��ǿ�ĵ��˳�ʼˢ��ʱ�� = 300f;
    public float ��ǿ�ĵ����ȶ�ˢ�¸��� = 0.5f;
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
        // BaseEnemy���ɸ���
        if (gameTime <= �������������������ɵĳ���ʱ��)
        {
            baseEnemyProbability = Mathf.Lerp(1f, ���������ȶ�ˢ�¸���, gameTime / �������������������ɵĳ���ʱ��);
        }
        else
        {
            baseEnemyProbability = ���������ȶ�ˢ�¸���;
        }

        // BetterEnemy���ɸ���
        if (gameTime <= ��ǿ�ĵ��˳�ʼˢ��ʱ��)
        {
            betterEnemyProbability = 0f;
        }
        else if (gameTime <= ��ǿ�ĵ����ȶ�ˢ��ʱ��)
        {
            betterEnemyProbability = Mathf.Lerp(0f, ��ǿ�ĵ����ȶ�ˢ�¸���, (gameTime - ��ǿ�ĵ��˳�ʼˢ��ʱ��) / (��ǿ�ĵ����ȶ�ˢ��ʱ�� - ��ǿ�ĵ��˳�ʼˢ��ʱ��));
        }
        else
        {
            betterEnemyProbability = ��ǿ�ĵ����ȶ�ˢ�¸���;
        }

        // BestEnemy���ɸ���
        if (gameTime <= ��ǿ�ĵ��˳�ʼˢ��ʱ��)
        {
            bestEnemyProbability = 0f;
        }
        else
        {
            bestEnemyProbability = Mathf.Lerp(0f, ��ǿ�ĵ����ȶ�ˢ�¸���, (gameTime - ��ǿ�ĵ��˳�ʼˢ��ʱ��) / ��ǿ�ĵ��˳�ʼˢ��ʱ��);
            bestEnemyProbability = Mathf.Min(bestEnemyProbability, ��ǿ�ĵ����ȶ�ˢ�¸���);
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
            case 0: // ��
                spawnPosition = new Vector3(player.transform.position.x - camWidth / 2 - 1, Random.Range(player.transform.position.y - camHeight / 2, player.transform.position.y + camHeight / 2), 0);
                break;
            case 1: // ��
                spawnPosition = new Vector3(player.transform.position.x + camWidth / 2 + 1, Random.Range(player.transform.position.y - camHeight / 2, player.transform.position.y + camHeight / 2), 0);
                break;
            case 2: // ��
                spawnPosition = new Vector3(Random.Range(player.transform.position.x - camWidth / 2, player.transform.position.x + camWidth / 2), player.transform.position.y + camHeight / 2 + 1, 0);
                break;
            case 3: // ��
                spawnPosition = new Vector3(Random.Range(player.transform.position.x - camWidth / 2, player.transform.position.x + camWidth / 2), player.transform.position.y - camHeight / 2 - 1, 0);
                break;
        }

        return spawnPosition;
    }
}

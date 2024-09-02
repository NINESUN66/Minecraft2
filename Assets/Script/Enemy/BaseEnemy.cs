using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemy : EnemyParent
{
    public int minDeadExpNum; // ��С���侭��������
    public int maxDeadExpNum; // �����侭��������
    public float expBallSummonRadius = 1.0f; // ���������ɰ뾶
    public float itemDropRate = 0.3f; // ������Ʒ����
    public float touchBodyDamageRate = 1.2f; // ��ײ���������˺�����

    private int deadExpNum; // �������侭��������
    private GameObject expBall;
    private List<GameObject> items = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        deadExpNum = Random.Range(minDeadExpNum, maxDeadExpNum + 1);

        expBall = Resources.Load<GameObject>("Prefabs/Items/EXP");

        Object[] loadedItems = Resources.LoadAll("Prefabs/Items/EnemyRandomDrop", typeof(GameObject));
        for (int i = 0; i < loadedItems.Length; i++)
        {
            items.Add((GameObject)loadedItems[i]);
        }
    }

    protected override void PlayHitSound(Transform transform)
    {
        base.PlayHitSound(transform);
    }

    private void RandomDropItem()
    {
        float randomValue = Random.value;
        if (randomValue < itemDropRate)
        {
            int randomIndex = Random.Range(0, items.Count);

            GameObject droppedItem = Instantiate(items[randomIndex]);
            droppedItem.transform.position = transform.position;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < deadExpNum; i++)
        {
            GameObject exp = Instantiate(expBall);
            float randomX = Random.Range(-expBallSummonRadius, expBallSummonRadius);
            float randomY = Random.Range(-expBallSummonRadius, expBallSummonRadius);
            exp.transform.position = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);
        }

        RandomDropItem();
    }
}

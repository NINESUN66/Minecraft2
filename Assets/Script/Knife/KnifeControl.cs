using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    public int minNum = 1;  //最小存刀数
    public int maxNum = 20; //最大存刀数
    public float rotateSpeed = 20f;  //刀的旋转速度
    public float generateTime = 2.0f; //生成刀的时间间隔

    public int nowNum = 1; //当前存刀数

    public int initNum = 1; //初始存刀数

    public float rotateRadius = 1.0f; //旋转半径

    public GameObject knifePrefab;

    private List<GameObject> knives = new List<GameObject>();

    private string playerKnifeKind;

    private void Rotating()
    {
        if(transform.parent.gameObject.tag == "Player")
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        if(transform.parent.gameObject.tag == "Enemy")
        {
            transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    private void Knife_Generate()
    {
        if (nowNum < maxNum)
        {
            nowNum++;
            GameObject knife = Instantiate(knifePrefab, Vector3.zero, Quaternion.identity);
            knives.Add(knife);
            knife.transform.parent = transform;
            UpdateKnives();
        }
    }

    private void UpdateKnives()
    {
        int count = knives.Count;
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            float radian = angle * Mathf.Deg2Rad;
            float x = rotateRadius * Mathf.Cos(radian);
            float y = rotateRadius * Mathf.Sin(radian);

            GameObject knife = knives[i];
            knife.transform.localPosition = new Vector3(x, y, 0);
            knife.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void ChangeKnifeKind()
    {
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();

        foreach (var item in knives)
        {
            positions.Add(item.transform.position);
            rotations.Add(item.transform.rotation);
        }

        foreach (var item in knives)
        {
            Destroy(item);
        }
        knives.Clear();

        int count = positions.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject knife = Instantiate(knifePrefab, Vector3.zero, Quaternion.identity);
            knives.Add(knife);
            knife.transform.parent = transform;
            knife.transform.position = positions[i];
            knife.transform.rotation = rotations[i];
        }
    }

    private void UpdateKnifeKind()
    {
        int nowLevel = GameObject.Find("Player").GetComponentInChildren<PlayerAttribute>().level;

        if (nowLevel < 10)
        {
            playerKnifeKind = "PlayerBaseKnife";
        }
        else if (nowLevel < 20)
        {
            playerKnifeKind = "PlayerBetterKnife";
            if (nowLevel == 10)
            {
                ChangeKnifeKind();
            }
        }
        else if (nowLevel < 30)
        {
            playerKnifeKind = "PlayerBetterBetterKnife";
            if (nowLevel == 20)
            {
                ChangeKnifeKind();
            }
        }
        else
        {
            playerKnifeKind = "PlayerBestKnife";
            if (nowLevel == 30)
            {
                ChangeKnifeKind();
            }
        }

        if (transform.gameObject.tag == "PlayerKnife")
        {
            knifePrefab = Resources.Load<GameObject>("Prefabs/Knife/" + playerKnifeKind);
        }
    }



    private void Start()
    {
        if(transform.gameObject.tag == "EnemyKnife")
        {
            knifePrefab = transform.Find("EnemyBaseKnife").gameObject;
        }
        else if(transform.gameObject.tag == "PlayerKnife")
        {
            UpdateKnifeKind();
        }
        else
        {
            Debug.LogError("未找到需要生成的刀的预制体");
        }

        float knifeWidth = knifePrefab.GetComponent<BaseKnife>().length;

        rotateRadius +=  knifeWidth / 2;

        foreach (Transform knife in transform)
        {
            knives.Add(knife.gameObject);
        }
        if(knives.Count != initNum)
        {
            for(int i = 0; i < initNum - knives.Count; i ++)
            {
                Knife_Generate();
            }
        }
    }

    private float timer = 0.0f;

    private void Update()
    {
        Rotating();

        timer += Time.deltaTime;
        if (timer >= generateTime)
        {
            timer = 0;
            if(transform.tag != "EnemyKnife") Knife_Generate();
        }

        if (transform.tag == "PlayerKnife") UpdateKnifeKind();
    }
}

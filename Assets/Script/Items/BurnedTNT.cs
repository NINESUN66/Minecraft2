using System.Collections;
using UnityEngine;

public class BurnedTNT : MonoBehaviour
{
    public float boomRadius = 5.0f; // 爆炸半径
    public float boomEnemyDamage = 100.0f; // 敌人受到的伤害
    public float boomPlayerDamage = 50.0f; // 玩家受到的伤害
    public float boomDamageIncreaseRate = 0.2f; // 伤害随距离增加的比率
    public float boomTime = 2.0f; // 爆炸时间
    public float minBlinkSpeed = 2f; // 最小闪烁速度
    public float maxBlinkSpeed = 5f; // 最大闪烁速度
    public float minAlpha = 0.2f; // 最小透明度
    public float maxAlpha = 0.7f; // 最大透明度
    public float lineWidth = 8.0f; // 范围线宽度

    public SpriteRenderer blinkSpriteRenderer;
    public LineRenderer lineRenderer;

    private AudioClip Explosion1;
    private AudioClip Explosion2;
    private AudioClip Explosion3;
    private AudioClip Explosion4;

    private Animator animator;

    private void Start()
    {
        Explosion1 = Resources.Load<AudioClip>("Audio/Explosion1");
        Explosion2 = Resources.Load<AudioClip>("Audio/Explosion2");
        Explosion3 = Resources.Load<AudioClip>("Audio/Explosion3");
        Explosion4 = Resources.Load<AudioClip>("Audio/Explosion4");

        animator = GetComponent<Animator>();

        // 初始化 LineRenderer 以显示爆炸范围
        if (lineRenderer != null)
        {
            DrawExplosionRadius();
        }

        // 协程，启动！
        StartCoroutine(Blink());
        StartCoroutine(WaitAndExplode(transform.gameObject));
    }

    private IEnumerator WaitAndExplode(GameObject tnt)
    {
        // 等一等
        yield return new WaitForSeconds(boomTime);

        // 关闭范围提示
        blinkSpriteRenderer.enabled = false;

        // 播放动画
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        lineRenderer.enabled = false;

        // 随机播放音效
        int randomAudio = Random.Range(1, 5);
        switch (randomAudio)
        {
            case 1:
                AudioSource.PlayClipAtPoint(Explosion1, transform.position);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(Explosion2, transform.position);
                break;
            case 3:
                AudioSource.PlayClipAtPoint(Explosion3, transform.position);
                break;
            case 4:
                AudioSource.PlayClipAtPoint(Explosion4, transform.position);
                break;
        }

        Explode(tnt.transform.position);
    }

    private void Explode(Vector3 explosionPosition)
    {
        Debug.Log("TNT爆炸！");
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPosition, boomRadius);

        foreach (var hitCollider in hitColliders)
        {
            // 计算tnt与碰撞体的距离，距离越近伤害越大，基础伤害为boomEnemyDamage
            float distance = Vector3.Distance(explosionPosition, hitCollider.transform.position);

            if (hitCollider.gameObject.CompareTag("EnemyBody"))
            {
                float damage = boomEnemyDamage + boomEnemyDamage * boomDamageIncreaseRate * (boomRadius - distance);
                hitCollider.GetComponent<BaseEnemy>().HP -= damage;
                Debug.Log("敌人受到伤害：" + damage);
            }

            if (hitCollider.gameObject.CompareTag("PlayerBody"))
            {
                float damage = boomPlayerDamage + boomPlayerDamage * boomDamageIncreaseRate * (boomRadius - distance);
                float damageToTake = hitCollider.GetComponent<PlayerAttribute>().Get_Damage(damage);
                Debug.Log("自身受到伤害：" + damageToTake);
            }
        }
    }

    private IEnumerator Blink()
    {
        float blinkSpeed = minBlinkSpeed;
        float nowAlpha = minAlpha;
        bool increasing = true;

        float timeElapsed = 0f;

        while (true)
        {
            if (increasing)
            {
                nowAlpha += Time.deltaTime * blinkSpeed;
                if (nowAlpha >= maxAlpha)
                {
                    nowAlpha = maxAlpha;
                    increasing = false;
                }
            }
            else
            {
                nowAlpha -= Time.deltaTime * blinkSpeed;
                if (nowAlpha <= minAlpha)
                {
                    nowAlpha = minAlpha;
                    increasing = true;
                }
            }

            Color spriteColor = blinkSpriteRenderer.color;
            spriteColor.a = nowAlpha;
            blinkSpriteRenderer.color = spriteColor;

            blinkSpeed = Mathf.Lerp(minBlinkSpeed, maxBlinkSpeed, timeElapsed / boomTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private void DrawExplosionRadius()
    {
        // 圆的中心点位置
        Vector3 center = transform.position;
        // 圆的半径
        float radius = boomRadius - lineWidth / 2;

        // 获取或添加 LineRenderer 组件
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // 设置坐标点个数为361个，确保圆形闭合
        lineRenderer.positionCount = 361;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Color startColor = new Color(1f, 0f, 0f, 0.5f);
        Color endColor = new Color(1f, 0f, 0f, 0.5f);

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // 设置闭环
        lineRenderer.loop = true;

        // 每一度求得一个在圆上的坐标点
        for (int i = 0; i <= 360; i++)
        {
            float rad = i * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(rad);
            float y = center.y + radius * Mathf.Sin(rad);

            Vector3 pos = new Vector3(x, y, center.z);
            lineRenderer.SetPosition(i, pos);
        }
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
}

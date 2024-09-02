using System.Collections;
using UnityEngine;

public class BurnedTNT : MonoBehaviour
{
    public float boomRadius = 5.0f; // ��ը�뾶
    public float boomEnemyDamage = 100.0f; // �����ܵ����˺�
    public float boomPlayerDamage = 50.0f; // ����ܵ����˺�
    public float boomDamageIncreaseRate = 0.2f; // �˺���������ӵı���
    public float boomTime = 2.0f; // ��ըʱ��
    public float minBlinkSpeed = 2f; // ��С��˸�ٶ�
    public float maxBlinkSpeed = 5f; // �����˸�ٶ�
    public float minAlpha = 0.2f; // ��С͸����
    public float maxAlpha = 0.7f; // ���͸����
    public float lineWidth = 8.0f; // ��Χ�߿��

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

        // ��ʼ�� LineRenderer ����ʾ��ը��Χ
        if (lineRenderer != null)
        {
            DrawExplosionRadius();
        }

        // Э�̣�������
        StartCoroutine(Blink());
        StartCoroutine(WaitAndExplode(transform.gameObject));
    }

    private IEnumerator WaitAndExplode(GameObject tnt)
    {
        // ��һ��
        yield return new WaitForSeconds(boomTime);

        // �رշ�Χ��ʾ
        blinkSpriteRenderer.enabled = false;

        // ���Ŷ���
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        lineRenderer.enabled = false;

        // ���������Ч
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
        Debug.Log("TNT��ը��");
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPosition, boomRadius);

        foreach (var hitCollider in hitColliders)
        {
            // ����tnt����ײ��ľ��룬����Խ���˺�Խ�󣬻����˺�ΪboomEnemyDamage
            float distance = Vector3.Distance(explosionPosition, hitCollider.transform.position);

            if (hitCollider.gameObject.CompareTag("EnemyBody"))
            {
                float damage = boomEnemyDamage + boomEnemyDamage * boomDamageIncreaseRate * (boomRadius - distance);
                hitCollider.GetComponent<BaseEnemy>().HP -= damage;
                Debug.Log("�����ܵ��˺���" + damage);
            }

            if (hitCollider.gameObject.CompareTag("PlayerBody"))
            {
                float damage = boomPlayerDamage + boomPlayerDamage * boomDamageIncreaseRate * (boomRadius - distance);
                float damageToTake = hitCollider.GetComponent<PlayerAttribute>().Get_Damage(damage);
                Debug.Log("�����ܵ��˺���" + damageToTake);
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
        // Բ�����ĵ�λ��
        Vector3 center = transform.position;
        // Բ�İ뾶
        float radius = boomRadius - lineWidth / 2;

        // ��ȡ����� LineRenderer ���
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // ���ò��ʺ���ɫ
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // ������������Ϊ361����ȷ��Բ�αպ�
        lineRenderer.positionCount = 361;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Color startColor = new Color(1f, 0f, 0f, 0.5f);
        Color endColor = new Color(1f, 0f, 0f, 0.5f);

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // ���ñջ�
        lineRenderer.loop = true;

        // ÿһ�����һ����Բ�ϵ������
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

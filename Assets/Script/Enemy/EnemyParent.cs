using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    public float HP;
    public float speed;
    private AudioClip hit1;
    private AudioClip hit2;
    private AudioClip hit3;

    protected virtual void Start()
    {
        hit1 = Resources.Load<AudioClip>("Audio/hit1");
        hit2 = Resources.Load<AudioClip>("Audio/hit2");
        hit3 = Resources.Load<AudioClip>("Audio/hit3");
    }

    protected virtual void PlayHitSound(Transform transform)
    {
        int randomAudio = Random.Range(1, 4);
        switch (randomAudio)
        {
            case 1:
                AudioSource.PlayClipAtPoint(hit1, transform.position);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(hit2, transform.position);
                break;
            case 3:
                AudioSource.PlayClipAtPoint(hit3, transform.position);
                break;
            case 4:
                Debug.LogWarning("…˙≥…¡À444444");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerKnife")
        {
            HP -= collision.gameObject.GetComponent<BaseKnife>().damage;
            PlayHitSound(collision.transform);
        }
    }

    private void Update()
    {
        if (HP <= 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}

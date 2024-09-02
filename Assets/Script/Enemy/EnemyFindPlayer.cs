using UnityEngine;

public class EnemyFindPlayer : MonoBehaviour
{
    private GameObject player;

    private float enemySpeed;

    private void Start()
    {
        player = GameObject.Find("Player");
        enemySpeed = transform.GetComponentInChildren<BaseEnemy>().speed;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.position += direction * enemySpeed * Time.deltaTime;
        }
    }
}

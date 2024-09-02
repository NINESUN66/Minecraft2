using UnityEngine;

public class SyncWithPlayer : MonoBehaviour
{
    private GameObject playerPosition;

    private void Start()
    {
        playerPosition = GameObject.Find("BasePlayer");
    }

    private void Update()
    {
        transform.position = new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y, transform.position.z);
    }
}

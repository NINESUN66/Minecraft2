using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderPearl : FollowPlayer
{
    private float flashDistance = 7.0f;

    private AudioClip enderman_teleport1;
    private AudioClip enderman_teleport2;

    public void Effect()
    {
        Debug.Log("使用了末影珍珠");

        GameObject player = GameObject.Find("Player");

        Vector3 playerPosition = player.transform.position;

        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.WorldToScreenPoint(playerPosition).z));

        Vector3 direction = (mouseWorldPosition - playerPosition).normalized;
        direction.z = 0;

        Vector3 newPosition = playerPosition + direction * flashDistance;

        player.transform.position = newPosition;

        if (Random.Range(0, 2) == 0)
        {
            enderman_teleport1 = Resources.Load<AudioClip>("Audio/Enderman_teleport1");
            AudioSource.PlayClipAtPoint(enderman_teleport1, player.transform.position);
        }
        else
        {
            enderman_teleport2 = Resources.Load<AudioClip>("Audio/Enderman_teleport2");
            AudioSource.PlayClipAtPoint(enderman_teleport2, player.transform.position);
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}

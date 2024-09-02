using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : FollowPlayer
{
    public void Effect()
    {
        Debug.Log(" π”√¡ÀTNT");

        GameObject player = GameObject.Find("Player");
        GameObject tntPrefab = Resources.Load<GameObject>("Prefabs/Items/EnemyRandomDrop/BurnedTNT");
        Vector3 playerPosition = player.transform.position;
        Instantiate(tntPrefab, playerPosition, Quaternion.identity);
    }

    protected override void Update()
    {
        base.Update();
    }
}

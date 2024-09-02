using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotionEffect : PotionEffectParent
{
    public float speedRate;

    private GameObject player;
    private float originSpeed;

    public override void UseOppsitePotition()
    {
        base.UseOppsitePotition();
        speedRate = 0;
    }

    public void IncreaseMoveSpeed()
    {
        player.GetComponent<PlayerMove>().playerMoveSpeed = originSpeed * (1 + speedRate);
    }

    public void RecoverMoveSpeed()
    {
        player.GetComponent<PlayerMove>().playerMoveSpeed = originSpeed;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        base.originDurationTime = base.maxDurationTime;
        base.nowDurationTime = 0f;
        originSpeed = player.GetComponent<PlayerMove>().playerMoveSpeed;
        IncreaseMoveSpeed();
    }

    void Update()
    {
        base.nowDurationTime += Time.deltaTime;
        if (base.nowDurationTime >= base.maxDurationTime)
        {
            Debug.Log("速度/缓慢药水持续时间结束");
            RecoverMoveSpeed();
            base.nowDurationTime = 0f;
            base.maxDurationTime = base.originDurationTime;
            this.enabled = false;
            return;
        }
        IncreaseMoveSpeed();
    }
}

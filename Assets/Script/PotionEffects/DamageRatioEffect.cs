using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRatioEffect : PotionEffectParent
{
    public float damageRate; // 受伤倍率

    private float originDamageRate;
    private GameObject player;

    public override void UseOppsitePotition()
    {
        base.UseOppsitePotition();
        damageRate = 0f;
    }

    public void IncreaseDamageRate()
    {
        player.GetComponentInChildren<PlayerAttribute>().damageReductionRate = Mathf.Clamp(damageRate, -1f, 1f);
    }

    public void RecoverDamageRate()
    {
        player.GetComponentInChildren<PlayerAttribute>().damageReductionRate = originDamageRate;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        base.originDurationTime = base.maxDurationTime;
        base.nowDurationTime = 0f;
        originDamageRate = player.GetComponentInChildren<PlayerAttribute>().damageReductionRate;
        IncreaseDamageRate();
    }

    void Update()
    {
        base.nowDurationTime += Time.deltaTime;
        if (base.nowDurationTime >= base.maxDurationTime)
        {
            Debug.Log("药水持续时间结束");
            RecoverDamageRate();
            base.nowDurationTime = 0f;
            base.maxDurationTime = base.originDurationTime;
            this.enabled = false;
            return;
        }
        IncreaseDamageRate();
    }

}

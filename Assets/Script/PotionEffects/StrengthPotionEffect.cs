using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPotionEffect : PotionEffectParent
{
    public float strengthRate;

    private float baseDamage = 0f;
    private GameObject player;
    private string nowKnifeName;

    public override void UseOppsitePotition()
    {
        base.UseOppsitePotition();
        strengthRate = 0f;
    }

    public void IncreaseAttackDamage()
    {
        var baseKnives = player.GetComponentsInChildren<BaseKnife>();
        foreach (var knife in baseKnives)
        {
            knife.damage = baseDamage * (1 + strengthRate);
        }
    }

    public void RecoverAttactDamage()
    {
        var baseKnives = player.GetComponentsInChildren<BaseKnife>();
        foreach (var knife in baseKnives)
        {
            knife.damage = baseDamage;
        }
    }

    public string GetNowKnifeName()
    {
        return player.GetComponentsInChildren<BaseKnife>()[0].transform.name;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        base.originDurationTime = base.maxDurationTime;
        base.nowDurationTime = 0f;
        nowKnifeName = GetNowKnifeName();
        baseDamage = player.GetComponentsInChildren<BaseKnife>()[0].damage;
        IncreaseAttackDamage();
    }

    void Update()
    {
        base.nowDurationTime += Time.deltaTime;
        if (base.nowDurationTime >= base.maxDurationTime)
        {
            Debug.Log("力量/虚弱药水持续时间结束");
            RecoverAttactDamage();
            base.nowDurationTime = 0f;
            base.maxDurationTime = base.originDurationTime;
            this.enabled = false;
            return;
        }
        if (nowKnifeName != GetNowKnifeName())
        {
            nowKnifeName = GetNowKnifeName();
            baseDamage = player.GetComponentsInChildren<BaseKnife>()[0].damage;
            Debug.Log("刀种类更新");
        }
        IncreaseAttackDamage();
        Debug.Log("BaseDamage" + baseDamage);
    }
}

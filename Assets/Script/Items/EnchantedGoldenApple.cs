using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class EnchantedGoldenApple : FollowPlayer
{
    public float increaseRate = 0.5f;

    public void Effect()
    {
        Debug.Log("使用了附魔金苹果");

        base.IncreasePotionEffects(
            new (Type, float)[]
            {
                (typeof(SpeedPotionEffect), increaseRate),
                (typeof(StrengthPotionEffect), increaseRate),
                (typeof(DamageRatioEffect), increaseRate)
            });
    }

    protected override void Update()
    {
        base.Update();
    }
}

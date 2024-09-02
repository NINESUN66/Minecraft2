using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaknessPotion : FollowPlayer
{
    public float strengthReduceRate = 0.5f;

    public void Effect()
    {
        Debug.Log("ʹ��������ҩˮ");
        base.PlayAudioDrinkPotionAtPlayerPosition();
        base.IncreasePotionEffects(
            new (Type,float)[]
            {(typeof(StrengthPotionEffect), strengthReduceRate)}
            );
    }

    protected override void Update()
    {
        base.Update();
    }
}

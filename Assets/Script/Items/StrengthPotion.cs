using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StrengthPotion : FollowPlayer
{
    public float strengthAddRate = 0.5f;

    public void Effect()
    {
        Debug.Log("ʹ��������ҩˮ");
        base.PlayAudioDrinkPotionAtPlayerPosition();
        base.IncreasePotionEffects(
            new (Type, float)[]
            {(typeof(StrengthPotionEffect), strengthAddRate)}
            );
    }

    protected override void Update()
    {
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlownessPotion : FollowPlayer
{
    public float speedReduceRate = -0.5f;   

    public void Effect()
    {
        Debug.Log("ʹ���˻���ҩˮ");
        base.PlayAudioDrinkPotionAtPlayerPosition();
        base.IncreasePotionEffects(
            new (Type, float)[]
            {(typeof(SpeedPotionEffect), speedReduceRate)}
            );


    }

    protected override void Update()
    {
        base.Update();
    }

}

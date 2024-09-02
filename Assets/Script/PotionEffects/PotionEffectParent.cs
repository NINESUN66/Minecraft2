using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectParent : MonoBehaviour
{
    public float onceDurationTime = 5.0f;

    protected float maxDurationTime = 5.0f;
    protected float nowDurationTime = 0f;
    protected float originDurationTime = 0f;

    public void UseSamePotition()
    {
        maxDurationTime += onceDurationTime;
        Debug.Log("药水叠加使用");
    }

    public virtual void UseOppsitePotition()
    {
        Debug.Log("药水抵消使用");
    }

}

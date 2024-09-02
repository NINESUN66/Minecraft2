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
        Debug.Log("ҩˮ����ʹ��");
    }

    public virtual void UseOppsitePotition()
    {
        Debug.Log("ҩˮ����ʹ��");
    }

}

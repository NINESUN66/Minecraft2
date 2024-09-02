using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float AtrractionRadius = 1.0f;
    public float AtrractionSpeed = 1.0f;

    protected void PlayAudioDrinkPotionAtPlayerPosition()
    {
        AudioClip drink = Resources.Load<AudioClip>("Audio/Drink");
        AudioSource.PlayClipAtPoint(drink, GameObject.Find("Player").transform.position);
    }

    protected void FollowPlayerPosition(float AtrractionRadius, float AtrractionSpeed)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < AtrractionRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, AtrractionSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 使用药水
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="effect"></param>
    /// <param name="rate"></param>
    /// <param name="getRate"></param>
    /// <param name="updateRate"></param>
    /// <param name="useSamePotion"></param>
    /// <param name="useOppositePotion"></param>

    protected void HandlePotionEffect<T>(T effect, float rate, Func<T, float> getRate, Action<T, float> updateRate, Action<T> useSamePotion, Action<T> useOppositePotion) where T : PotionEffectParent
    {
        if (!effect.enabled)
        {
            effect.enabled = true;
            updateRate(effect, getRate(effect) + rate);
        }
        else
        {
            if (getRate(effect) < 0)
            {
                useSamePotion(effect);
            }
            else
            {
                useOppositePotion(effect);
            }
        }
    }

    /// <summary>
    /// 药水使用效果
    /// </summary>
    /// <param name="effects"></param>

    public void IncreasePotionEffects((Type type, float rate)[] effects)
    {
        GameObject effectManager = GameObject.Find("EffectManager");
        if (effectManager == null) {
            Debug.LogError("EffectManager为空");
            return;
        }
        else if (effects.Length <= 0) {
            Debug.LogError("没有效果");
            return;
        }
        else
        {
            foreach (var effectPair in effects)
            {
                var effectType = effectPair.type;
                var rate = effectPair.rate;

                if (effectType == typeof(SpeedPotionEffect))
                {
                    var effect = effectManager.GetComponent<SpeedPotionEffect>();
                    HandlePotionEffect(
                        effect,
                        rate,
                        e => e.speedRate,
                        (e, r) => e.speedRate = r,
                        e => e.UseSamePotition(),
                        e => e.UseOppsitePotition()
                    );
                }
                else if (effectType == typeof(StrengthPotionEffect))
                {
                    var effect = effectManager.GetComponent<StrengthPotionEffect>();
                    HandlePotionEffect(
                        effect,
                        rate,
                        e => e.strengthRate,
                        (e, r) => e.strengthRate = r,
                        e => e.UseSamePotition(),
                        e => e.UseOppsitePotition()
                    );
                }
                else if (effectType == typeof(DamageRatioEffect))
                {
                    var effect = effectManager.GetComponent<DamageRatioEffect>();
                    HandlePotionEffect(
                        effect,
                        rate,
                        e => e.damageRate,
                        (e, r) => e.damageRate = r,
                        e => e.UseSamePotition(),
                        e => e.UseOppsitePotition()
                    );
                }
            }

        }
    }
    protected virtual void Update()
    {
        FollowPlayerPosition(AtrractionRadius, AtrractionSpeed);
    }
}   

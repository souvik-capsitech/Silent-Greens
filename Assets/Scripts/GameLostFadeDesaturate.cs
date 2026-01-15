using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GameLostFadeDesaturate : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup panelGroup;
    public float panelFadeDuration = 0.35f;

    [Header("Effect")]
    public Volume postProcessVolume;
    public float desaturateAmount = -80f;
    public float exposureDarken = -0.5f;
    public float effectDuration = 0.5f;

    ColorAdjustments color;
    float baseSaturation;
    float baseExposure;

    void Awake()
    {
        panelGroup.alpha = 0f;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;

        postProcessVolume.profile.TryGet(out color);
    }

    void OnEnable()
    {

        baseSaturation = color.saturation.value;
        baseExposure = color.postExposure.value;

        StopAllCoroutines();
        StartCoroutine(ApplyLossEffect());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        RestoreImmediately();
    }

    IEnumerator ApplyLossEffect()
    {
        float t = 0f;

        while (t < effectDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / effectDuration;

            color.saturation.value = Mathf.Lerp(baseSaturation, desaturateAmount, p);
            color.postExposure.value = Mathf.Lerp(baseExposure, exposureDarken, p);

            yield return null;
        }

 
        t = 0f;
        while (t < panelFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            panelGroup.alpha = Mathf.Lerp(0f, 1f, t / panelFadeDuration);
            yield return null;
        }

        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;
    }

    void RestoreImmediately()
    {
        color.saturation.value = baseSaturation;
        color.postExposure.value = baseExposure;
        panelGroup.alpha = 0f;
    }
}

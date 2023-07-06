using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public void ShakeVCam(float amp, float freq, float time)
    {
        StopAllCoroutines();
        IEnumerator shake = Shake(amp, freq, time);
        StartCoroutine(shake);
    }

    private IEnumerator Shake(float intensity, float freq, float time)
    {
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_FrequencyGain = freq;

        float startAmp = intensity;
        float endAmp = 0.5f;
        float timer = 0f;
        float lerp = 0f;


        while (lerp < 1f)
        {
            noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            lerp = timer / time;
            noise.m_AmplitudeGain = Mathf.Lerp(startAmp, endAmp, lerp);
            timer += Time.deltaTime;

            yield return null;
        }

        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_FrequencyGain = 0.3f;
    }
}
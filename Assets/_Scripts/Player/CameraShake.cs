using System.Collections;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera _cam;
    CinemachineBasicMultiChannelPerlin _noise;

    void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    IEnumerator ShakeRoutine(float intensity, float duration)
    {
        _noise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        _noise.m_AmplitudeGain = 0f;
    }
}

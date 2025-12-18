using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFeedbackUI : MonoBehaviour
{
    [SerializeField] Image overlayImage;
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] float maxAlpha = 0.4f;

    Coroutine _flashRoutine;

    void Awake()
    {
        if (!overlayImage)
            overlayImage = GetComponent<Image>();
    }

    public void PlayDamageFlash()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);

        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color c = overlayImage.color;
        c.a = maxAlpha;
        overlayImage.color = c;

        yield return new WaitForSeconds(flashDuration);

        c.a = 0f;
        overlayImage.color = c;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressureSliderUI : MonoBehaviour
{
    [SerializeField] Slider pressureSlider;
    [SerializeField] Image fillImage;
    [SerializeField] Color lowPressureColor = Color.green;
    [SerializeField] Color highPressureColor = Color.red;
    
    [SerializeField] TMP_Text hintText;

    private float _pressure;
    void Start()
    {
        if (!pressureSlider)
            pressureSlider = GetComponent<Slider>();

        pressureSlider.minValue = 0;
        pressureSlider.maxValue = 100;
    }

    void Update()
    {
        if (!BattlePressureManager.Instance) return;
        
        pressureSlider.value = BattlePressureManager.Instance.currentPressure;
        fillImage.color = Color.Lerp(lowPressureColor, highPressureColor, pressureSlider.value / pressureSlider.maxValue);
        _pressure = BattlePressureManager.Instance.currentPressure;

        if (_pressure < 30)
            hintText.text = "Zona estable";
        else if (_pressure < 70)
            hintText.text = "Combate intenso";
        else
            hintText.text = "Enemigos agresivos";
    }
}

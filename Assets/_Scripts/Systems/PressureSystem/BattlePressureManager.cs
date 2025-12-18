using System;
using UnityEngine;

public class BattlePressureManager : MonoBehaviour
{
    public static BattlePressureManager Instance;

    [Header("Pressure Settings")]
    [Range(0, 100)] public float currentPressure = 0f;
    public float maxPressure = 100f;
    public float minPressure = 0f;

    [Header("Decay")]
    [SerializeField] float passiveDecayPerSecond = 2f;

    public event Action OnPressureHigh;
    public event Action OnPressureLow;

    const float HIGH_THRESHOLD = 70f;
    const float LOW_THRESHOLD = 30f;

    bool _wasHigh;
    bool _wasLow;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        ModifyPressure(-passiveDecayPerSecond * Time.deltaTime);
        CheckThresholds();
    }

    public void ModifyPressure(float amount)
    {
        currentPressure = Mathf.Clamp(currentPressure + amount, minPressure, maxPressure);
    }

    void CheckThresholds()
    {
        bool isHigh = currentPressure >= HIGH_THRESHOLD;
        bool isLow = currentPressure <= LOW_THRESHOLD;

        if (isHigh && !_wasHigh)
            OnPressureHigh?.Invoke();

        if (isLow && !_wasLow)
            OnPressureLow?.Invoke();

        _wasHigh = isHigh;
        _wasLow = isLow;
    }
}

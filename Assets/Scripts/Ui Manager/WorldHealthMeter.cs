using UnityEngine;
using UnityEngine.UI;

public class WorldHealthMeter : MonoBehaviour
{
    public GameObject Panel;
    public float MaxHealth;
    public float HealthLossPerSecond;
    public float HealthBarUpdateFrequency;
    public Image HealthBar;
    private float _currentHealth;
    private float _timePassed;
    private float _healthLossPerTick;
    private bool _isSystemActive; public bool IsSystemActive => _isSystemActive;

    public void Initialize()
    {
        _currentHealth = MaxHealth;
        _healthLossPerTick = HealthLossPerSecond * HealthBarUpdateFrequency;

        UpdateVisual();
    }

    public void SetTimerState(bool state)
    {
        if(!state)
            _isSystemActive = state;
        else
            _isSystemActive = state;

        ResetTimer();
    }

    public void SetSystemIsEnabled(bool state)
    {
        _isSystemActive = state;
        ResetTimer();
        SetPanelState(state);
    }

    public void SetHealth(float health) => _currentHealth = health;

    public void GainHealth(float health)
    {
        _currentHealth += health;
        UpdateVisual();
    }

    public void LoseHealth(float health)
    {
        _currentHealth -= health;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        HealthBar.fillAmount = _currentHealth / MaxHealth;
    }

    void SetPanelState(bool state) => Panel.SetActive(state);

    void Timer()
    {
        if(!_isSystemActive) return;

        _timePassed += Time.deltaTime;

        if(_timePassed >= HealthBarUpdateFrequency)
        {
            _currentHealth -= _healthLossPerTick;
            UpdateVisual();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        _timePassed = 0;
    }

    void ResetHealth()
    {
        _currentHealth = MaxHealth;
        UpdateVisual();
    }

    void Update()
    {
        Timer();
    }
}
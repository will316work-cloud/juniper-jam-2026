using UnityEngine;
using UnityEngine.UI;

public class WorldHealthMeter : MonoBehaviour
{
    public GameObject Panel;
    public float MaxHealth;
    public float HealthBarUpdateFrequency;
    public Image HealthBar;
    private float _currentHealth;
    private float _timePassed;
    private float _healthLossPerTick;
    private float _healthLossPerSecond;
    private bool _isSystemActive; public bool IsSystemActive => _isSystemActive;
    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _currentHealth = MaxHealth;
        _ctx = ctx;
        UpdateVisual();
    }

     /// <summary>
    /// Disables the timer.
    /// </summary>
    public void SetTimerState(bool state)
    {
        if(!state)
            _isSystemActive = state;
        else
            _isSystemActive = state;

        ResetTimer();
    }

    /// <summary>
    /// Disables the timer and the panel.
    /// </summary>
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
        if(_currentHealth > MaxHealth) _currentHealth = MaxHealth;
        UpdateVisual();
    }

    public void LoseHealth(float health)
    {
        _currentHealth -= health;
        if(_currentHealth < 0 && _ctx.GameStateController.IsPlayerDead == false)
        {
            _ctx.GameStateController.IsPlayerDead = true;
            _currentHealth = 0;
            _ctx.GameStateController.ChangeState(StateType.GameOver);
        }

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
            LoseHealth(_healthLossPerTick);
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        _timePassed = 0;
    }

    public void ResetHealth()
    {
        _currentHealth = MaxHealth;
        UpdateVisual();
    }

    void Update()
    {
        Timer();
    }

    public void SetHealthLossPerSecond(float healthLossAmount)
    {
        _healthLossPerSecond = healthLossAmount;
        _healthLossPerTick = _healthLossPerSecond * HealthBarUpdateFrequency;
    }
}
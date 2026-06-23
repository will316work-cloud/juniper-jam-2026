using System;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenuHandler : IUiHandler
{
    private GameObject _panel;

    private Button _mainMenuButton;
    private Button _restartButton;

    [Range(0,100)] private Slider _masterVolumeSlider;
    [Range(0,100)] private Slider _sfxVolumeSlider;
    [Range(0,100)] private Slider _musicVolumeSlider;
    private Slider _fpsSlider;

    private Toggle _vsyncToggle;
    private Dropdown _resolutionDropdown;

    GameContext _ctx;

    public bool IsPanelActive() => _panel.activeSelf;
    public void SetPanelState(bool state) => _panel.SetActive(state);

    public void Initialize(GameContext ctx, IngameMenuHandlerData data)
    {
        _ctx = ctx;

        _panel = data.Panel;
        _mainMenuButton = data.MainMenuButton;
        _restartButton = data.RestartButton;

        _masterVolumeSlider = data.MasterVolumeSlider;
        _sfxVolumeSlider = data.SfxVolumeSlider;
        _musicVolumeSlider = data.MusicVolumeSlider;
        _fpsSlider = data.FpsSlider;
        _vsyncToggle = data.VsyncToggle;
        _resolutionDropdown = data.ResolutionDropdown;

        _mainMenuButton.onClick.AddListener(() => MainMenuButtonClickHandler());
        _restartButton.onClick.AddListener(() => RestartButtonClickHandler());

    }

    private void RestartButtonClickHandler()
    {
        
    }

    private void MainMenuButtonClickHandler()
    {
        // _ctx.GameStateController.ChangeState(StateType.MainMenu);
    }

    void HandleVsyncToggleValueChange()
    {
        if(_vsyncToggle.isOn) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }
    void HandleMasterVolumeChange()
    {
        float volume;

        if(_masterVolumeSlider.value == 0) 
            volume = 0;
         else
            volume = _masterVolumeSlider.value = 100;

        _ctx.PoolManager.OnMasterVolumeChange(volume);
    }
}

[Serializable]
public class IngameMenuHandlerData
{
    public GameObject Panel;
    public Button MainMenuButton;
    public Button RestartButton;

    public Slider MasterVolumeSlider;
    public Slider SfxVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider FpsSlider;

    public Toggle VsyncToggle;
    public Dropdown ResolutionDropdown;
}
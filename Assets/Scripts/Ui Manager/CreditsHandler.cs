using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsHandler : IUiHandler
{
    private GameObject _panel;
    private Button _backButton;

    private GameContext _ctx;

    public void Initialize(GameContext ctx, CreditsHandlerData data)
    {
        _ctx = ctx;

        _panel = data.CreditsPanel;
        _backButton = data.BackButton;

        _backButton.onClick.AddListener(BackButtonHandler);

        SetPanelState(false);
    }

    public void SetPanelState(bool state)
    {
        _panel.SetActive(state);
    }

    public bool IsPanelActive()
    {
        return _panel.activeSelf;
    }

    private void BackButtonHandler()
    {
        _ctx.UiManager.StartCoroutine(BackButtonHandlerRoutine());
    }
    private IEnumerator BackButtonHandlerRoutine()
    {
        SetPanelState(false);
        _ctx.CameraController.SwitchToCamera(CameraType.Menu);
        yield return new WaitForSeconds(1.1f);
        _ctx.UiManager.MainMenuHandler.SetPanelState(true);
    }
}

[System.Serializable]
public class CreditsHandlerData
{
    public GameObject CreditsPanel;
    public Button BackButton;
}
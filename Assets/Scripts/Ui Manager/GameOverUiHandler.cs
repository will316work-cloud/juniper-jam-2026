using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUiHandler : IUiHandler
{
    GameObject _panel;
    TextMeshProUGUI _scoreText;
    Button _restartButton;
    Button _mainMenuButton;
    GameContext _ctx;

    public void Initialize(GameContext ctx, GameOverUiHandlerData data)
    {
        _ctx = ctx;

        _panel = data.Panel;
        _scoreText = data.ScoreText;
        _restartButton = data.RestartButton;
        _mainMenuButton = data.MainMenuButton;

        SetPanelState(false);
    }

    public bool IsPanelActive() => _panel.activeSelf;
    public void SetPanelState(bool state)
    {
        _scoreText.text = $"Score: {_ctx.MoneyController.CurrentMoney()}";
        _panel.SetActive(state);
    }
}
 
[System.Serializable]
public class GameOverUiHandlerData
{
    public GameObject Panel;
    public TextMeshProUGUI ScoreText;
    public Button RestartButton;
    public Button MainMenuButton;
}
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class ScoreBoardView
{ 
    private GameObject _scorePanel;
    private List<ScoreBoardEntry> _scoreTextPairs = new();
    Button _backToMenuButton;
    TextMeshProUGUI _loadingText;

    public ScoreBoardView(ScoreBoardUiData data, GameContext ctx)
    {
        _scorePanel = data.ScorePanel;
        _backToMenuButton = data.MenuButton;
        _loadingText = data.LoadingText;
        _backToMenuButton.onClick.AddListener(()=> OnScoreClose(ctx));

        CreateEntries(data.ScoreEntryPrefab, data.ScoreEntryParent);
    }

    public void HidePanel() => _scorePanel.SetActive(false);
    public void ShowPanel() => _scorePanel.SetActive(true);

    public void ShowLoadingText() => _loadingText.gameObject.SetActive(true);
    public void HideLoadingText() => _loadingText.gameObject.SetActive(false);

    void CreateEntries(GameObject prefab, Transform parent)
    {
        for(int i = 0; i < 10; i++)
        {
            ScoreBoardEntry entry = GameObject.Instantiate(prefab,parent).GetComponent<ScoreBoardEntry>();
            _scoreTextPairs.Add(entry);
        }
    }

    void SetScores(ScoresArray scores)
    {
        for(int i = 0; i < _scoreTextPairs.Count; i++)
        {
            if(scores.scores.Length <= i)
            {
                _scoreTextPairs[i].ClearEntry();
                continue;
            }

            _scoreTextPairs[i].SetEntry(scores.scores[i].name, scores.scores[i].score.ToString());
        }
    }

    public void HideEntries()
    {
        for(int i = 0; i < _scoreTextPairs.Count; i++)
        {
            _scoreTextPairs[i].ClearEntry();
        }
    }

    public void OnScoreOpen(ScoresArray scores)
    {
        SetScores(scores);
        HideLoadingText();
    }

    public void OnScoreClose(GameContext ctx)
    {
        _scorePanel.SetActive(false);
        ctx.UiManager.MainMenuHandler.SetPanelState(true);
    }
}

[System.Serializable]
public class ScoreBoardUiData
{
    public GameObject ScorePanel;
    public GameObject ScoreEntryPrefab;
    public Transform ScoreEntryParent;
    public Button MenuButton;
    public TextMeshProUGUI LoadingText;
}
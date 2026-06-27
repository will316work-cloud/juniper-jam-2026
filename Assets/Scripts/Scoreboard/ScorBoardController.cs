using System.Collections;
using UnityEngine;

public class ScoreBoardController
{
    ScoreBoardView _scoreBoardView;
    ScoreBoardModel _model;
    MonoBehaviour _monoBehaviour;

    public void Initialize(ScoreBoardUiData data,MonoBehaviour monoBehaviour, GameContext ctx)
    {
        _monoBehaviour = monoBehaviour;
        _scoreBoardView = new ScoreBoardView(data, ctx);
        _model = new ScoreBoardModel();

        HidePanel();
    }
    public void ShowPanel()
    {
        _scoreBoardView.ShowPanel();
    }
    public void HidePanel()
    {
        _scoreBoardView.HidePanel();
    }

    public void OpenScoreboard() => _monoBehaviour.StartCoroutine(OpenScoreBoardRoutine());

    public IEnumerator OpenScoreBoardRoutine()
    {
        _scoreBoardView.HideEntries();
        _scoreBoardView.ShowPanel();
        _scoreBoardView.ShowLoadingText();
        yield return _model.GetScores();
        _scoreBoardView.OnScoreOpen(_model.Scores);
    }

    public void PostScore(string name, int score)
    {
        _monoBehaviour.StartCoroutine(_model.PostScore(name, score));
    }
}
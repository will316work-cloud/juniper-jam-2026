using TMPro;
using UnityEngine;

public class ScoreBoardEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI ScoreText;

    public void SetEntry(string name, string score)
    {
        NameText.text = name;
        ScoreText.text = score;

        if(gameObject.activeSelf == false)
            EnableEntry();
    }
    public void ClearEntry()
    {
        SetEntry("", "");
        DisableEntry();
    }
    public void DisableEntry() => gameObject.SetActive(false);
    public void EnableEntry() => gameObject.SetActive(true);
}
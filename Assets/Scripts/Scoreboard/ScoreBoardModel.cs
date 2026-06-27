using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreBoardModel
{
    private string _url = "https://script.google.com/macros/s/AKfycbxsjRIwqoDNDflDKDK2ahIXt7YfdFgZ1Xht7RAO6rW8QbBm6XUfwdHDVa7jUgfbaequ1A/exec";

    public IEnumerator PostScore(string name, int score)
    {
        ScoreData data = new(name,score);
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(_url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ScoresArray array = JsonUtility.FromJson<ScoresArray>(request.downloadHandler.text);
            ScoresArray topTen = GetTopTenInOrder(array);

            for(int i = 0; i < topTen.scores.Length; i++)
            {
                if(topTen.scores[i] == null) continue;
                Debug.Log(topTen.scores[i].name + " " + topTen.scores[i].score);
            }            
        }
    }

    public ScoresArray GetTopTenInOrder(ScoresArray array)
    {
        ScoresArray arr = new ScoresArray();

        ScoresArray bestScores = new ScoresArray();
        arr.scores = array.scores;
    
        int limit = Mathf.Min(10,arr.scores.Length);
        bestScores.scores = new ScoreData[limit];

        int highestScore = 0;
        int highestScoreIndex = 0;

        for(int i = 0; i < limit; i++)
        {
            for(int j = 0; j < arr.scores.Length; j++)
            {
                if(arr.scores[j] == null) continue;

                if(arr.scores[j].score > highestScore)
                {
                    highestScore = arr.scores[j].score;
                    highestScoreIndex = j;
                }
            }

            bestScores.scores[i] = arr.scores[highestScoreIndex];
            arr.scores[highestScoreIndex] = null;
            highestScore = 0;
        }

        return bestScores;
    }

    public ScoresArray Scores { get; private set; }

    public IEnumerator GetScores()
    {
        UnityWebRequest request = UnityWebRequest.Get(_url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            yield break;
        }

        ScoresArray array = JsonUtility.FromJson<ScoresArray>(request.downloadHandler.text);
        Scores = GetTopTenInOrder(array);
    }
}

[System.Serializable]
public class ScoreData
{
    public string name;
    public int score;

    public ScoreData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
public class ScoresArray
{
    public ScoreData[] scores;
}
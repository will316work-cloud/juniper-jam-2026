using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] GameObject _transitionPanel;
    [SerializeField] TextMeshProUGUI _transitionText;
    [SerializeField] Image _transitionImage;
    [SerializeField] float _baseStillTime = 4;
    [SerializeField] float _baseFadeTime = 2;
    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _transitionText.color = new(255, 255, 255, 0);
        _transitionImage.color = new(0, 0, 0, 0);
        _transitionPanel.SetActive(false);
        
        _ctx = ctx;
    }

    public void StartTransition(string text, float fadeTime = 3, float stillTime = 4, bool startTimeAtEnd = true)
    {
        StartCoroutine(TransitionRoutine(text, fadeTime, stillTime, startTimeAtEnd));
    }

    IEnumerator TransitionRoutine(string text, float fadeTime, float stillTime,  bool startTimeAtEnd = true)
    {
        _transitionText.text = text;
        _transitionPanel.SetActive(true);
        _transitionImage.DOFade(1, fadeTime / 2).OnComplete(()=> _transitionText.DOFade(1, fadeTime / 2));
        yield return new WaitForSeconds(stillTime/2 + fadeTime / 2);
        _transitionText.DOFade(0, fadeTime / 2);
        yield return new WaitForSeconds(stillTime/2);
        _transitionImage.DOFade(0, fadeTime / 2).OnComplete(() => _transitionPanel.SetActive(false));
    }

    public IEnumerator TransitionFadeIn(string text)
    {
        _transitionText.text = text;
        _transitionPanel.SetActive(true);

        _transitionImage.DOFade(1, 1).OnComplete(()
            => _transitionText.DOFade(1, 1f));

        yield return new WaitForSeconds(2f);             
    }

    public IEnumerator TransitionFadeOut()
    {
        _transitionText.DOFade(0, 1f).OnComplete(()
                =>_transitionImage.DOFade(0, 2f).OnComplete(() 
                => _transitionPanel.SetActive(false)));

        yield return new WaitForSeconds(3f);                                
    }
}

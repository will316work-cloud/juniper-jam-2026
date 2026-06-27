using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignalHandler : MonoBehaviour
{
    public AudioSource BoomSound_01;
    public AudioSource BoomSound_02;
    public AudioSource BoomSound_03;
    public AudioSource BoomSound_04;
    public AudioSource BoomSound_05;
    public AudioSource Music;
    public AudioSource Ambient;

    public List<string> Texts = new();
    public TextMeshProUGUI _text;
    public TextMeshProUGUI _skipIntroText;
    Coroutine _skipHandlerRoutine;
    Tween _skipIntroTween;
    
    int _textIndex = 0;

    bool _canSkipIntro = false;

    void Start()
    {
        Ambient.loop = true;   

        _skipIntroText.text = "Press any key to skip intro..";
        _skipHandlerRoutine = StartCoroutine(SkipHandler());
    }

    void Update()
    {
        if(!_canSkipIntro) return;

        if(Keyboard.current.anyKey.wasPressedThisFrame)
        {
            _canSkipIntro = false;
            GoToNextScene(); 
        }      
    }

    IEnumerator SkipHandler()
    {
        yield return new WaitForSeconds(6f);
        _canSkipIntro = true;
        _skipIntroTween = _skipIntroText.DOFade(1, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(6f);
        _skipIntroTween.Kill();
        _skipIntroText.DOFade(0, 1f);
    }

    public void PlayBoomSound_01() => BoomSound_01.Play();
    public void PlayBoomSound_02() => BoomSound_02.Play();
    public void PlayBoomSound_03() => BoomSound_03.Play();
    public void PlayBoomSound_04() => BoomSound_04.Play();
    public void PlayBoomSound_05() => BoomSound_05.Play();

    public void PlayMusic() => Music.Play();
    public void PlayAmbient() => Ambient.Play();

    public void FadeInText()
    {
        _text.text = Texts[_textIndex];
        _text.DOFade(1, 1f);
        _textIndex++;
    }
    public void ChangeText()
    {
        FadeOutText().OnComplete(() => FadeInText());
    }
    public Tween FadeOutText()
    {
        return _text.DOFade(1, 0f);
    }
    public void FadeTextOut()
    {
        _text.DOFade(0, 1f);
    }

    public void GoToNextScene()
    {
        if(_skipHandlerRoutine != null) StopCoroutine(_skipHandlerRoutine);
        _skipIntroTween.Kill();
        _skipIntroText.DOKill();
        BoomSound_01.Stop();
        BoomSound_02.Stop();
        Music.Stop();
        Ambient.Stop();
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

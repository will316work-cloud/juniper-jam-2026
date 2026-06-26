using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignalHandler : MonoBehaviour
{
    public AudioSource BoomSound;
    public AudioSource Music;
    public AudioSource Ambient;

    public List<string> Texts = new();
    public TextMeshProUGUI _text;
    public TextMeshProUGUI _skipIntroText;
    
    int _textIndex = 0;

    bool _canSkipIntro = false;

    void Start()
    {
        Ambient.loop = true;   

        _skipIntroText.text = "Press any key to skip intro..";
        StartCoroutine(SkipHandler());
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
        yield return new WaitForSeconds(2f);
        _canSkipIntro = true;
        _skipIntroText.DOFade(1, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }



    public void PlayBoomSound() => BoomSound.Play();
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
        _skipIntroText.DOKill();
        BoomSound.Stop();
        Music.Stop();
        Ambient.Stop();
        

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SignalHandler : MonoBehaviour
{
    public AudioSource BoomSound;
    public AudioSource Music;
    public AudioSource Ambient;

    public List<string> Texts = new();
    public TextMeshProUGUI _text;
    int _textIndex = 0;

    void Start()
    {
        Ambient.loop = true;   
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

    public void GoToNextScene() => UnityEngine.SceneManagement.SceneManager.LoadScene(1);
}

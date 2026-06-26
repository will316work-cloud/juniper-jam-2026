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

    public Sprite _healthyPlanet;
    public Sprite _unhealthyPlanet;
    public Sprite _collage;

    public List<string> Texts = new();
    public TextMeshProUGUI _text;
    int _textIndex = 0;

    public void PlayBoomSound() => BoomSound.Play();
    public void PlayMusic() => Music.Play();
    public void PlayAmbient() => Ambient.Play();


    public void ShowText()
    {
        _text.text = Texts[_textIndex];
        StartCoroutine(FadeHandler());
        _textIndex++;
    }

    IEnumerator FadeHandler()
    {
        _text.DOFade(1, 1f);
        yield return new WaitForSeconds(2f);
        _text.DOFade(0, 1f);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler
{
    public AudioType AudioType = AudioType.UiClick;
    Button _button;
    SfxPooler _sfxPooler;

    public void Initialize(SfxPooler sfxPooler)
    {
        _sfxPooler = sfxPooler;

        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(() => ButtonClickHandler(_sfxPooler));
    }
    void ButtonClickHandler(SfxPooler sfxPooler) => sfxPooler.GetAudio(AudioType);

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sfxPooler.GetAudio(AudioType.ButtonHover);
    }
}
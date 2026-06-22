using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaperSheet : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Button SignButton;
    public GameObject XMark;
    public GameObject SignedMark;

    public bool IsSignable { get; private set; }
    public bool IsResolved { get; private set; }

    private PaperShredder _task;
    private RectTransform _rect;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    public void Initialize(PaperShredder task, bool signable)
    {
        _task = task;
        IsSignable = signable;

        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();

        XMark.SetActive(!signable);
        SignedMark.SetActive(false);

        SignButton.onClick.RemoveAllListeners();
        SignButton.onClick.AddListener(Sign);

        _rect.anchoredPosition = new Vector2(Random.Range(-250, 250), Random.Range(-150, 150));
    }

    public void Sign()
    {
        if (IsResolved) return;

        if (!IsSignable) { _task.FailTask(); return; }

        IsResolved = true;
        SignedMark.SetActive(true);
        SignButton.interactable = false;
        _task.ResolvePaper();
    }

    public void Shred()
    {
        if (IsResolved) return;

        if (IsSignable) { _task.FailTask(); return; }

        IsResolved = true;
        _task.ResolvePaper();
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }
}
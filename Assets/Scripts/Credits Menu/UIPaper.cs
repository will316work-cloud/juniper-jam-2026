using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
public class UIPaper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private bool started;
    private float currTime;
    public bool IsHolding;
    public bool Opened;
    private bool isAnimating;

    private Vector2 dragOffset;

    [Header("Audio")]
    //public AudioSource AudioSource;

    [Header("Profile")]
    public Image ProfileImage;
    public Sprite ProfileSprite;

    [Header("Texts")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI RoleText;
    public TextMeshProUGUI QuoteText;

    public string PaperName;
    public string PaperRole;

    [TextArea(2, 5)]
    public string PaperQuote;

    [Header("Website")]
    public string WebsiteURL;

    [Header("Animation")]
    public Vector3 OpenScale = new Vector3(2f, 2f, 1f);
    public Vector3 ClosedScale = new Vector3(0.8f, 0.8f, 1f);

    [Header("Drag Bounds")]
    public float MinX = -500f;
    public float MaxX = 500f;
    public float MinY = -300f;
    public float MaxY = 300f;

    public float OpenRotation = 0f;
    public float ClosedRotation = 7f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (ProfileImage != null) ProfileImage.sprite = ProfileSprite;
        if (NameText != null) NameText.text = PaperName;
        if (RoleText != null) RoleText.text = PaperRole;
        if (QuoteText != null) QuoteText.text = PaperQuote;
    }

    private void Update()
    {
        if (started)
        {
            currTime += Time.deltaTime;
            if (currTime >= 0.2f) IsHolding = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Opened) return;
        currTime = 0;
        started = true;

        rectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rectTransform.parent, eventData.position, eventData.pressEventCamera, out var localPoint);
        dragOffset = rectTransform.anchoredPosition - localPoint;

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Opened) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rectTransform.parent, eventData.position, eventData.pressEventCamera, out localPoint);

        Vector2 targetPos = localPoint + dragOffset;

        targetPos.x = Mathf.Clamp(targetPos.x, MinX, MaxX);
        targetPos.y = Mathf.Clamp(targetPos.y, MinY, MaxY);

        rectTransform.anchoredPosition = targetPos;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        started = false;
        if (!IsHolding)
        {
            if (Opened) ResetImage();
            else AnimateImage();
        }
        currTime = 0;
        IsHolding = false;

    }

    public void AnimateImage()
    {
        if (IsHolding || isAnimating) return;

        Opened = true;

        //AudioSource?.Play();

        StartCoroutine(SmoothTransform(OpenScale, OpenRotation, 0.3f));
    }

    public void ResetImage()
    {
        if (IsHolding || isAnimating) return;

        Opened = false;

        //AudioSource?.Play();

        StartCoroutine(SmoothTransform(ClosedScale, ClosedRotation, 0.3f));
    }

    public void OpenWebsite()
    {
        if (!string.IsNullOrWhiteSpace(WebsiteURL)) Application.OpenURL(WebsiteURL);
    }

    private IEnumerator SmoothTransform(Vector3 targetScale, float targetRotation, float duration)
    {
        isAnimating = true;

        Vector3 startScale = rectTransform.localScale;
        float startRotation = rectTransform.localEulerAngles.z;

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, targetRotation, t));

            time += Time.deltaTime;

            yield return null;
        }

        rectTransform.localScale = targetScale;
        rectTransform.localRotation = Quaternion.Euler(0, 0, targetRotation);

        isAnimating = false;
    }
}

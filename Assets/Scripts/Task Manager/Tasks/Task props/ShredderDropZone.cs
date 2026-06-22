using UnityEngine;
using UnityEngine.EventSystems;

public class ShredderDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var paper = eventData.pointerDrag?.GetComponent<PaperSheet>();

        if (paper != null)
            paper.Shred();
    }
}
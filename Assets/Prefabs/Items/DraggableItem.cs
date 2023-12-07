using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [HideInInspector] public Transform parentItemDrag;
    public Image image;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentItemDrag = transform.parent;
        transform.SetParent(transform.root.transform.root);
        transform.SetAsLastSibling();
        GetComponent<Image>().raycastTarget = false;
        transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(1).GetComponent<Image>().raycastTarget = false;
        GetComponent<TooltipTrigger>().tooltipEnabled = false;
        GetComponent<TooltipTrigger>().hideTooltip();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentItemDrag);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GetComponent<Image>().raycastTarget = true;
        transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        transform.GetChild(1).GetComponent<Image>().raycastTarget = true;
        GetComponent<TooltipTrigger>().tooltipEnabled = true;
        GetComponent<TooltipTrigger>().hideTooltip();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

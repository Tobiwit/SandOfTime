using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        BaseItem item = draggableItem.gameObject.GetComponent<BaseItemPrefab>().item;
        InventorySlot currentSlot = transform.parent.gameObject.GetComponent<InventorySlot>();
        switch (currentSlot.slotType)
        {
            case SlotType.NormalSlot:
                //if(item.itemType == )
                SwitchItemSLots(draggableItem);
                break;
            case SlotType.WeaponSlot:
                if(item.itemType == ItemType.Weapon) {
                    SwitchItemSLots(draggableItem);
                }
                break;
            case SlotType.EquipmentSlot:
                if(item.itemType == ItemType.Equipment) {
                    SwitchItemSLots(draggableItem);
                }
                break;
            case SlotType.ConsumableSlotOne:
                if(item.itemType == ItemType.Consumable) {
                    SwitchItemSLots(draggableItem);
                }
                break;
            case SlotType.ConsumableSlotTwo:
                if(item.itemType == ItemType.Consumable) {
                    SwitchItemSLots(draggableItem);
                }
                break;
            default:
                break;
        }
    }

    private void SwitchItemSLots(DraggableItem draggableItem)
    {
        Transform oldSlotTransform = draggableItem.parentItemDrag;
        draggableItem.parentItemDrag = transform.parent;
        parentItemDrag = oldSlotTransform;
        transform.SetParent(parentItemDrag);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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

    public void SwitchItemSlots() {
        
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

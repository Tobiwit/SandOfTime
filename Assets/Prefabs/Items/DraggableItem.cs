using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    [HideInInspector] public Transform parentItemDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentItemDrag = transform.parent;
        transform.SetParent(transform.root.transform.root);
        transform.SetAsLastSibling();
        GetComponent<Image>().raycastTarget = false;
        transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(1).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(2).GetComponent<Image>().raycastTarget = false;
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

        if(item.itemType == GetComponent<BaseItemPrefab>().item.itemType) {
            BaseItemPrefab itemToSwitch = draggableItem.gameObject.GetComponent<BaseItemPrefab>();
            BaseItemPrefab itemToBeSwitched = GetComponent<BaseItemPrefab>();
            if (itemToSwitch.belongingHero <= 10 && itemToBeSwitched.belongingHero <= 10) {
                // Nix
            } else if(itemToSwitch.belongingHero <= 10) {
                
                draggableItem.GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                draggableItem.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                switch (draggableItem.parentItemDrag.GetComponent<InventorySlot>().slotType)
                {
                    case SlotType.WeaponSlot:
                        HeroPageManager.Instance.selectedHero.weaponSlot = itemToSwitch.item;
                        break;
                    case SlotType.EquipmentSlot:
                        HeroPageManager.Instance.selectedHero.equipmentSlot = itemToSwitch.item;
                        break;
                    case SlotType.ConsumableSlotOne:
                        HeroPageManager.Instance.selectedHero.consumableSlotOne = itemToSwitch.item;
                        break;
                    case SlotType.ConsumableSlotTwo:
                        HeroPageManager.Instance.selectedHero.consumableSlotTwo = itemToSwitch.item;
                        break;
                }
                GetComponent<BaseItemPrefab>().belongingHero = 99;
                transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
                
            }
            else if (itemToBeSwitched.belongingHero <= 10) {

                GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                switch (parentItemDrag.GetComponent<InventorySlot>().slotType)
                {
                    case SlotType.WeaponSlot:
                        HeroPageManager.Instance.selectedHero.weaponSlot = itemToBeSwitched.item;
                        break;
                    case SlotType.EquipmentSlot:
                        HeroPageManager.Instance.selectedHero.equipmentSlot = itemToBeSwitched.item;
                        break;
                    case SlotType.ConsumableSlotOne:
                        HeroPageManager.Instance.selectedHero.consumableSlotOne = itemToBeSwitched.item;
                        break;
                    case SlotType.ConsumableSlotTwo:
                        HeroPageManager.Instance.selectedHero.consumableSlotTwo = itemToBeSwitched.item;
                        break;
                }
                draggableItem.gameObject.GetComponent<BaseItemPrefab>().belongingHero = 99;
                draggableItem.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
            }
            SwitchItemSlots(draggableItem);

        }
        else {
            if (currentSlot.slotType == SlotType.NormalSlot && draggableItem.parentItemDrag.GetComponent<InventorySlot>().slotType == SlotType.NormalSlot) {
                SwitchItemSlots(draggableItem);
            }
        }
    }

    private void SwitchItemSlots(DraggableItem draggableItem)
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
        transform.GetChild(2).GetComponent<Image>().raycastTarget = true;
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

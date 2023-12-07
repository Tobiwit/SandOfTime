using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum SlotType {
    NormalSlot,
    WeaponSlot,
    EquipmentSlot,
    ConsumableSlotOne,
    ConsumableSlotTwo

}

public class InventorySlot : MonoBehaviour, IDropHandler
{

    public SlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        BaseItem item = draggableItem.gameObject.GetComponent<BaseItemPrefab>().item;
        switch (slotType)
        {
            case SlotType.NormalSlot:
                draggableItem.parentItemDrag = transform;
                break;
            case SlotType.WeaponSlot:
                if(item.itemType == ItemType.Weapon) {
                    draggableItem.parentItemDrag = transform;
                }
                break;
            case SlotType.EquipmentSlot:
                if(item.itemType == ItemType.Equipment) {
                    draggableItem.parentItemDrag = transform;
                }
                break;
            case SlotType.ConsumableSlotOne:
                if(item.itemType == ItemType.Consumable) {
                    draggableItem.parentItemDrag = transform;
                }
                break;
            case SlotType.ConsumableSlotTwo:
                if(item.itemType == ItemType.Consumable) {
                    draggableItem.parentItemDrag = transform;
                }
                break;
            default:
                break;
        }

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

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

        if(0 <= dropped.GetComponent<BaseItemPrefab>().belongingHero && dropped.GetComponent<BaseItemPrefab>().belongingHero <= 10) {
            switch (draggableItem.parentItemDrag.GetComponent<InventorySlot>().slotType)
            {
                case SlotType.NormalSlot:
                    Debug.Log("You Coded Wrong");
                    break;
                case SlotType.WeaponSlot:
                    HeroPageManager.Instance.selectedHero.weaponSlot = null;
                    Debug.Log("Removed Item");
                    break;
                case SlotType.EquipmentSlot:
                    HeroPageManager.Instance.selectedHero.equipmentSlot = null;
                    Debug.Log("Removed Item");
                    break;
                case SlotType.ConsumableSlotOne:
                    HeroPageManager.Instance.selectedHero.consumableSlotOne = null;
                    Debug.Log("Removed Item");
                    break;
                case SlotType.ConsumableSlotTwo:
                    HeroPageManager.Instance.selectedHero.consumableSlotTwo= null;
                    Debug.Log("Removed Item");
                    break;
                default:
                    Debug.Log("ERROR");
                    break;
            }
            dropped.GetComponent<BaseItemPrefab>().belongingHero = 99;
            dropped.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
        }

        switch (slotType)
        {
            case SlotType.NormalSlot:
                draggableItem.parentItemDrag = transform;
                break;
            case SlotType.WeaponSlot:
                if(item.itemType == ItemType.Weapon) {
                    draggableItem.parentItemDrag = transform;
                    HeroPageManager.Instance.selectedHero.weaponSlot = item;
                    dropped.GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                    dropped.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                    Debug.Log("Added Item");
                }
                break;
            case SlotType.EquipmentSlot:
                if(item.itemType == ItemType.Equipment) {
                    draggableItem.parentItemDrag = transform;
                    HeroPageManager.Instance.selectedHero.equipmentSlot = item;
                    dropped.GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                    dropped.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                    Debug.Log("Added Item");
                }
                break;
            case SlotType.ConsumableSlotOne:
                if(item.itemType == ItemType.Consumable) {
                    draggableItem.parentItemDrag = transform;
                    HeroPageManager.Instance.selectedHero.consumableSlotOne = item;
                    dropped.GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                    dropped.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                    Debug.Log("Added Item");
                }
                break;
            case SlotType.ConsumableSlotTwo:
                if(item.itemType == ItemType.Consumable) {
                    draggableItem.parentItemDrag = transform;
                    HeroPageManager.Instance.selectedHero.consumableSlotTwo = item;
                    dropped.GetComponent<BaseItemPrefab>().belongingHero = (int) HeroManager.Instance.TranslateStringTypeToUnitType(HeroPageManager.Instance.selectedHero.type);
                    dropped.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                    Debug.Log("Added Item");
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

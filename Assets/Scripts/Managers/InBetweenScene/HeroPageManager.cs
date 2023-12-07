using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPageManager : MonoBehaviour
{
    public static HeroPageManager Instance;

    public GameObject _inventoryBox;

    
    private void Awake() {
        Instance = this;
        SetupMainInventory();
    }


    public void SetupMainInventory() {
        List <BaseItem> inventoryItems = InventoryManager.Instance.getInventory();
        GameObject InventoryContainer = _inventoryBox.transform.GetChild(1).gameObject;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            BaseItem item = inventoryItems[i];
            InventoryManager.Instance.SpawnItem(item,InventoryContainer.transform.GetChild(i).gameObject, true);

        }
    }
}

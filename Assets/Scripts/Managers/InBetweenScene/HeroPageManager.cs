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
        List <KeyValuePair<BaseItem,int>> inventoryItems = InventoryManager.Instance.getInventory();
        GameObject InventoryContainer = _inventoryBox.transform.GetChild(1).gameObject;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            KeyValuePair<BaseItem,int> pair = inventoryItems[i];
            InventoryManager.Instance.SpawnItem(pair.Key,InventoryContainer.transform.GetChild(i).gameObject, true,pair.Value);

        }
    }
}

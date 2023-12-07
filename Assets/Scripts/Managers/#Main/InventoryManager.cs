using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<BaseItem> inventory = new List<BaseItem>();

    public GameObject itemPrefab;

    private int maxCapacity = 21;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void addItemToInventory(BaseItem newItem) {
        if(inventory.Count < maxCapacity) {
            inventory.Add(newItem);
        } else {
            Debug.Log("Inventory full");
        }
    }

    public void removeItemFromInventory(BaseItem removingItem) {
        if(inventory.Count > 0) {
            inventory.Remove(removingItem);
        } else {
            Debug.Log("Inventory empty");
        }
    }

    public List<BaseItem> getInventory(){
        return inventory;
    }

    public void SpawnItem(BaseItem item, GameObject targetObject) {
        if (item != null) {
                var itemUnit = Instantiate(itemPrefab, targetObject.transform.position, Quaternion.identity);
                itemUnit.transform.SetParent(targetObject.transform);
                itemUnit.GetComponent<BaseItemPrefab>().SetData(item);
                if(item.image) {
                    itemUnit.transform.GetChild(1).GetComponent<Image>().sprite = item.image;
                    itemUnit.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                }
                itemUnit.GetComponent<TooltipTrigger>().ChangeTooltip(item.displayName,item.description);
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

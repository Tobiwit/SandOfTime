using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<BaseItem> inventory = new List<BaseItem>();

    private int maxCapacity = 20;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<KeyValuePair<BaseItem, int>> inventory = new List<KeyValuePair<BaseItem, int>>();

    public List<BaseItem> startInventory = new List<BaseItem>();

    public GameObject itemPrefab;

    private int maxCapacity = 21;

    private List<Color32> unitColorCodes = new()
    {
            new Color32(223,113,38,255),
            new Color32(118,66,138,255),
            new Color32(63,63,116,255),
            new Color32(230,186,5,255),
            new Color32(172,50,50,255),
            new Color32(1,1,1,255),
            new Color32(1,1,1,255),
        };

    void Awake() {
        if (Instance == null) {
            Instance = this;
            InitializeStartInventory();
        } else {
            Destroy(gameObject);
        }
    }

    public void ClearAllItems() {
        inventory.Clear();
        InitializeStartInventory();
    }

    public void addItemToInventory(BaseItem newItem, int belogingHeroId = 99) {
        if(inventory.Count < maxCapacity) {
            inventory.Add(new KeyValuePair<BaseItem, int>(newItem,belogingHeroId));
        } else {
            Debug.Log("Inventory full");
        }
    }

    public void removeItemFromInventory(BaseItem removingItem, int belogingHeroId = 99) {
        if(inventory.Count > 0) {
            inventory.Remove(new KeyValuePair<BaseItem, int>(removingItem,belogingHeroId));
        } else {
            Debug.Log("Inventory empty");
        }
    }

    public List<BaseItem> getInventoryItems(){
        List<BaseItem> list = new List<BaseItem>();
        foreach (KeyValuePair<BaseItem,int> pair in inventory) {
            list.Add(pair.Key);
        }
        return list;
    }

    public List<KeyValuePair<BaseItem,int>> getInventory(){
        return inventory;
    }

    public List<BaseItem> getInventoryItemsByUnitType(int Id){
        List<BaseItem> list = new List<BaseItem>();
        foreach (KeyValuePair<BaseItem,int> pair in inventory) {
            if (pair.Value == Id) {
                list.Add(pair.Key);
            }
        }
        return list;
    }

    public void SpawnItem(BaseItem item, GameObject targetObject, bool draggable = false, int belongingHero = 99) {
        if (item != null) {
                var itemUnit = Instantiate(itemPrefab, targetObject.transform.position, Quaternion.identity);
                itemUnit.transform.SetParent(targetObject.transform);
                itemUnit.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                itemUnit.GetComponent<BaseItemPrefab>().SetData(item);
                itemUnit.GetComponent<BaseItemPrefab>().belongingHero = belongingHero;
                if(item.image) {
                    itemUnit.transform.GetChild(2).GetComponent<Image>().sprite = item.image;
                    itemUnit.transform.GetChild(2).GetComponent<Image>().gameObject.SetActive(true);
                }
                itemUnit.GetComponent<TooltipTrigger>().ChangeTooltip(item.displayName,item.description);
                if(draggable) {
                    itemUnit.AddComponent<DraggableItem>();
                }

                if(belongingHero > -1 && belongingHero < 10) {
                    itemUnit.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                    //Color32 unitColor = HeroManager.Instance.GetUnitColor(belongingHero);
                    itemUnit.transform.GetChild(0).GetComponent<Image>().color = unitColorCodes[belongingHero];
                }
            }
    }

    public void SetupMainInventory(GameObject slotContainer, bool skipHeroInventory = false) {
        List <KeyValuePair<BaseItem,int>> inventoryItems = InventoryManager.Instance.getInventory();
        
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            KeyValuePair<BaseItem,int> pair = inventoryItems[i];
            if (skipHeroInventory && pair.Value < 10 && pair.Value >= 0) {
                continue;
            }
            InventoryManager.Instance.SpawnItem(pair.Key,slotContainer.transform.GetChild(i).gameObject, true,pair.Value);

        }
    }

    public void AddItem(BaseItem item, int heroId = 99) {
        inventory.Add(new KeyValuePair<BaseItem, int>(item,heroId));
    }

    public void InitializeStartInventory() {
        foreach (BaseItem item in startInventory)
        {
            AddItem(item);
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

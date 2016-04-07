using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int inventorySize;
    public InventoryEntry[] inventory;

    public int hotbarSize;
    public InventoryEntry[] hotbar;
    private int _currentIndex;
    public InventoryEntry currentItem;

    public int currentIndex
    {
        get { return _currentIndex; }
        set
        {
            _currentIndex = value;
            currentItem = inventory[_currentIndex];
        }
    }

    public GameObject hotbarUI;
    public GameObject inventoryUI;

    int nextInventory;
    Image[] hotbarPanels;
    Image[] inventoryPanels;

    // Use this for initialization
    void Start()
    {
        nextInventory = 0;
        inventory = new InventoryEntry[inventorySize];
        hotbar = new InventoryEntry[hotbarSize];

        Transform hotbarPanelTransform = hotbarUI.transform.Find("HotbarPanels");
        hotbarPanels = new Image[hotbarPanelTransform.childCount];
        for (int i = 0; i < hotbarPanels.Length; i++)
        {
            hotbarPanels[i] = hotbarPanelTransform.GetChild(i).gameObject.GetComponent<Image>();
            hotbarPanels[i].gameObject.SetActive(false);
        }

        Transform inventoryPanelTransform = inventoryUI.transform.Find("InventoryPanels");
        Transform[] rowTransforms = new Transform[inventoryPanelTransform.childCount];
        inventoryPanels = new Image[50];
        for (int i = 0; i < 5; i++ )
            for (int j = 0; j < 10; j++) {
                inventoryPanels[j + 10 * i] = inventoryPanelTransform.GetChild(i).GetChild(j).gameObject.GetComponent<Image>();
                inventoryPanels[j + 10*i].gameObject.SetActive(false);
        }

        inventoryUI.SetActive(false);
        
    }

    private void UpdateHotbar(InventoryEntry[] hotbar, InventoryEntry[] inventory)
    {
        int counter = 0;
        for (int i = 0; i < inventorySize && counter < hotbarSize; i++)
            if (inventory[i] != null)
            {
                hotbar[counter] = inventory[i];
                hotbarPanels[counter].gameObject.SetActive(inventoryPanels[i].gameObject.activeSelf);
                hotbarPanels[counter].sprite = inventoryPanels[i].sprite;
                counter++;
            }
        currentItem = inventory[currentIndex];
    }

    /*
     * Adds the given object to the inventory.
     * If there is already an instance of the item in the inventory, simply
     *   increments the count of the entry.
     * If there is no existing instance, will put the object in the first open
     *   slot in the inventory.
     * This will update the hotbar as needed.
     * Will do nothing if there are no more open spots for the object.
     */
    public void Add(InventoryEntry ie)
    {
        // see if an InventoryEntry of the same type and species is in the inventory
        foreach (InventoryEntry entry in inventory)
            if (entry != null && ie.type.Equals(entry.type) && ie.species.Equals(entry.species))
            {
                // if so, increment the count on the existing object
                entry.count++;
                return;
            }
        // if the inventory is full, don't add anything
        if (nextInventory == inventory.Length)
            return;
        // if nothing else, add it to the inventory at the first available spot
        inventory[nextInventory] = ie;
        inventoryPanels[nextInventory].gameObject.SetActive(true);
        inventoryPanels[nextInventory].sprite = ie.sprite;
        // increment the indicator for the next open spot until it finds one
        //   or reaches the end of the inventory
        do
            nextInventory++;
        while (inventory[nextInventory] != null && nextInventory < inventory.Length);
        UpdateHotbar(hotbar, inventory);
    }

    /*
     * Removes an item from the inventory.
     * If there is more than one of the object, simply decrements the
     *    count of the remaining object.
     * If the count drops to zero, the item is removed from the inventory.
     * This will update the hotbar as needed.
     */
    public GameObject Remove(int index)
    {
        InventoryEntry o;
        if (inventory[index] == null)
            return null;
        o = inventory[index];

        // create game object from InventoryEntry
        GameObject plantableObject = (GameObject)Instantiate(Resources.Load(o.prefabName));
        Debug.Log("Removing");
        inventory[index].count--;
        if (inventory[index].count == 0)
        {
            inventory[index] = null;
            inventoryPanels[index].gameObject.SetActive(false);
        }
        if (index < nextInventory)
            nextInventory = index;
        UpdateHotbar(hotbar, inventory);
        return plantableObject;
    }
}

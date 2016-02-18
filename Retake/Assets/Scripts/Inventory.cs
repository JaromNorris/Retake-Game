using UnityEngine;
using System.Collections;
using System;

public class Inventory : MonoBehaviour
{
    public int inventorySize;
    public PlantableObject[] inventory;
    
    public int hotbarSize;
    public PlantableObject[] hotbar;
    public int currentIndex;
    public PlantableObject currentItem;

    int nextInventory;

    // Use this for initialization
    void Start()
    {
        nextInventory = 0;
        inventory = new PlantableObject[inventorySize];
        hotbar = new PlantableObject[hotbarSize];
        UpdateHotbar(hotbar, inventory);
    }

    private void UpdateHotbar(PlantableObject[] hotbar, PlantableObject[] inventory)
    {
        int counter = 0;
        for (int i = 0; i < inventorySize && counter < hotbarSize; i++)
            if (inventory[i] != null)
            {
                hotbar[counter] = inventory[i];
                counter++;
            }
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
    public void Add(PlantableObject o)
    {
        // see if a PlantableObject of the same type and species is in the inventory
        foreach(PlantableObject o2 in inventory)
            if(o.GetType() == o2.GetType() && o.species.Equals(o2.species))
            {
                // if so, increment the count on the existing object
                o2.count++;
                return;
            }
        // if the inventory is full, don't add anything
        if (nextInventory == inventory.Length)
            return;
        // if nothing else, add it to the inventory at the first available spot
        inventory[nextInventory] = o;
        inventory[nextInventory].count = 1;
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
    public PlantableObject Remove(int index)
    {
        PlantableObject o;
        if (inventory[index] == null)
            return null;
        o = inventory[index];
        inventory[index].count--;
        if(inventory[index].count == 0)
            inventory[index] = null;
        UpdateHotbar(hotbar, inventory);
        return o;
    }
}

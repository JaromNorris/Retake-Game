using UnityEngine;
using System.Collections;
using System;

public class Inventory : MonoBehaviour
{
    public PlantableObject[] inventory;
    public int inventorySize;
    public int hotbarSize;
    public Hotbar hotbar;
    int nextInventory;

    public class Hotbar
    {
        public int hotbarSize;
        private int currentIndex;
        public PlantableObject[] items;
        Inventory inventory;

        public Hotbar(int _size)
        {
            hotbarSize = _size;
            items = new PlantableObject[_size];
            currentIndex = 0;
        }

        public Hotbar(int _size, PlantableObject[] _items)
        {
            hotbarSize = _size;
            items = _items;
            currentIndex = 0;
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        public PlantableObject CurrentItem
        {
            get { return items[currentIndex]; }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                CurrentIndex = 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2) && hotbarSize >= 2)
                CurrentIndex = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha3) && hotbarSize >= 3)
                CurrentIndex = 2;
            else if (Input.GetKeyDown(KeyCode.Alpha4) && hotbarSize >= 4)
                CurrentIndex = 3;
            else if (Input.GetKeyDown(KeyCode.Alpha5) && hotbarSize >= 5)
                CurrentIndex = 4;
            else if (Input.GetKeyDown(KeyCode.Alpha6) && hotbarSize >= 6)
                CurrentIndex = 5;
            else if (Input.GetKeyDown(KeyCode.Alpha7) && hotbarSize >= 7)
                CurrentIndex = 6;
            else if (Input.GetKeyDown(KeyCode.Alpha8) && hotbarSize >= 8)
                CurrentIndex = 7;
            else if (Input.GetKeyDown(KeyCode.Alpha9) && hotbarSize >= 9)
                CurrentIndex = 8;
            else if (Input.GetKeyDown(KeyCode.Alpha0) && hotbarSize >= 10)
                CurrentIndex = 9;
        }
    }

    // Use this for initialization
    public Inventory()
    {
        nextInventory = 0;
        inventory = new PlantableObject[inventorySize];
        hotbar = new Hotbar(hotbarSize, GetFirstN(hotbarSize));
    }

    public Inventory(PlantableObject[] _inventory) : this()
    {
        int counter = 0;
        int lastIndex = _inventory.GetUpperBound(0);
        foreach(PlantableObject o in _inventory)
        {
            if (o == null)
                continue;
            if (counter >= inventory.Length)
                break;
            inventory[counter] = o;
            counter++;
        }
        nextInventory = counter;
    }

    public PlantableObject[] GetFirstN(int n)
    {
        PlantableObject[] firstN = new PlantableObject[n];
        Array.Copy(inventory, 0, firstN, 0, n);
        return firstN;
    }

    public void Add(PlantableObject o)
    {
        int index;
        if ((index = Array.IndexOf(inventory, o)) != -1)
        {
            inventory[index].count += 1;
            return;
        }
        if (nextInventory == inventory.Length)
            return;
        inventory[nextInventory] = o;
        while (inventory[nextInventory] != null && nextInventory < inventory.Length)
            nextInventory++;
    }

    public PlantableObject Remove(int index)
    {
        PlantableObject o;
        if (inventory[index] == null)
            return null;
        o = inventory[index];
        inventory[index].count--;
        if(inventory[index].count == 0)
            inventory[index] = null;
        return o;
    }

    void Update()
    {

    }
}

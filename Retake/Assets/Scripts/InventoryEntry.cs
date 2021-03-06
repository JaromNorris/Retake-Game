﻿using UnityEngine;
using System.Collections;

public class InventoryEntry : ScriptableObject
{
    public string prefabName;
    public string species;
    public int currentSize;
    public int maxSize;
    public int count = 0;
    public string type;
    public Sprite sprite;

    public void SetInventoryEntry(PlantableObject _obj)
    {
        prefabName = _obj.prefabName;
        species = _obj.species;
        currentSize = _obj.currentSize;
        maxSize = _obj.maxSize;
        type = _obj.Type;
        count = 1;
        sprite = _obj.sprite;
    }

    public void SetInventoryEntry(New_Seed _seed)
    {
        prefabName = _seed.name;
		species = prefabName;
        count = _seed.number_of;
		type = "seed";
        sprite = _seed.sprite;
    }
}

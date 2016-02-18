using UnityEngine;
using System.Collections;

public class InventoryEntry : ScriptableObject
{

    public string species;
    public int currentSize;
    public int maxSize;
    public int count = 0;
    public string type;

    public void SetInventoryEntry(PlantableObject _obj)
    {
        species = _obj.species;
        currentSize = _obj.currentSize;
        maxSize = _obj.maxSize;
        type = _obj.Type;
    }
}

using UnityEngine;
using System.Collections;

public abstract class PlantableObject : MonoBehaviour
{
    public string species;
    public int currentSize;
    public int maxSize;
    public int lightRequired;
    public int waterRequired;
    public Plantable_Space currentSpace;

    public abstract string Type { get; }
}

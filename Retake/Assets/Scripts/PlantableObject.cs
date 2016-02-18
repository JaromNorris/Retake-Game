using UnityEngine;
using System.Collections;

public abstract class PlantableObject : MonoBehaviour
{
    public abstract string Species { get; set; }
    public abstract int CurrentSize { get; set; }
    public abstract int MaxSize { get; set; }
    public abstract int LightRequired { get; set; }
    public abstract int WaterRequired { get; set; }
    public abstract Plantable_Space CurrentSpace { get; set; }

    public int count = 0;
}

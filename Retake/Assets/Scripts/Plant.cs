using UnityEngine;
using System.Collections;

public class Plant : PlantableObject
{
    private string species;
    private int current_size;
    private int max_size;
    private int water_required;
    private int light_required;
    private Plantable_Space current_space;

    public override string Species
    {
        get { return species; }
        set { species = value; }
    }

    public override int CurrentSize
    {
        get { return current_size; }
        set { current_size = value; }
    }

    public override int MaxSize
    {
        get { return max_size; }
        set { max_size = value; }
    }

    public override int LightRequired
    {
        get { return light_required; }
        set { light_required = value; }
    }

    public override int WaterRequired
    {
        get { return water_required; }
        set { water_required = value; }
    }

    public override Plantable_Space CurrentSpace
    {
        get { return current_space; }
        set { current_space = value; }
    }
}

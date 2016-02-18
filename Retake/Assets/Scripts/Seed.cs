using UnityEngine;
using System.Collections;

/*Currently does nothing but list information
 * Most likely plantableSpace will handle changing its information/visuals
 * */
public class Seed : PlantableObject
{
    public override string Type
    {
        get { return "Seed"; }
    }
}

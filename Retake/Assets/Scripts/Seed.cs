using UnityEngine;
using System.Collections;

/*Currently does nothing but list information
 * Most likely plantableSpace will handle changing its information/visuals
 * */
public class Seed : MonoBehaviour {
	public string species;
	public int max_growth_size;

	public int water_required;
	public int light_required;

	public Plantable_Space current_space;
}

using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {
	public string species;
	public int current_size;
	public int max_size;
	
	public int water_required;
	public int light_required;

	public Plantable_Space current_space;
}

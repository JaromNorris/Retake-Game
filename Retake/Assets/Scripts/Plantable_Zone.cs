using UnityEngine;
using System.Collections;

public class Plantable_Zone : MonoBehaviour
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public Plantable_Space[] spaces;

	void Start()
	{
		spaces = GetComponentsInChildren<Plantable_Space>();
	}
		
}

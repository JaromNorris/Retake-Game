using UnityEngine;
using System.Collections;

public class ClimbTest : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		Debug.Log ("Tada");
		if(col.gameObject.tag == "Ladder")
		{
			Debug.Log("Working");
			Destroy(col.gameObject);
		}
	}
}
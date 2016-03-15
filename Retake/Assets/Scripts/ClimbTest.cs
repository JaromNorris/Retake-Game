using UnityEngine;
using System.Collections;

public class ClimbTest : MonoBehaviour
{
	public bool canClimb; 

	void OnTriggerEnter (Collider col)
	{
		Debug.Log ("Tada");
		if(col.gameObject.tag == "Ladder")
		{
			canClimb = true;
			//Debug.Log(gameObject.m_MoveDir);
		}
	}
	void OnTriggerExit (Collider col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			canClimb = false;
			Debug.Log ("Disconnect");
		}
	}
}
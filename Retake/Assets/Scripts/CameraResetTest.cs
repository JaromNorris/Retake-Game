using UnityEngine;
using System.Collections;

public class CameraResetTest : MonoBehaviour {

	public Vector3 StartPosition;
	public Vector3 StartRotation;
	float Var1 = .03f;
	float Var2 = .62f;
	float Var3 = -2.569f;
	//GameObject Char = gameObject.parent;
	// Use this for initialization
	void Start () {
		Vector3 StartPosition = new Vector3 (this.transform.position.x,this.transform.position.y,this.transform.position.z);
		Vector3 StartRotation = new Vector3 (this.transform.rotation.x,this.transform.rotation.y,this.transform.rotation.z);
	}
	
	// Update is called once per frame
	void OnCollisionExit (Collision col){
		new WaitForSeconds (2);
		Debug.Log ("TriggeredAfterWait");
		this.transform.localPosition = new Vector3 (Var1, Var2, Var3);
		//this.transform.localPosition = StartPosition;
		this.transform.localRotation = Quaternion.Euler(StartRotation);
	}
	void Update () {
		//transform.position = StartPosition;
		//this.transform.localPosition = StartPosition;
		//this.transform.localRotation = Quaternion.Euler(StartRotation);
		//Debug.Log (StartPosition);
	}
}

using UnityEngine;
using System.Collections;

public class ThirdPCameraScript : MonoBehaviour {

	public bool fpsMode = true;
	public Camera FirstPCamera;
	public Camera ThirdPCamera; 


		
		// Use this for initialization
	void Start () {
		gameObject.tag = "ThirdPersonCamera";
		FirstPCamera.enabled = true;
		ThirdPCamera.enabled = false;
		//GameObject.Find ("Main Camera").tag = "Untagged";
		//GameObject.FindGameObjectWithTag ("MainCamera").name = "FirstPCamera";
		//GameObject.FindGameObjectWithTag ("ThirdPersonCamera").name = "ThirdPCamera";

	}

	// Update is called once per frame
	void Update () {
	if(Input.GetKeyUp("f")) {
			fpsMode = !fpsMode;
			if (fpsMode) {
				Debug.Log("First Person.?");
				FirstPCamera.gameObject.tag = "MainCamera"; 
				ThirdPCamera.gameObject.tag = "ThirdPersonCamera";
				ThirdPCamera.enabled = false;
				FirstPCamera.enabled = true;
				//GameObject.FindGameObjectWithTag("MainCamera").tag = "ThirdPersonCamera";
				//GameObject.FindGameObjectWithTag("FirstPersonCamera").tag = "MainCamera";
			} else if (!fpsMode) {
				Debug.Log ("Shouldn't Be FPS");
				//GameObject.Find("FirstPCamera").tag = "FirstPersonCamera"; 
				//GameObject.Find ("ThirdPCamera").tag = "MainCamera";
				FirstPCamera.gameObject.tag = "FirstPersonCamera";
				ThirdPCamera.gameObject.tag = "MainCamera";
				ThirdPCamera.enabled = true;
				FirstPCamera.enabled = false;
				//GameObject.FindGameObjectWithTag ("FirstPCamera").tag = "FirstPersonCamera";
				//GameObject.FindGameObjectWithTag ("ThirdPe").tag = "MainCamera";
				}
		}
		
	}
}
